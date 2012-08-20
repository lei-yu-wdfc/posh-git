using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace Wonga.QA.Generators.Core
{
	public class SchemaToTypesGenerator
	{
		private readonly XmlSchema _schema;
		private readonly DirectoryInfo _directory = new DirectoryInfo(Directory.GetCurrentDirectory());

		public SchemaToTypesGenerator(FileInfo schema)
		{
			_schema = GetValidatedSchema(schema);
		}

		public Dictionary<string, Type> GenerateTypes()
		{
			CreateDirectoryIfDoesntExist(_directory.FullName);
			return GetSchemaTypes(_schema);
		}
		
		public Dictionary<string, IEnumerable<XmlSchemaElement>> GenerateNamespaceMessagesPairs()
		{
			CreateDirectoryIfDoesntExist(_directory.FullName);
			return GetNamespaceMessagesDictionary(_schema);
		}

		private static void CreateDirectoryIfDoesntExist(String directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		private XmlSchema GetValidatedSchema(FileInfo schemaFile)
		{
			var schema = ConvertFileToSchema(schemaFile);
			ValidateSchema(schema);
			return schema;
		}

		private static Dictionary<string, IEnumerable<XmlSchemaElement>> GetNamespaceMessagesDictionary(XmlSchema schema)
		{
			var nameMessagesPairs = new Dictionary<string, IEnumerable<XmlSchemaElement>>();
			var groups = schema.Items.OfType<XmlSchemaGroup>().Where(a => a.Name.Contains("Queries") || a.Name.Contains("Commands"));

			foreach (var group in groups)
			{
				var messages = group.Particle.Items.OfType<XmlSchemaElement>().Where(a => !a.Name.Contains("Response"));
				nameMessagesPairs.Add(group.Name, messages);
			}

			return nameMessagesPairs;
		}

		private Dictionary<String, Type> GetSchemaTypes(XmlSchema schema)
		{
			var rootElement = schema.Items.OfType<XmlSchemaElement>().First();
			var results = CompileGeneratedClassFileForXmlSchemaElement(_directory, rootElement, schema);
			return results.CompiledAssembly.GetTypes().Where(a => a.Name.Contains("Messages") && !a.Name.Contains("Response")).ToDictionary(t => t.GetName());
		}

		private XmlSchema ConvertFileToSchema(FileInfo file)
		{
			using (XmlReader reader = XmlReader.Create(file.FullName))
				return XmlSchema.Read(reader, (s, a) => { throw a.Exception; });
		}

		private void ValidateSchema(XmlSchema schema)
		{
			try
			{
				var set = new XmlSchemaSet();
				set.Add(schema);
				set.Compile();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		
		private CompilerResults CompileGeneratedClassFileForXmlSchemaElement(DirectoryInfo codeDirectory, XmlSchemaElement element, XmlSchema schema)
		{
			var ns = new CodeNamespace();
			var exporter = new XmlCodeExporter(ns);
			var importer = new XmlSchemaImporter(new XmlSchemas { schema });
			exporter.ExportTypeMapping(importer.ImportTypeMapping(element.QualifiedName));

			FileInfo classFile = Repo.File(String.Format("{0}.cs", element.Name), codeDirectory, true);

			var provider = new CSharpCodeProvider();
			using (StreamWriter writer = classFile.CreateText())
			{
				provider.GenerateCodeFromNamespace(ns, writer, null);
			}

			CompilerResults results =
				provider.CompileAssemblyFromFile(
					new CompilerParameters(new[] { "System.dll", "System.Xml.dll" }) { GenerateInMemory = true }, classFile.FullName);

			results.Output.Cast<String>().ForEach(Console.WriteLine);
			if (results.Errors.HasErrors)
				throw new Exception(String.Join(Environment.NewLine, results.Errors.Cast<CompilerError>()));

			return results;
		}
	}
}

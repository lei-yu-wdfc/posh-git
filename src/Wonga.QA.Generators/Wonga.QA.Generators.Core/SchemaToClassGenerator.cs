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
	public static class SchemaToClassGenerator
	{
		private static DirectoryInfo _directory = new DirectoryInfo("c:/Api");

		public static IEnumerable<MessageClassDefinition> Generate(FileInfo schemaFile, DirectoryInfo directory)
		{
			_directory = directory;

			CreateDirectoryIfDoesntExist(_directory.FullName);

			var schema = GetValidatedSchema(schemaFile);
			var messages = GetNamespaceMessagesDictionary(schema);
			var types = GetSchemaTypes(schema);
			
			return ClassBuilder.Build(messages, types, _directory);
		}

		private static void CreateDirectoryIfDoesntExist(String directory)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		private static XmlSchema GetValidatedSchema(FileInfo schemaFile)
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

		private static Dictionary<String, Type> GetSchemaTypes(XmlSchema schema)
		{
			var rootElement = schema.Items.OfType<XmlSchemaElement>().First();
			var results = CompileGeneratedClassFileForXmlSchemaElement(_directory, rootElement, schema);
			return results.CompiledAssembly.GetTypes().Where(a => a.Name.Contains("Messages") && !a.Name.Contains("Response")).ToDictionary(t => t.GetName());
		}

		private static XmlSchema ConvertFileToSchema(FileInfo file)
		{
			using (XmlReader reader = XmlReader.Create(file.FullName))
				return XmlSchema.Read(reader, (s, a) => { throw a.Exception; });
		}

		private static void ValidateSchema(XmlSchema schema)
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
		
		private static CompilerResults CompileGeneratedClassFileForXmlSchemaElement(DirectoryInfo codeDirectory, XmlSchemaElement element, XmlSchema schema)
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

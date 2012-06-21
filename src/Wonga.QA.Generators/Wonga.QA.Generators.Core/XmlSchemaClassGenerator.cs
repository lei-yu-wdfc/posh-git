using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.CSharp;
using QAFramework = Wonga.QA.Generators.Core.Config.Framework;

namespace Wonga.QA.Generators.Core
{
	public class XmlSchemaClassGenerator
	{
		private bool _errorsOccurred;
		
		public bool ContinueOnError { get; private set; }

		public GeneratorRepoDirectories BinRootDirectories { get; private set; }

		public IGenerateEnum EnumGenerator { get; private set; }

		public QAFramework Framework { get; private set; }

		public bool ErrorsOccurred
		{
			get { return _errorsOccurred || EnumGenerator.ErrorsOccurred; }
		}

		public XmlSchemaClassGenerator(QAFramework framework, GeneratorRepoDirectories binRootDirectories, bool continueOnError = true)
		{
			_errorsOccurred = false;
			Framework = framework;
			BinRootDirectories = binRootDirectories;
			ContinueOnError = continueOnError;
			EnumGenerator = new EnumGenerator(EnumGenerationMode.UseNormalTypeNameWithDescription, null, continueOnError);
		}

		public void GenerateXmlSchemaClassesFiles(FileInfo xmlSchemaFile, ILookup<String, Type> typesToGenerate)
		{
			try
			{
				GenerateXmlSchemaClassesFilesInternal(xmlSchemaFile, typesToGenerate);
			}
			catch (Exception e)
			{
				_errorsOccurred = true;
				Console.Error.WriteLine("\t*** FAILED GENERATION FOR SCHEMA: {0}. {1}", xmlSchemaFile.FullName, e.Message);
				if(!ContinueOnError)
				{
					throw;
				}
			}
		}


		private void GenerateXmlSchemaClassesFilesInternal(FileInfo xmlSchemaFile, ILookup<String, Type> typesToGenerate)
		{
			Console.WriteLine(xmlSchemaFile.FullName);

			XmlSchema schema = xmlSchemaFile.GetSchema();

			var set = new XmlSchemaSet();
			set.Add(schema);
			set.Compile();

			XmlSchemaElement[] elements = schema.Items.OfType<XmlSchemaElement>().ToArray();

			elements.Where(e => e.SchemaType == null && e.SchemaTypeName == XmlQualifiedName.Empty).ForEach(
				e => e.SchemaType = new XmlSchemaComplexType());

			DirectoryInfo codeDirectory = Repo.Directory(xmlSchemaFile.GetName(), BinRootDirectories.CodeDirectory);

			foreach (XmlSchemaElement element in elements)
			{
				if (!typesToGenerate.Contains(element.Name))
					continue;

				var results = GenerateClassFileForXmlSchemaElement(codeDirectory, element, schema);
				
				Dictionary<String, Type> types = results.CompiledAssembly.GetTypes().ToDictionary(t => t.GetName());

				String className = String.Format("{0}{1}{2}{3}", types[element.Name].GetClean(), xmlSchemaFile.GetProduct(),
												 xmlSchemaFile.GetRegion(), typesToGenerate[element.Name].First().GetSuffix());
				FileInfo code = Repo.File(String.Format("{0}.cs", className), BinRootDirectories.ClassesDirectory);

				//TODO: BEGIN update these vars to split messages through different folders to avoid name collision
				String classNamespace = Framework.Project;
				String classFullName = String.Format("{0}.{1}", xmlSchemaFile.GetName(), element.Name);
				//string generatedEnumNamespace = string.Format("{0}.{1}.{2}", Framework.Project, EnumsDirectoryName, EnumNamespaceRelativePath);
				string generatedEnumNamespace = Framework.Project;
				string enumSubfolderName = "";
				//TODO: END update these vars to split messages through different folders to avoid name collision

				var classBuilder = InitializeClassDefinition(className, element, classFullName, classNamespace);

				foreach (PropertyInfo property in types[element.Name].GetProperties().Where(p => !p.IsIgnore()))
					classBuilder.AppendFormatLine("        public Object {0} {{ get; set; }}", property.GetName());

				classBuilder.AppendLine("    }").AppendLine("}");

				EnumGenerator.StartEnumGenerationForClass(classNamespace);

				foreach (Type type in results.CompiledAssembly.GetTypes().Where(t => t.IsEnum))
				{
					EnumGenerator.GenerateAllEnumsUsedByClassMember(
									type, generatedEnumNamespace,
									BinRootDirectories.EnumsDirectory, enumSubfolderName);
				}

				//now insert all the include directives at the begining of the file!!!!
				InsertUsingDirectivesOnClassDefinition(classBuilder, EnumGenerator.GetEnumUsingDirectivesForCurrentClass());

				using (StreamWriter writer = code.CreateText())
					writer.Write(classBuilder);

				Console.WriteLine("\t{0} \u2192 {1}", element.Name, code.Name);
			}
		}

		private static CompilerResults GenerateClassFileForXmlSchemaElement(DirectoryInfo codeDirectory, XmlSchemaElement element, XmlSchema schema)
		{
			var ns = new CodeNamespace();
			var exporter = new XmlCodeExporter(ns);
			var importer = new XmlSchemaImporter(new XmlSchemas {schema});
			exporter.ExportTypeMapping(importer.ImportTypeMapping(element.QualifiedName));

			FileInfo classFile = Repo.File(String.Format("{0}.cs", element.Name), codeDirectory);

			var provider = new CSharpCodeProvider();
			using (StreamWriter writer = classFile.CreateText())
			{
				provider.GenerateCodeFromNamespace(ns, writer, null);
			}

			CompilerResults results =
				provider.CompileAssemblyFromFile(
					new CompilerParameters(new[] {"System.dll", "System.Xml.dll"}) {GenerateInMemory = true}, classFile.FullName);

			results.Output.Cast<String>().ForEach(Console.WriteLine);
			if (results.Errors.HasErrors)
				throw new Exception(String.Join(Environment.NewLine, results.Errors.Cast<CompilerError>()));

			return results;
		}

		private StringBuilder InitializeClassDefinition(string className, XmlSchemaElement element, string classFullName, string classNamespace)
		{
			StringBuilder builder = new StringBuilder().AppendFormatLine(new[]
    		                                                              	{
    		                                                              		"namespace {0}",
    		                                                              		"{{",
    		                                                              		"    /// <summary> {1} </summary>",
    		                                                              		"    [XmlRoot({2})]",
    		                                                              		"    public partial class {3} : {4}<{3}>",
    		                                                              		"    {{",
    		                                                              	},
																		  classNamespace,
																		  classFullName,
																		  element.Name.Quote(),
																		  className,
																		  Framework.Base);
			return builder;
		}

		private static void InsertUsingDirectivesOnClassDefinition(StringBuilder builder, string enumUsingDirectives)
		{
			StringBuilder usingDirectivesBuilder = new StringBuilder()
				.AppendLine("using System;")
				.AppendLine("using System.Xml.Serialization;")
				.AppendLine("")
				.Append(enumUsingDirectives);

			builder.Insert(0, usingDirectivesBuilder.ToString());
		}
	}
}

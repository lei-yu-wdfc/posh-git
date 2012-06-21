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
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
    public class ApiGenerator
    {
		public const string RequestsDirectoryName = "Requests";
		public const string CodeDirectoryName = "Code";
		public const string EnumsDirectoryName = "Enums";

        public static void Main(String[] args)
        {
            if (args.Any())
                Config.Origin = args.Single();

			bool errorsOccurred = false;

			var binRootDirectories = new
            {
				Requests = Repo.Directory(RequestsDirectoryName),
				Code = Repo.Directory(CodeDirectoryName),
				Enums = Repo.Directory(EnumsDirectoryName)
            };

            ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

			var enumGenerator = new EnumGenerator(EnumGenerationMode.UseNormalTypeNameWithDescription, null);

            foreach (FileInfo file in Origin.GetSchemas().Where(f => !f.IsCs()))
            {
				Console.WriteLine(file.FullName);

				try
				{

					XmlSchema schema = file.GetSchema();

					XmlSchemaSet set = new XmlSchemaSet();
					set.Add(schema);
					set.Compile();

					XmlSchemaElement[] elements = schema.Items.OfType<XmlSchemaElement>().ToArray();

					elements.Where(e => e.SchemaType == null && e.SchemaTypeName == XmlQualifiedName.Empty).ForEach(
						e => e.SchemaType = new XmlSchemaComplexType());

					DirectoryInfo codeDirectory = Repo.Directory(file.GetName(), binRootDirectories.Code);

					foreach (XmlSchemaElement element in elements)
					{
						if (!requests.Contains(element.Name))
							continue;


						CodeNamespace ns = new CodeNamespace();
						XmlCodeExporter exporter = new XmlCodeExporter(ns);
						XmlSchemaImporter importer = new XmlSchemaImporter(new XmlSchemas {schema});
						exporter.ExportTypeMapping(importer.ImportTypeMapping(element.QualifiedName));

						FileInfo classFile = Repo.File(String.Format("{0}.cs", element.Name), codeDirectory);

						CSharpCodeProvider provider = new CSharpCodeProvider();
						using (StreamWriter writer = classFile.CreateText())
							provider.GenerateCodeFromNamespace(ns, writer, null);

						CompilerResults results =
							provider.CompileAssemblyFromFile(
								new CompilerParameters(new[] {"System.dll", "System.Xml.dll"}) {GenerateInMemory = true}, classFile.FullName);
						results.Output.Cast<String>().ForEach(Console.WriteLine);
						if (results.Errors.HasErrors)
							throw new Exception(String.Join(Environment.NewLine, results.Errors.Cast<CompilerError>()));

						Dictionary<String, Type> types = results.CompiledAssembly.GetTypes().ToDictionary(t => t.GetName());

						String className = String.Format("{0}{1}{2}{3}", types[element.Name].GetClean(), file.GetProduct(),
						                                 file.GetRegion(), requests[element.Name].First().GetSuffix());
						FileInfo code = Repo.File(String.Format("{0}.cs", className), binRootDirectories.Requests);

						//TODO: BEGIN update to tree ns
						String classNamespace = Config.Api.Project;
						String classFullName = String.Format("{0}.{1}", file.GetName(), element.Name);
						//string generatedEnumNamespace = string.Format("{0}.{1}.{2}", Config.Api.Project, EnumsDirectoryName, messageClassNamespaceRelativePath);
						string generatedEnumNamespace = Config.Api.Project;
						string enumSubfolderName = "";
						//TODO: END update to tree ns

						var classBuilder = InitializeClassDefinition(className, element, classFullName, classNamespace);

						foreach (PropertyInfo property in types[element.Name].GetProperties().Where(p => !p.IsIgnore()))
							classBuilder.AppendFormatLine("        public Object {0} {{ get; set; }}", property.GetName());

						classBuilder.AppendLine("    }").AppendLine("}");

						enumGenerator.StartEnumGenerationForClass(classNamespace);

						foreach (Type type in results.CompiledAssembly.GetTypes().Where(t => t.IsEnum))
						{
							enumGenerator.GenerateAllEnumsUsedByClassMember(type, generatedEnumNamespace, binRootDirectories.Enums,enumSubfolderName);
						}

						//now insert all the include directives at the begining of the file!!!!
						InsertUsingDirectivesOnClassDefinition(classBuilder, enumGenerator.GetEnumUsingDirectivesForCurrentClass());

						using (StreamWriter writer = code.CreateText())
							writer.Write(classBuilder);

						Console.WriteLine("\t{0} \u2192 {1}", element.Name, code.Name);
					}
				}
				catch (Exception e)
				{
					Console.Error.WriteLine("\t*** FAILED GENERATION FOR SCHEMA: {0}. {1}", file.FullName, e.Message);
					errorsOccurred = true;
				}
                
            }//for each schema

			if (errorsOccurred || enumGenerator.ErrorsOccurred)
			{
				Console.Error.WriteLine("*** THERE WHERE ERRORS DURING GENERATION... NOT UPDATING QAF!!!!!");
				return;
			}

			Repo.Inject(binRootDirectories.Requests, Config.Api.Folder, Config.Api.Project);
            Repo.Inject(binRootDirectories.Enums, EnumsDirectoryName, Config.Api.Project);

        }

    	private static StringBuilder InitializeClassDefinition(string className, XmlSchemaElement element, string classFullName, string classNamespace)
    	{
    		StringBuilder builder = new StringBuilder().AppendFormatLine(new[]
    		                                                              	{
    		                                                              		"namespace {0}",
    		                                                              		"{{",
    		                                                              		"    /// <summary> {1} </summary>",
    		                                                              		"    [XmlRoot({2})]",
    		                                                              		"    public partial class {3} : ApiRequest<{3}>",
    		                                                              		"    {{",
    		                                                              	},
    		                                                              classNamespace,
    		                                                              classFullName,
    		                                                              element.Name.Quote(),
    		                                                              className);
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
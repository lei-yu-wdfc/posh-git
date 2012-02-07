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
        public static void Main(String[] args)
        {
            if (args.FirstOrDefault() != null)
                Config.Origin = args.First();

            var bin = new
            {
                Requests = Repo.Directory("Requests"),
                Code = Repo.Directory("Code"),
                Enums = Repo.Directory("Enums")
            };

            ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

            Dictionary<String, String[]> enums = new Dictionary<String, String[]>();

            foreach (FileInfo file in Origin.GetSchemas())
            {
                Console.WriteLine(file.Name);

                XmlSchema schema = file.GetSchema();

                XmlSchemaSet set = new XmlSchemaSet();
                set.Add(schema);
                set.Compile();

                XmlSchemaElement[] elements = schema.Items.OfType<XmlSchemaElement>().ToArray();

                elements.Where(e => e.SchemaType == null && e.SchemaTypeName == XmlQualifiedName.Empty).ForEach(e => e.SchemaType = new XmlSchemaComplexType());
                
                DirectoryInfo code3 = Repo.Directory(file.GetName(), bin.Code);

                foreach (XmlSchemaElement element in elements)
                {
                    if (!requests.Contains(element.Name))
                        continue;

                    CodeNamespace ns = new CodeNamespace();
                    XmlCodeExporter exporter = new XmlCodeExporter(ns);
                    XmlSchemaImporter importer = new XmlSchemaImporter(new XmlSchemas { schema });
                    exporter.ExportTypeMapping(importer.ImportTypeMapping(element.QualifiedName));

                    FileInfo code2 = Repo.File(String.Format("{0}.cs", element.Name), code3);

                    CSharpCodeProvider provider = new CSharpCodeProvider();
                    using (StreamWriter writer = code2.CreateText())
                        provider.GenerateCodeFromNamespace(ns, writer, null);

                    CompilerResults results = provider.CompileAssemblyFromFile(new CompilerParameters(new[] { "System.dll", "System.Xml.dll" }) { GenerateInMemory = true }, code2.FullName);
                    results.Output.Cast<String>().ForEach(Console.WriteLine);
                    if (results.Errors.HasErrors)
                        throw new Exception(String.Join(Environment.NewLine, results.Errors.Cast<CompilerError>()));

                    Dictionary<String, Type> types = results.CompiledAssembly.GetTypes().ToDictionary(t => t.GetName());

                    String name1 = String.Format("{0}{1}{2}{3}", types[element.Name].GetClean(), file.GetProduct(), file.GetRegion(), requests[element.Name].First().GetSuffix());
                    FileInfo code = Repo.File(String.Format("{0}.cs", name1), bin.Requests);

                    StringBuilder builder1 = new StringBuilder().AppendFormatLine(new[]{
			            "using System;",
			            "using System.Xml.Serialization;",
			            "",
			            "namespace {0}",
			            "{{",
			            "    [XmlRoot({1})]",
			            "    public partial class {2} : ApiRequest<{2}>",
			            "    {{",
		            }, Config.Api.Project, element.Name.Quote(), name1);

                    foreach (PropertyInfo property in types[element.Name].GetProperties().Where(p => !p.IsIgnore()))
                        builder1.AppendFormatLine("        public Object {0} {{ get; set; }}", property.GetName());

                    builder1.AppendLine("    }").AppendLine("}");

                    using (StreamWriter writer = code.CreateText())
                        writer.Write(builder1);

                    Console.WriteLine("\t{0} \u2192 {1}", element.Name, code.Name);

                    foreach (Type type in results.CompiledAssembly.GetTypes().Where(t => t.IsEnum))
                    {
                        String name = type.GetName().ToEnum().ToCamel();
                        String[] values = Enum.GetNames(type).Select(e => type.GetField(e).GetEnum()).Distinct().ToArray();

                        if (enums.ContainsKey(name))
                            if (enums[name].SequenceEqual(values))
                                continue;
                            else
                                throw new NotImplementedException();
                        enums.Add(name, values);

                        FileInfo fenum = Repo.File(String.Format("{0}.cs", name), bin.Enums);

                        StringBuilder builder = new StringBuilder().AppendFormatLine(new[]
	                    {
		                    "namespace {0}",
		                    "{{",
		                    "    public enum {1}",
		                    "    {{"
	                    }, Config.Api.Project, name);

                        foreach (String value in values)
                            builder.AppendFormatLine("        {0},", value);

                        builder.AppendLine("    }").AppendLine("}");

                        using (StreamWriter writer = fenum.CreateText())
                            writer.Write(builder);

                        Console.WriteLine("\t{0} \u2192 {1}", type.Name, fenum.Name);
                    }
                }
            }

            Repo.Inject(bin.Requests, Config.Api.Folder, Config.Api.Project);
            Repo.Inject(bin.Enums, "Enums", Config.Api.Project);
			
        }
    }
}

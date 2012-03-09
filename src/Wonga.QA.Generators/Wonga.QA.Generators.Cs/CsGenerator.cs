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

namespace Wonga.QA.Generators.Cs
{
    public class CsGenerator
    {
        public static void Main(String[] args)
        {
            if (args.Any())
                Config.Origin = args.Single();

            var bin = new
            {
                Requests = Repo.Directory("Requests"),
                Code = Repo.Directory("Code"),
            };

            ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

            foreach (FileInfo file in Origin.GetSchemas().Where(f => f.IsCs()))
            {
                Console.WriteLine(file.Name);

                XmlSchema schema = file.GetSchema();

                XmlSchemaSet set = new XmlSchemaSet();
                set.Add(schema);
                set.Compile();

                XmlSchemaElement[] elements = schema.Items.OfType<XmlSchemaElement>().ToArray();
                elements.Where(e => e.SchemaType == null && e.SchemaTypeName == XmlQualifiedName.Empty).ForEach(e => e.SchemaType = new XmlSchemaComplexType());

                DirectoryInfo directory = Repo.Directory(file.GetName(), bin.Code);

                foreach (XmlSchemaElement element in elements)
                {
                    if (!requests.Contains(element.Name))
                        continue;

                    Console.Write("\t{0} \u2192 ", element.Name);

                    CodeNamespace ns = new CodeNamespace();
                    XmlCodeExporter exporter = new XmlCodeExporter(ns);
                    XmlSchemaImporter importer = new XmlSchemaImporter(new XmlSchemas { schema });
                    exporter.ExportTypeMapping(importer.ImportTypeMapping(element.QualifiedName));

                    FileInfo code = Repo.File(String.Format("{0}.cs", element.Name), directory);
                    CSharpCodeProvider provider = new CSharpCodeProvider();
                    using (StreamWriter writer = code.CreateText())
                        provider.GenerateCodeFromNamespace(ns, writer, null);

                    CompilerResults results = provider.CompileAssemblyFromFile(new CompilerParameters(new[] { "System.dll", "System.Xml.dll" }) { GenerateInMemory = true }, code.FullName);
                    results.Output.Cast<String>().ForEach(Console.WriteLine);
                    if (results.Errors.HasErrors)
                        throw new Exception(String.Join(Environment.NewLine, results.Errors.Cast<CompilerError>()));

                    Dictionary<String, Type> types = results.CompiledAssembly.GetTypes().ToDictionary(t => t.GetName());
                    String name = String.Format("{0}{1}{2}{3}", types[element.Name].GetClean(), file.GetProduct(), file.GetRegion(), requests[element.Name].First().GetSuffix());
                    FileInfo source = Repo.File(String.Format("{0}.cs", name), bin.Requests);

                    StringBuilder builder = new StringBuilder().AppendFormatLine(new[]{
                        "using System;",
                        "using System.Xml.Serialization;",
                        "",
                        "namespace {0}",
                        "{{",
                        "    /// <summary> {1} </summary>",
                        "    [XmlRoot({2})]",
                        "    public partial class {3} : CsRequest<{3}>",
                        "    {{",
                    }, "Wonga.QA.Framework.Cs", String.Format("{0}.{1}", file.GetName(), element.Name), element.Name.Quote(), name);

                    foreach (PropertyInfo property in types[element.Name].GetProperties().Where(p => !p.IsIgnore()))
                        builder.AppendFormatLine("        public Object {0} {{ get; set; }}", property.GetName());

                    builder.AppendLine("    }").AppendLine("}");

                    using (StreamWriter writer = source.CreateText())
                        writer.Write(builder);

                    Console.WriteLine(source.Name);

                    //TODO enums
                }

                Repo.Inject(bin.Requests, "Requests", "Wonga.QA.Framework.Cs");
            }
        }
    }
}

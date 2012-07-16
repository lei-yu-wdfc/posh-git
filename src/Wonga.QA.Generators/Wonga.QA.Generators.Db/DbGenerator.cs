using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Db.Split
{
    public class DbGenerator
    {
        public static void Main(String[] args)
        {
            var server = args.Length >= 1 ? args[0] : "localhost";
            Config.Databases = args.Length >= 2 ? new[] {args[1]} : Config.Databases;

            var bin = new
            {
                Dbml = Repo.Directory("Dbml", false),
                Code = Repo.Directory("Code", false)
            };
            

            foreach (String database in Config.Databases)
            {
                Console.WriteLine(database);

                var file = new
                {
                    Dbml = Repo.File(String.Format("{0}Database.dbml", database), bin.Dbml, true),
                    Code = Repo.File(String.Format("{0}Database.cs", database), bin.Code, true)
                };

                Console.WriteLine("\t{0}", file.Dbml.Name);
                Command.Run(Config.SqlMetal.FullName, 3, new[]
                {
                    "/database:{0}",
                    "/dbml:{1}",
                    "/pluralize",
                    "/server:"+server
                }, database, file.Dbml.FullName.Quote());

                XElement root = XDocument.Load(file.Dbml.FullName).Root;
                XNamespace ns = root.GetDefaultNamespace();
                List<XAttribute> attributes = root.Descendants().SelectMany(d => d.Attributes()).ToList();

                foreach (XElement table in root.Descendants(ns.GetName("Table")))
                {
                    String name = table.Attribute("Name").Value;
                    String schema = name.Substring(0, name.IndexOf('.')).ToLower() + '_';

                    XAttribute member = table.Attribute("Member");
                    if (member.Value.ToLower().StartsWith(schema))
                    {
                        String value = member.Value.Substring(schema.Length);
                        attributes.Where(a => a.Value == member.Value).ForEach(a => a.SetValue(value));
                    }

                    XAttribute type = table.Element(ns.GetName("Type")).Attribute("Name");
                    if (type.Value.ToLower().StartsWith(schema))
                    {
                        String value = type.Value.Substring(schema.Length) + "Entity";
                        attributes.Where(a => a.Value == type.Value).ForEach(a => a.SetValue(value));
                    }
                    ResolveCollision(table, ns);
                }

                root.Document.Save(file.Dbml.FullName);

                Console.WriteLine("\t{0}", file.Code.Name);
                Command.Run(Config.SqlMetal.FullName, 3, new[]
                {
                    "/code:{0}",
                    "/namespace:{1}.{2}",
                    "/context:{2}Database",
                    "/entitybase:{3}",
                    "{4}"
                }, file.Code.FullName.Quote(), Config.Db.Project, database, Config.Db.Base, file.Dbml.FullName.Quote());

                String source = File.ReadAllText(file.Code.FullName);
                //source = source.Insert(source.IndexOf('{') + 1, String.Format("\n\tusing {0};", Config.Db.Project));
                source = Regex.Replace(source, @"class ([@\w]+) : System.Data.Linq.DataContext", "class $1 : DbDatabase<$1>");
                source = Regex.Replace(source, String.Format(@"class ([@\w]+) : {0}", Config.Db.Base), "$0<$1>");
                //source = Regex.Replace(source, @"GetTable<([@\w]+)>\(\)", "$0.SetTable<$1>()");
                File.WriteAllText(file.Code.FullName, source);
            }

            Repo.Inject(bin.Code, Config.Db.Folder, Config.Db.Project, false, true);
        }

        private static void ResolveCollision(XElement table, XNamespace ns)
        {
            var type = table.Element(ns.GetName("Type"));
            switch(table.Attribute("Name").Value)
            {
                case("ssis.ArrearsExport"):
                    table.Attribute("Member").Value = type.Attribute("Name").Value = "ArrearsExportSsis";
                    break;
            }
        }

        private static String Trim(String value, String schema)
        {
            String[] split = value.Split('_');
            return split.First().ToLower() == schema ? String.Join(null, split.Skip(1)) : value;
        }
    }
}

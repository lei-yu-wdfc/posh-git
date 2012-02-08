using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Db
{
    public class DbGenerator
    {
        public static void Main(String[] args)
        {
            if (!Config.SqlMetal.Exists)
                throw new FileNotFoundException(Config.SqlMetal.FullName);

            var bin = new
            {
                Dbml = Repo.Directory("Dbml"),
                Code = Repo.Directory("Code")
            };

            foreach (String database in Config.Databases)
            {
                Console.WriteLine(database);

                var file = new
                {
                    Dbml = Repo.File(String.Format("{0}Database.dbml", database), bin.Dbml),
                    Code = Repo.File(String.Format("{0}Database.cs", database), bin.Code)
                };

                Console.WriteLine("\t{0}", file.Dbml.Name);
                Command.Run(Config.SqlMetal.FullName, 3, new[]
                {
                    "/database:{0}",
                    "/dbml:{1}",
                    "/pluralize"
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

            Repo.Inject(bin.Code, Config.Db.Folder, Config.Db.Project);
        }

        private static String Trim(String value, String schema)
        {
            String[] split = value.Split('_');
            return split.First().ToLower() == schema ? String.Join(null, split.Skip(1)) : value;
        }
    }
}

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

                Dictionary<String, String> values = new Dictionary<String, String>();
                XElement root = XDocument.Load(file.Dbml.FullName).Root;

                foreach (XElement table in root.Elements(root.GetDefaultNamespace().GetName("Table")))
                {
                    String schema = table.Attribute("Name").Value.Split('.').First().ToLower();
                    foreach (String value in new[] { table.Attribute("Member").Value, table.Element(root.GetDefaultNamespace().GetName("Type")).Attribute("Name").Value }.Distinct().Where(v => v.Contains('_') && !values.ContainsKey(v)))
                    {
                        String[] split = value.Split('_');
                        if (split.First().ToLower() == schema)
                            values.Add(value, String.Join(null, split.Skip(1)));
                    }
                }

                List<XAttribute> attributes = root.Descendants().SelectMany(d => d.Attributes()).ToList();
                attributes.Where(a => values.ContainsValue(a.Value)).ForEach(a => values.Add(a.Value, a.Value + a.Parent.Name.LocalName));
                attributes.Where(a => values.ContainsKey(a.Value)).ForEach(a => a.Value = values[a.Value]);
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
                source = source.Insert(source.IndexOf('{') + 1, String.Format("\n\tusing {0};", Config.Db.Project));
                source = Regex.Replace(source, String.Format(@"class ([@\w]+) : {0}", Config.Db.Base), "$0<$1>");
                source = Regex.Replace(source, @"GetTable<([@\w]+)>\(\)", "$0.SetTable<$1>()");
                File.WriteAllText(file.Code.FullName, source);
            }

            Repo.Inject(bin.Code, Config.Db.Folder, Config.Db.Project);
        }
    }
}

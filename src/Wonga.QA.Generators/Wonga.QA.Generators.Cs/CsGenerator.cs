using System;
using System.IO;
using System.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Cs
{
    public class CsGenerator
    {
		public static void Main(String[] args)
        {
			ProgramArgumentsParser.ParseArgumentsParameters(args);
		    Config.RepoName = "";
            var binRootDirectories = new GeneratorRepoDirectories(Config.CsApi.Folder);
            var classGenerator = new XmlSchemaClassGenerator(Config.CsApi, binRootDirectories, false);

            foreach (var repo in Config.Repos)
            {
                Config.RepoName = repo;
                ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

                foreach (FileInfo file in Origin.GetSchemas().Where(f => f.IsCs()))
                {
                    classGenerator.GenerateXmlSchemaClassesFiles(file, requests);
                }
            }

            Repo.Inject(binRootDirectories.ClassesDirectory, Config.CsApi.Folder, Config.CsApi.Project, delete: true, overwrite: true);
            Repo.Inject(binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.CsApi.Project, delete: true, overwrite: true);
        }
    }
}

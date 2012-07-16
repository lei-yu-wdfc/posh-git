using System;
using System.IO;
using System.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
	public class ApiGenerator
	{
		public static void Main(String[] args)
		{
			ProgramArgumentsParser.ParseArgumentsParameters(args);

            foreach (var repo in Config.Repos)
            {
                Config.RepoName = repo;
                var binRootDirectories = new GeneratorRepoDirectories(Config.Api.Folder);
                var classGenerator = new XmlSchemaClassGenerator(Config.Api, binRootDirectories, false);

                ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

                foreach (FileInfo file in Origin.GetSchemas().Where(f => !f.IsCs()))
                {
                    classGenerator.GenerateXmlSchemaClassesFiles(file, requests);
                }
            }           

            Repo.Inject(new GeneratorRepoDirectories(Config.Api.Folder).ClassesDirectory, Config.Api.Folder, Config.Api.Project, delete: true, overwrite: true);
            Repo.Inject(new GeneratorRepoDirectories(Config.Api.Folder).EnumsDirectory, Config.Enums.Folder, Config.Api.Project, delete: false, overwrite: true);
		}

		private static void GenerateForRepo(string repoName)
		{

		}
	}
}
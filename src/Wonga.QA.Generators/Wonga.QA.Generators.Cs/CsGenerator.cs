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

			GenerateForRepo("ops");
			GenerateForRepo("comms");
			GenerateForRepo("payments");
			GenerateForRepo("risk");
			GenerateForRepo("marketing");
        }

		private static void GenerateForRepo(String repo)
		{
			Config.RepoName = repo;
			var binRootDirectories = new GeneratorRepoDirectories(Config.CsApi.Folder);
			var classGenerator = new XmlSchemaClassGenerator(Config.CsApi, binRootDirectories);

			ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

			foreach (FileInfo file in Origin.GetSchemas().Where(f => f.IsCs()))
			{
				classGenerator.GenerateXmlSchemaClassesFiles(file, requests);
			}

			if (classGenerator.ErrorsOccurred)
			{
				Console.Error.WriteLine("*** THERE WERE ERRORS DURING GENERATION... NOT UPDATING QAF!!!!!");
				return;
			}

			Repo.Inject(binRootDirectories.EnumsDirectory, Config.CsApi.Folder, Config.Api.Project, delete: true, overwrite: true);
			Repo.Inject(binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.Enums.Project, delete: true, overwrite: true);
		}
    }
}

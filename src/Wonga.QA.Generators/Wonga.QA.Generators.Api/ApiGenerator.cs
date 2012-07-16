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

			GenerateForRepo("ops");
			GenerateForRepo("comms");
			GenerateForRepo("payments");
			GenerateForRepo("risk");
			GenerateForRepo("marketing");
		}

		private static void GenerateForRepo(string repoName)
		{
			Config.RepoName = repoName;
			var binRootDirectories = new GeneratorRepoDirectories(Config.Api.Folder);
			var classGenerator = new XmlSchemaClassGenerator(Config.Api, binRootDirectories);
			ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

			foreach (FileInfo file in Origin.GetSchemas().Where(f => !f.IsCs()))
			{
				classGenerator.GenerateXmlSchemaClassesFiles(file, requests);
			}

			if (classGenerator.ErrorsOccurred)
			{
				Console.Error.WriteLine("*** THERE WERE ERRORS DURING GENERATION... NOT UPDATING QAF!!!!!");
				return;
			}

			Repo.Inject(binRootDirectories.ClassesDirectory, Config.Api.Folder, Config.Api.Project, delete: true, overwrite:true);
			Repo.Inject(binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.Enums.Project, delete:true, overwrite:true);
		}
	}
}
using System;
using System.IO;
using System.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
    public class ApiGenerator
    {
		public const string RequestsDirectoryName = "Requests";
		public const string EnumsDirectoryName = "Enums";

        public static void Main(String[] args)
        {
            if (args.Any())
                Config.Origin = args.Single();

			var binRootDirectories = new GeneratorRepoDirectories(RequestsDirectoryName);

			var classGenerator = new XmlSchemaClassGenerator(Config.Api, binRootDirectories);

            ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());
			
            foreach (FileInfo file in Origin.GetSchemas().Where(f => !f.IsCs()))
            {
				classGenerator.GenerateXmlSchemaClassesFiles(file, requests);
            }

			if (classGenerator.ErrorsOccurred)
			{
				Console.Error.WriteLine("*** THERE WHERE ERRORS DURING GENERATION... NOT UPDATING QAF!!!!!");
				return;
			}

			Repo.Inject(binRootDirectories.ClassesDirectory, Config.Api.Folder, Config.Api.Project);
			Repo.Inject(binRootDirectories.EnumsDirectory, EnumsDirectoryName, Config.Api.Project);

        }
    }
}
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
            if (args.Any())
                Config.Origin = args.Single();

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

			Repo.Inject(binRootDirectories.ClassesDirectory, Config.Api.Folder, Config.Api.Project);
			Repo.Inject(binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.Api.Project);

        }
    }
}
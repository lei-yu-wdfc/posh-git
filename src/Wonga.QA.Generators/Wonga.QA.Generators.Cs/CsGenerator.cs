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
            if (args.Any())
                Config.Origin = args.Single();

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

			Repo.Inject(binRootDirectories.ClassesDirectory, Config.CsApi.Folder, Config.CsApi.Project);
			Repo.Inject(binRootDirectories.EnumsDirectory, Config.Enums.Folder, Config.CsApi.Project);
        }
    }
}

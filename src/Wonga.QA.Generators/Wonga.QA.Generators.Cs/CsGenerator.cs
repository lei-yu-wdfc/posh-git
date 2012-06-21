using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.CSharp;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Cs
{
    public class CsGenerator
    {
		public const string RequestsDirectoryName = "Requests";
		public const string EnumsDirectoryName = "Enums";

        public static void Main(String[] args)
        {
            if (args.Any())
                Config.Origin = args.Single();

			var binRootDirectories = new GeneratorRepoDirectories(RequestsDirectoryName);

			var classGenerator = new XmlSchemaClassGenerator(Config.CsApi, binRootDirectories);

            ILookup<String, Type> requests = Origin.GetTypes().Where(t => t.IsRequest()).ToLookup(t => t.GetName());

            foreach (FileInfo file in Origin.GetSchemas().Where(f => f.IsCs()))
            {
				classGenerator.GenerateXmlSchemaClassesFiles(file, requests);
            }

			if (classGenerator.ErrorsOccurred)
			{
				Console.Error.WriteLine("*** THERE WHERE ERRORS DURING GENERATION... NOT UPDATING QAF!!!!!");
				return;
			}

			Repo.Inject(binRootDirectories.ClassesDirectory, Config.CsApi.Folder, Config.CsApi.Project);
			Repo.Inject(binRootDirectories.EnumsDirectory, EnumsDirectoryName, Config.CsApi.Project);
        }
    }
}

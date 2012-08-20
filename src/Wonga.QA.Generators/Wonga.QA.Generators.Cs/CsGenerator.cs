using System.Collections.Generic;
using System.IO;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Cs
{
    public class CsGenerator
    {
        public void Generate()
        {
			var outputDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "CSAPI", "Requests"));

			Generate(Origin.GetCsApiCommandsSchema(), outputDirectory);
			Generate(Origin.GetCsApiQueriesSchema(), outputDirectory);
        	
        }

		private void Generate(FileInfo schemaFile, DirectoryInfo outputDirectory)
		{
			var schemaToTypesGenerator = new SchemaToTypesGenerator(schemaFile);
			var namespaceMessagePairs = schemaToTypesGenerator.GenerateNamespaceMessagesPairs();
			var types = schemaToTypesGenerator.GenerateTypes();
			var commandClasses = MessageClassBuilder.Build(MessageClassBuilder.MessageClassType.CsApi, namespaceMessagePairs, types, outputDirectory);
			HierarchicalClassFileWriter.WriteClassFilesToDisk(outputDirectory, commandClasses);
		}
    }
}

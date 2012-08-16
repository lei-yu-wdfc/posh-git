using System.Collections.Generic;
using System.IO;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
	public class ApiGenerator
	{
		public void Generate()
		{
			var outputDirectory = new DirectoryInfo("C:/");

			var commandsSchemaFile = Origin.GetApiCommandsSchema();
			var commandClasses = SchemaToClassGenerator.Generate(commandsSchemaFile, outputDirectory);
			HierarchicalClassFileWriter.WriteClassFilesToDisk(outputDirectory, commandClasses as List<MessageClassDefinition>);

			var queriesSchemaFile = Origin.GetApiQueriesSchema();
			var queryClasses = SchemaToClassGenerator.Generate(queriesSchemaFile, outputDirectory);
			HierarchicalClassFileWriter.WriteClassFilesToDisk(outputDirectory, queryClasses as List<MessageClassDefinition>);
		}
	}
}
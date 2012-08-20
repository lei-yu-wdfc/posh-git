using System.IO;

namespace Wonga.QA.Generators.Core
{
	public abstract class ApiGenerator
	{
		public abstract void Generate();

		protected void Generate(FileInfo schemaFile, MessageClassBuilder.MessageClassType messageClassType, DirectoryInfo outputDirectory)
		{
			var schemaToTypesGenerator = new SchemaToTypesGenerator(schemaFile);
			var namespaceMessagePairs = schemaToTypesGenerator.GenerateNamespaceMessagesPairs();
			var types = schemaToTypesGenerator.GenerateTypes();
			var commandClasses = MessageClassBuilder.Build(messageClassType, namespaceMessagePairs, types, outputDirectory);
			HierarchicalClassFileWriter.WriteClassFilesToDisk(outputDirectory, commandClasses);
		}
	}
}

using System.Collections.Generic;
using System.IO;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
	public class ApiGenerator
	{
		public void Generate()
		{
			var schemaFile = Origin.GetApiSchema();
			var classes = SchemaToClassGenerator.Generate(schemaFile);
			HierarchicalClassFileWriter.WriteClassFilesToDisk(new DirectoryInfo("C:/Api"), classes as List<MessageClassDefinition>);
		}
	}
}
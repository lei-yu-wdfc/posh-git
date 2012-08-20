using System.Collections.Generic;
using System.IO;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
	public class ApiGenerator : Core.ApiGenerator
	{
		public override void Generate()
		{
			var outputDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "API", "Requests"));

			Generate(Origin.GetApiCommandsSchema(), MessageClassBuilder.MessageClassType.Api,  outputDirectory);
			Generate(Origin.GetApiQueriesSchema(), MessageClassBuilder.MessageClassType.Api, outputDirectory);
		}
	}
	
}
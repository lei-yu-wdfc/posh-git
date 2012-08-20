using System.IO;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Cs
{
    public class CsApiGenerator : ApiGenerator
    {
    	public override void Generate()
        {
			var outputDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "CSAPI", "Requests"));

			Generate(Origin.GetCsApiCommandsSchema(), MessageClassBuilder.MessageClassType.CsApi, outputDirectory);
			Generate(Origin.GetCsApiQueriesSchema(), MessageClassBuilder.MessageClassType.CsApi, outputDirectory);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Api
{
	public class Program
	{
		public static void Main(String[] args)
		{
            ProgramArgumentsParser.ParseArgumentsParameters(args);
            new ApiGenerator().Generate();
            ProgramTools.ExitPrompt();
		}
	}
}
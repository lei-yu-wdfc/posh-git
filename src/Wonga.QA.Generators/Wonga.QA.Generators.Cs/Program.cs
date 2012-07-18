using System;
using System.IO;
using System.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Cs
{
    public class Program
    {
		public static void Main(String[] args)
        {
			ProgramArgumentsParser.ParseArgumentsParameters(args);
            new CsGenerator().Generate();
            ProgramTools.ExitPrompt();
        }
    }
}

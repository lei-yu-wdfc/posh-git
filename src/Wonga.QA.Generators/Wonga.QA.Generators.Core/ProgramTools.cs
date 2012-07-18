using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Generators.Core
{
	public static class ProgramTools
	{
		public static void ExitPrompt()
		{
		    Console.WriteLine("Press any key to exit the generator..");
		    Console.ReadKey();
		}
	}
}

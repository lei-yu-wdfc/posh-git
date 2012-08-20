using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Msmq
{
    /// <summary>
    /// Let me explain to you how the generator works before you start pulling your hair and jump through a window..
    /// * The generator will process everything under v3split/package
    /// * For each repo it will try to find the csproj files under that repo, ie v3split\Payments
    /// * For each csproj found it will try to load it's corresponding dll from the v3split\package folder, if for whatever reason this
    ///   fails it will go to the next one without crashing.
    /// * For each assembly loaded it will generate types for the messages and enums and store them under Bin\.Msmq and Bin\.Enums folder.
    /// * Once everything has been processed and given no error occured, it will first of all delete the Messages and Enums folder of the
    ///   Framework.Msmq project.
    /// * It will the temporary directories to the corresponding Messages and Enums folder we deleted in the previous step.
    /// * It will register any files found underneath those folders in the Framework.Msmq.csproj
    /// </summary>
    public class Program
    {

    	public static void Main(String[] args)
    	{
			ProgramArgumentsParser.ParseArgumentsParameters(args);
    	    new MsmqGenerator().Generate();
            ProgramTools.ExitPrompt();
    	}
    }
}

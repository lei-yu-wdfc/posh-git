using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Wonga.QA.Generators.Core;

namespace Wonga.QA.Generators.Db.Split
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var server = args.Length >= 1 ? args[0] : "localhost";
            Config.Databases = args.Length >= 2 ? new[] {args[1]} : Config.Databases;
            new DbGenerator().Generate(server, Config.Databases);
            ProgramTools.ExitPrompt();
        }
    }
}

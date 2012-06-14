using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tools.NonParallelHunter
{
    internal static class Program
    {

        private static void Main(string[] args)
        {
            var input = new ConsoleKeyInfo();
            while (input.KeyChar != '0')
            {
                Console.Clear();
                Console.WriteLine("Enter 1 for v3qa, 2 for v3");
                Console.WriteLine("You must have already built and v3qa and v3 folders to be siblings");
                input = Console.ReadKey();
                switch (input.KeyChar)
                {
                    case('1'):
                        v3QATests.GenerateReports();
                        break;
                    case('2'):
                        v3Tests.GenerateReports();
                        break;
                }
                
            }
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MbUnit.Framework;

namespace Wonga.QA.Tools.NonParallelHunter
{
    class Program
    {
        static void Main(string[] args)
        {
            string binFolder = Path.Combine(Environment.CurrentDirectory);
            var files = Directory.GetFiles(binFolder, "Wonga.QA.Tests.*.dll");
            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
                var assembly = Assembly.LoadFile(file);
                var testFixtures = assembly.GetTypes().Where(
                    t => !t.IsDefined(typeof (ParallelizableAttribute), true)
                    && t.IsDefined(typeof(TestFixtureAttribute), true)
                    && t.IsClass
                    && !t.IsAbstract);
                foreach(var testFixture in testFixtures)
                    Console.WriteLine(string.Format("\t {0}", testFixture.Name));
            }
            Console.Read();
        }
    }
}

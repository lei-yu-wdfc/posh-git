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
    public static class v3Tests
    {
        private static StringBuilder sb = null;
        private static int totalTestFixtures = 0;
        private static int totalTests = 0;
        private static string binFolder = Path.Combine(Environment.CurrentDirectory, "..\\..\\v3\\build\\tests");
        private static IEnumerable<string> files;
        private static bool includePendingOrIgnoredOrExplicit = false;

        public static void GenerateReports()
        {
            CopyEverythingFromTests();
            files = Directory.GetFiles(Environment.CurrentDirectory, "*.Tests.dll", SearchOption.AllDirectories).Distinct().ToList().Where(x => !x.Contains("Integration") && !x.Contains("DataAccess") && !x.Contains("Data.Access"));

            DocumentNonParallelation();
            Console.ReadKey();
        }

        private static void CopyEverythingFromTests()
        {
            var allfiles = Directory.GetFiles(binFolder, "*.*", SearchOption.AllDirectories);
            foreach(var file in allfiles)
            {
                var dest = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(file));
                try
                {
                    File.Copy(file, dest, true);
                    Console.WriteLine("Copying.."+Path.GetFileName(file));
                }
                catch (Exception)
                {

                }
            }

        }

        private static void DocumentNonParallelation()
        {
            //Generating Level 4 tests.
            sb = new StringBuilder();
            totalTestFixtures = 0;
            totalTests = 0;            

            sb.AppendLine("-- Level 4 Report --");
            sb.AppendLine("-- Listing all Tests and TestFixtures that are not parallelized --");
            foreach (var file in files)
            {
                sb.AppendLine(Path.GetFileName(file));
                var assembly = Assembly.LoadFile(file);
                var testFixtures = assembly.GetTestFixtures(includePendingOrIgnoredOrExplicit, null)
                    .GetNonParallelFixtures();
                ListTestFixtures(testFixtures, true);
            }
            sb.AppendLine();
            sb.AppendLine("Grand Total TestFixtures: " + totalTestFixtures);
            sb.AppendLine("Grand Total Tests: " + totalTests);
            Console.Write(sb);
            File.WriteAllText(string.Format("_Level_4_Tests.txt"), sb.ToString());

            //Generating Level 3 tests.
            sb = new StringBuilder();
            totalTestFixtures = 0;
            totalTests = 0;
            sb.AppendLine("-- Level 3 Report --");
            sb.AppendLine("-- Listing all TestFixtures that are not parallelized --");
            foreach (var file in files)
            {
                sb.AppendLine(Path.GetFileName(file));
                var assembly = Assembly.LoadFile(file);
                var testFixtures = assembly.GetTestFixtures(includePendingOrIgnoredOrExplicit, null)
                    .GetNonParallelFixtures().Concat(
                        assembly.GetTestFixtures(includePendingOrIgnoredOrExplicit, null).GetParallelFixtures(TestScope.Descendants));
                ListTestFixtures(testFixtures, false);
            }
            sb.AppendLine();
            sb.AppendLine("Grand Total TestFixtures: " + totalTestFixtures);
            sb.AppendLine("Grand Total Tests: " + totalTests);
            Console.Write(sb);
            File.WriteAllText(string.Format("_Level_3_Tests.txt"), sb.ToString());


            //Generating Level 2 tests.
            sb = new StringBuilder();
            totalTestFixtures = 0;
            totalTests = 0;
            sb.AppendLine("-- Level 2 Report --");
            sb.AppendLine("-- Listing all parallelized TestFixtures and their tests that aren't parallelized --");
            foreach (var file in files)
            {
                sb.AppendLine(Path.GetFileName(file));
                var assembly = Assembly.LoadFile(file);
                var testFixtures = assembly.GetTestFixtures(includePendingOrIgnoredOrExplicit, null)
                    .GetParallelFixtures(TestScope.Self);
                ListTestFixtures(testFixtures, true);
            }
            sb.AppendLine();
            sb.AppendLine("Grand Total TestFixtures: " + totalTestFixtures);
            sb.AppendLine("Grand Total Tests: " + totalTests);

            Console.Write(sb);
            File.WriteAllText(string.Format("_Level_2_Tests.txt"), sb.ToString());
        }

        private static void ListTestFixtures(IEnumerable<Type> testFixtures, bool listNotParallelMethods)
        {
            int testsNumber = 0;
            int fixturesNumber = 0;

            foreach (var testFixture in testFixtures)
            {
                var methods = testFixture.GetNonParallelTests(includePendingOrIgnoredOrExplicit, null);
                if (methods.Count() > 0)
                {
                    sb.AppendLine(string.Format("\t {0}", testFixture.Name));
                    totalTestFixtures++;
                    fixturesNumber++;
                }
                if (listNotParallelMethods)
                {
                    foreach (var method in methods)
                    {
                        totalTests++;
                        sb.AppendLine(string.Format("\t\t {0}", method.Name));
                        testsNumber++;
                    }
                }
            }
            if (fixturesNumber > 0)
                sb.AppendLine("Total TestFixtures: " + testFixtures.Count());
            if (testsNumber > 0)
                sb.AppendLine("Total Tests: " + testsNumber);
        }
    }
}

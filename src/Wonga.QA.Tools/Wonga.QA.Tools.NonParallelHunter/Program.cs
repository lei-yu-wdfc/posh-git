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
        private static StringBuilder sb = null;
        private static int totalTestFixtures = 0;
        private static int totalTests = 0;
        private static AUT? specificAut = null;
        private static string binFolder = Path.Combine(Environment.CurrentDirectory);
        private static IEnumerable<string> files;
        private static bool includePendingOrIgnoredOrExplicit = false;

        private static void Main(string[] args)
        {
            files = Directory.GetFiles(binFolder, "Wonga.QA.Tests.*.dll").Where(x => !x.Contains(".Ui"));

            foreach (var aut in Enum.GetValues(typeof(AUT)))
                DocumentNonParallelation((AUT?)aut);
            DocumentNonParallelation(null);
            Console.ReadKey();
        }

        public static void DocumentNonParallelation(AUT? aut)
        {
            //Generating Level 4 tests.
            sb = new StringBuilder();
            totalTestFixtures = 0;
            totalTests = 0;
            specificAut = aut;

            sb.AppendLine("-- Level 4 Report --");
            sb.AppendLine("-- Listing all Tests and TestFixtures that are not parallelized --");
            foreach (var file in files)
            {
                sb.AppendLine(Path.GetFileName(file));
                var assembly = Assembly.LoadFile(file);
                var testFixtures = assembly.GetTestFixtures()
                    .GetNonParallelFixtures();
                ListTestFixtures(testFixtures, true);
            }
            sb.AppendLine();
            sb.AppendLine("Grand Total TestFixtures: " + totalTestFixtures);
            sb.AppendLine("Grand Total Tests: " + totalTests);
            Console.Write(sb);
            File.WriteAllText(string.Format("_Level_4_Tests_{0}.txt", aut.ToString()), sb.ToString());

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
                var testFixtures = assembly.GetTestFixtures()
                    .GetNonParallelFixtures().Concat(
                        assembly.GetTestFixtures().GetParallelFixtures(TestScope.Descendants));
                ListTestFixtures(testFixtures, false);
            }
            sb.AppendLine();
            sb.AppendLine("Grand Total TestFixtures: " + totalTestFixtures);
            sb.AppendLine("Grand Total Tests: " + totalTests);
            Console.Write(sb);
            File.WriteAllText(string.Format("_Level_3_Tests_{0}.txt", aut.ToString()), sb.ToString());


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
                var testFixtures = assembly.GetTestFixtures()
                    .GetParallelFixtures(TestScope.Self);
                ListTestFixtures(testFixtures, true);
            }
            sb.AppendLine();
            sb.AppendLine("Grand Total TestFixtures: " + totalTestFixtures);
            sb.AppendLine("Grand Total Tests: " + totalTests);

            Console.Write(sb);
            File.WriteAllText(string.Format("_Level_2_Tests_{0}.txt", aut.ToString()), sb.ToString());
        }

        private static void ListTestFixtures(IEnumerable<Type> testFixtures, bool listNotParallelMethods)
        {
            int testsNumber = 0;
            int fixturesNumber = 0;

            foreach (var testFixture in testFixtures)
            {
                var methods = testFixture.GetNonParallelTests();
                if (methods.Count() > 0)
                {
                    sb.AppendLine(string.Format("\t {0}", testFixture.Name));
                    totalTestFixtures++;
                    fixturesNumber++;
                }
                if(listNotParallelMethods)
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

        public static IEnumerable<MemberInfo> GetNonParallelTests(this Type type)
        {
            return type.GetMethods().Where(m => !m.IsDefined(typeof(ParallelizableAttribute), false)
                                             && m.IsDefined(typeof(TestAttribute), false)
                                             && m.IsAppplicationSpecific()
                                             && !m.IsPending());
        }

        public static bool IsTestFixture(this Type t)
        {
            return t.IsDefined(typeof(TestFixtureAttribute), true)
                   || t.GetMethods().Any(x => x.IsDefined(typeof(TestAttribute), true));
        }

        public static IEnumerable<Type> GetTestFixtures(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsTestFixture()
                && t.IsAppplicationSpecific()
                && !t.IsAbstract
                && t.IsClass
                && !t.IsPending());
        }

        public static bool IsAppplicationSpecific(this MemberInfo type)
        {
            var autAttribute = type.GetCustomAttributes(typeof(AUTAttribute), false).FirstOrDefault() as AUTAttribute;
            if (!specificAut.HasValue || autAttribute == null)
                return true;
            return autAttribute.AUTs.Contains(specificAut.Value);
        }

        public static bool IsPending(this MemberInfo type)
        {
            if(includePendingOrIgnoredOrExplicit)
                return false;

            return type.IsDefined(typeof (PendingAttribute), false)
                                                     || type.IsDefined(typeof (ExplicitAttribute), false)
                                                     || type.IsDefined(typeof (IgnoreAttribute), false);
        }

        public static bool IsPending(this Type type)
        {
            if(includePendingOrIgnoredOrExplicit)
                return false;

            return type.IsDefined(typeof (PendingAttribute), false)
                                                     || type.IsDefined(typeof (ExplicitAttribute), false)
                                                     || type.IsDefined(typeof (IgnoreAttribute), false);
        }

        public static bool IsAppplicationSpecific(this Type type)
        {
            var autAttribute = type.GetCustomAttributes(typeof (AUTAttribute), false).FirstOrDefault() as AUTAttribute;
            if (!specificAut.HasValue || autAttribute == null)
                return true;
            return autAttribute.AUTs.Contains(specificAut.Value) && type.GetNonParallelTests().Count() > 0;
        }

        public static IEnumerable<Type> GetParallelFixtures(this IEnumerable<Type> types, TestScope testScope)
        {
            return types.Where(t =>
            {
                ParallelizableAttribute attr =
                    t.GetCustomAttributes(typeof(ParallelizableAttribute), false).FirstOrDefault() as
                    ParallelizableAttribute;
                return t.IsDefined(typeof(ParallelizableAttribute), false)
                       && attr != null && attr.Scope == testScope;
            });
        }

        public static IEnumerable<Type> GetNonParallelFixtures(this IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsDefined(typeof(ParallelizableAttribute), false));
        }
    }
}

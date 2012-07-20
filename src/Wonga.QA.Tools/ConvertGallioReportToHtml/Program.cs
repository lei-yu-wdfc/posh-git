using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
namespace ConvertGallioReportToHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = args[0];
            string destination = args[1];
            string format = args[2];
            string output = "";

            XDocument doc = XDocument.Load(source);
            List<TestResult> testResults = new List<TestResult>(ReadTestResults(doc));

            if (format.ToLower() == "html")
                output = new HtmlFormatTestReport().FormatReport(testResults);
            else if (format.ToLower() == "csv")
                output = new CsvFormatTestReport().FormatReport(testResults);

            File.WriteAllText(destination, output);
        }

        private static IEnumerable<TestResult> ReadTestResults(XDocument doc)
        {
            var tests = GetTests(doc);
            foreach(var test in tests)
            {
                var debugTraceNode = test.Descendants().FirstOrDefault(x => x.Name.LocalName == "text" && x.Ancestors().Any(anc => anc.Attribute("name") != null && anc.Attribute("name").Value == "DebugTrace"));
                var stackTraceNodes = test.Descendants().Where(x => x.Name.LocalName == "text" && x.Ancestors().Any(anc => anc.Attribute("name") != null && anc.Attribute("name").Value == "Failures"));
                var stackTraceText = "";
                foreach (var stackTraceNode in stackTraceNodes)
                    stackTraceText += stackTraceNode.Value;
                yield return new TestResult()
                                 {
                                     DebugTrace = debugTraceNode != null ? debugTraceNode.Value : "",
                                     StackTrace = stackTraceText,
                                     Name = test.Descendants().First(x => x.Name.LocalName == "testStep").Attribute("name").Value,
                                     FullName = test.Descendants().First(x => x.Name.LocalName == "testStep").Attribute("fullName").Value,
                                     Outcome = GetOutcome(test)
                                 };
            }
        }

        public static IEnumerable<XElement> GetTests(XDocument doc)
        {
            return doc.Root.Descendants().Where(
                x =>
                x.Name.LocalName == "testStepRun" &&
                x.Descendants().Count(desc => desc.Name.LocalName == "outcome") == 1)
                .Where(
                    x =>
                    x.Descendants().Any(
                        desc =>
                        desc.Name.LocalName == "outcome" && desc.Attribute("status") != null));
        }

        public static bool HasOutcome(XElement testElement, TestOutcome testOutcome)
        {
            string outcomeString = testOutcome.ToString().ToLower();
            return GetOutcome(testElement) == testOutcome;
        }

        public static TestOutcome GetOutcome(XElement testElement)
        {
            var outcome = testElement.Descendants().First(
                    desc =>
                        desc.Name.LocalName == "outcome" && desc.Attribute("status") != null);
            return (TestOutcome)Enum.Parse(typeof(TestOutcome), outcome.Attribute("status").Value, true);
        }

        public static string GetCurrentFolder()
        {
            //get the full location of the assembly with DaoTests in it
            string fullPath = System.Reflection.Assembly.GetAssembly(typeof(Program)).Location;

            //get the folder that's in
            return Path.GetDirectoryName(fullPath);
        }
    }
}

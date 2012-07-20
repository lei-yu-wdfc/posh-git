using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.ReportConverter
{
    class HtmlFormatTestReport : IFormatTestReport
    {
        private string resourcesFolder = "";
        public string FormatReport(List<TestResult> testResults)
        {
            resourcesFolder = Path.Combine(Program.GetCurrentFolder(), "HtmlFormatResources");
            string testHtml = File.ReadAllText(Path.Combine(resourcesFolder, "test.html"));
            string failedTestOutput = "";
            string passedTestOutput = "";
            var passedTests = testResults.Where(x => x.Outcome == TestOutcome.Passed);
            var failedTests = testResults.Where(x => x.Outcome == TestOutcome.Failed);
            var inconclusiveTests = testResults.Where(x => x.Outcome == TestOutcome.Inconclusive);

            foreach (var failedTest in failedTests)
                failedTestOutput += ApplyTemplate(GetTemplate(failedTest.Outcome), failedTest);

            foreach (var passedTest in passedTests)
                passedTestOutput += ApplyTemplate(GetTemplate(passedTest.Outcome), passedTest);

            return testHtml.Replace("%%FAILEDTESTS%%", failedTestOutput)
                .Replace("%%PASSEDTESTS%%", passedTestOutput)
                .Replace("%%TOTALALL%%", testResults.Count.ToString())
                .Replace("%%TOTALPASSED%%", passedTests.Count().ToString())
                .Replace("%%TOTALFAILED%%", failedTests.Count().ToString())
                .Replace("%%TOTALINCONCLUSIVE%%", inconclusiveTests.Count().ToString());
        }

        private static string ApplyTemplate(string testTemplate, TestResult testNode)
        {
            var stackTraceText = HttpUtility.HtmlEncode(testNode.StackTrace);
            var debugTraceText = HttpUtility.HtmlEncode(testNode.DebugTrace);
            return testTemplate.Replace("%%TESTNAME%%", testNode.FullName)
                .Replace("%%DEBUGTRACE%%", debugTraceText)
                .Replace("%%STACKTRACE%%", stackTraceText);
        }

        public string GetTemplate(TestOutcome testOutcome)
        {
            return File.ReadAllText(Path.Combine(resourcesFolder, string.Format("{0}.html", testOutcome.ToString().ToLower())));
        }
    }
}

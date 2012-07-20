using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ConvertGallioReportToHtml
{
    class HtmlFormatTestReport : IFormatTestReport
    {
        public string FormatReport(List<TestResult> testResults)
        {
            string resourcesFolder = Path.Combine(Program.GetCurrentFolder(), "HtmlFormatResources");
            string failedTestTemplate = File.ReadAllText(Path.Combine(resourcesFolder, "failed-test-template.html"));
            string passedTestTemplate = File.ReadAllText(Path.Combine(resourcesFolder, "passed-test-template.html"));
            string testHtml = File.ReadAllText(Path.Combine(resourcesFolder, "test.html"));
            string failedTestOutput = "";
            string passedTestOutput = "";

            foreach (var failedTest in testResults.Where(x => x.Outcome == TestOutcome.Failed))
                failedTestOutput += ApplyTemplate(failedTestTemplate, failedTest);

            foreach (var passedTest in testResults.Where(x => x.Outcome == TestOutcome.Passed))
                passedTestOutput += ApplyTemplate(passedTestTemplate, passedTest);

            return testHtml.Replace("%%FAILEDTESTS%%", failedTestOutput).Replace("%%PASSEDTESTS%%", passedTestOutput);
        }

        private static string ApplyTemplate(string testTemplate, TestResult testNode)
        {
            var stackTraceText = HttpUtility.HtmlEncode(testNode.StackTrace);
            var debugTraceText = HttpUtility.HtmlEncode(testNode.DebugTrace);
            return testTemplate.Replace("%%TESTNAME%%", testNode.FullName)
                .Replace("%%DEBUGTRACE%%", debugTraceText)
                .Replace("%%STACKTRACE%%", stackTraceText);
        }
    }
}

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
        public string FormatReport(TestReport testReport)
        {
            resourcesFolder = Path.Combine(Program.GetCurrentFolder(), "HtmlFormatResources");
            string testHtml = File.ReadAllText(Path.Combine(resourcesFolder, "test.html"));
            string failedTestOutput = "";
            string passedTestOutput = "";
            string ignoredTestOutput = "";
            string pendingTestOutput = "";

            var passedTests = testReport.Results.SelectMany(x => x.Children).Where(x => x.Outcome == TestOutcome.Passed);
            var failedTests = testReport.Results.SelectMany(x => x.Children).Where(x => x.Outcome == TestOutcome.Failed);
            var pendingTests = testReport.Results.SelectMany(x => x.Children).Where(x => x.Outcome == TestOutcome.Pending);
            var ignoredTests = testReport.Results.SelectMany(x => x.Children).Where(x => x.Outcome == TestOutcome.Ignored);

            foreach (var failedTest in failedTests)
                failedTestOutput += ApplyTemplate(GetTemplate(failedTest.Outcome), failedTest);

            foreach (var passedTest in passedTests)
                passedTestOutput += ApplyTemplate(GetTemplate(passedTest.Outcome), passedTest);

            foreach (var ignoredTest in ignoredTests)
                ignoredTestOutput += ApplyTemplate(GetTemplate(ignoredTest.Outcome), ignoredTest);

            foreach (var pendingTest in pendingTests)
                pendingTestOutput += ApplyTemplate(GetTemplate(pendingTest.Outcome), pendingTest);

            ApplyFunFacts(testReport, ref testHtml);
            return 
                testHtml.Replace("%%FAILEDTESTS%%", failedTestOutput)
                .Replace("%%PASSEDTESTS%%", passedTestOutput)
                .Replace("%%PENDINGTESTS%%", pendingTestOutput)
                .Replace("%%IGNOREDTESTS%%", ignoredTestOutput)
                
                .Replace("%%TOTALALL%%", testReport.Statistics.TestCount.ToString())
                .Replace("%%TOTALRUN%%", testReport.Statistics.RunCount.ToString())
                .Replace("%%TOTALPASSED%%", testReport.Statistics.PassedCount.ToString())
                .Replace("%%TOTALFAILED%%", testReport.Statistics.FailedCount.ToString())
                .Replace("%%TOTALIGNORED%%", testReport.Statistics.IgnoredCount.ToString())
                .Replace("%%TOTALPENDING%%", testReport.Statistics.PendingCount.ToString())
                .Replace("%%TOTALINCONCLUSIVE%%", testReport.Statistics.InconclusiveCount.ToString())
                .Replace("%%TOTALDURATION%%", TimeSpan.FromSeconds(testReport.Statistics.Duration).ToString() );
        }

        private void ApplyFunFacts(TestReport testReport, ref string testHtml)
        {
            var longestRunningTest =
                testReport.Results.FirstOrDefault(x => x.Duration == testReport.Results.Max(max => max.Duration));
            var longestRunningTestName = longestRunningTest != null ? longestRunningTest.FullName : "";
            var longestRunningTestDuration = longestRunningTest != null ? TimeSpan.FromSeconds(longestRunningTest.Duration).ToString() : "";
            testHtml = testHtml.Replace("%%LONGESTRUNNINGTESTNAME%%", longestRunningTestName)
                .Replace("%%LONGESTRUNNINGTESTDURATION%%", longestRunningTestDuration);
        }

        private string ApplyTemplate(string testTemplate, TestResult testNode)
        {
            var stackTraceText = HttpUtility.HtmlEncode(testNode.StackTrace);
            var debugTraceText = HttpUtility.HtmlEncode(testNode.DebugTrace);
            var result = testTemplate.Replace("%%TESTNAME%%", testNode.FullName)
                .Replace("%%DEBUGTRACE%%", debugTraceText)
                .Replace("%%STACKTRACE%%", stackTraceText);
            result = ApplyInfo(result, testNode);
            return result;
        }

        private string ApplyInfo(string testTemplate, TestResult test)
        {
            string metadata = "<ul>";
            metadata += string.Format("<li><b>Duration</b>: {0}</li>", TimeSpan.FromSeconds(test.Duration).ToString());
            foreach (var entry in test.Metadata.Where(x => !new[] { "TestKind" }.Contains(x.Key)))
            {
                switch(entry.Key)
                {
                    case("JIRA"):
                        metadata += string.Format("<li><b>{0}</b>: <a href='{1}' target='_blank'>{1}</a></li>", entry.Key, entry.Value);
                        break;
                    default:
                        metadata += string.Format("<li><b>{0}</b>: {1}</li>", entry.Key, entry.Value);
                        break;
                }
            }
            metadata += "</ul>";
            return testTemplate.Replace("%%INFO%%", metadata);
        }

        public string GetTemplate(TestOutcome testOutcome)
        {
            return File.ReadAllText(Path.Combine(resourcesFolder, string.Format("{0}.html", testOutcome.ToString().ToLower())));
        }
    }
}

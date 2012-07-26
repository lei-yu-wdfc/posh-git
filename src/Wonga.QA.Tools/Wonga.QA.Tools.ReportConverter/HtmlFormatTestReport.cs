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

            var tests = testReport.GetTestsWithoutTestFixtures();
            var passedTests = tests.Where(x => x.Outcome == TestOutcome.Passed);
            var failedTests = tests.Where(x => x.Outcome == TestOutcome.Failed);
            var pendingTests = tests.Where(x => x.Outcome == TestOutcome.Pending);
            var ignoredTests = tests.Where(x => x.Outcome == TestOutcome.Ignored);

            foreach (var failedTest in failedTests)
                failedTestOutput += ApplyTemplate(GetTemplate(failedTest.Outcome), failedTest);

            foreach (var passedTest in passedTests)
                passedTestOutput += ApplyTemplate(GetTemplate(passedTest.Outcome), passedTest);

            foreach (var ignoredTest in ignoredTests)
                ignoredTestOutput += ApplyTemplate(GetTemplate(ignoredTest.Outcome), ignoredTest);

            foreach (var pendingTest in pendingTests)
                pendingTestOutput += ApplyTemplate(GetTemplate(pendingTest.Outcome), pendingTest);

            ApplyFunFacts(tests, ref testHtml);
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

        private void ApplyFunFacts(List<TestResult> tests, ref string testHtml)
        {
            var longestRunningText = "<dt>Longest Running Tests</dt>";
            var longestRunningTests =
                tests.OrderByDescending(desc => desc.Duration).Take(5);
            foreach(var longestRunningTest in longestRunningTests)
                longestRunningText += "<dd>{0} - {1}</dd>".FormatWith(longestRunningTest.FullName, TimeSpan.FromSeconds(longestRunningTest.Duration).ToString());
            testHtml = testHtml.Replace("%%LONGESTRUNNINGTESTS%%", longestRunningText);
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
            string mailSubject = HttpUtility.HtmlEncode(string.Format("Test {1} - {0}", test.FullName, test.Outcome));
            foreach (var entry in test.Metadata.Where(x => !new[] { "TestKind", "Aut", "TestsOn" }.Contains(x.Key) && x.Value.Count > 0))
            {
                switch (entry.Key)
                {
                    case ("JIRA"):
                        metadata += string.Format("<li><b>{0}</b>: {1}</li>", entry.Key, AppendTemplateWithMetadata("<a href='{0}' target='_blank'>{0}</a>", entry.Value));
                        break;
                    case ("OwnerEmail"):
                        metadata += string.Format("<li><b>{0}</b>: {1}</li>", entry.Key, AppendTemplateWithMetadata("<a href='mailto:{0}?subject=" + mailSubject + "' target='_blank'>{0}</a>", entry.Value));
                        break;
                    default:
                        metadata += string.Format("<li><b>{0}</b>: {1}</li>", entry.Key, AppendTemplateWithMetadata("{0}", entry.Value));
                        break;
                }
            }
            metadata += "</ul>";
            return testTemplate.Replace("%%INFO%%", metadata);
        }

        private string AppendTemplateWithMetadata(string format, List<string> values)
        {
            var result = "";
            for(int i = 0; i < values.Count; i++)
            {
                var comma = i == 0 ? "" : ", ";
                result += string.Format(comma + format, values[i]);
            }
            return result;
        }

        public string GetTemplate(TestOutcome testOutcome)
        {
            return File.ReadAllText(Path.Combine(resourcesFolder, string.Format("{0}.html", testOutcome.ToString().ToLower())));
        }
    }

    public static class Extensions
    {
        public static string FormatWith(this string val, params object[] args)
        {
            return string.Format(val, args);
        } 
    }
}

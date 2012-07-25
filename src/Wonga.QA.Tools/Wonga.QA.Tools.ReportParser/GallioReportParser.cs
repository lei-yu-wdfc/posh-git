using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.ReportParser
{
    public class GallioReportParser
    {
        private XDocument _doc;
        private XNamespace _ns;

        public GallioReportParser(XDocument doc)
        {
            _doc = doc;
            _ns = _doc.Root.Name.Namespace;
        }

        public TestReport GetTestReport()
        {
            var testReport = new TestReport();
            var testModel = _doc.Root.Element(_ns + "testModel");
            var testModelNodes = testModel.Descendants(_ns + "test").Where(
                    tests => tests.Element(_ns + "metadata").Descendants(_ns + "value").Any(meta => meta.Value == "Fixture")).
                    ToList();

            foreach (var test in testModelNodes)
            {
                testReport.Results.Add(GetTest(test));
            }

            testReport.Statistics = GetStatistics();

            return testReport;
        }

        private TestStatistics GetStatistics()
        {
            var statistics = new TestStatistics();
            var statNode = _doc.Root.Element(_ns + "testPackageRun").Element(_ns + "statistics");
            statistics.AssertCount = int.Parse(statNode.Attribute("assertCount").Value);
            statistics.Duration = float.Parse(statNode.Attribute("duration").Value);
            statistics.FailedCount = int.Parse(statNode.Attribute("failedCount").Value);
            statistics.IgnoredCount = GetCount("ignored", statNode);
            statistics.PendingCount = GetCount("pending", statNode);
            statistics.InconclusiveCount = int.Parse(statNode.Attribute("inconclusiveCount").Value);
            statistics.RunCount = int.Parse(statNode.Attribute("runCount").Value);
            statistics.StepCount = int.Parse(statNode.Attribute("stepCount").Value);
            statistics.PassedCount = int.Parse(statNode.Attribute("passedCount").Value);
            statistics.TestCount = int.Parse(statNode.Attribute("testCount").Value);
            return statistics;
        }

        private int GetCount(string status, XElement statNode)
        {
            var val = (from outcomeSummary in statNode.Descendants(_ns + "outcomeSummary")
                                       where
                                           outcomeSummary.Descendants(_ns + "outcome").Any(
                                               outcome =>
                                               outcome.Attribute("status") != null &&
                                               outcome.Attribute("status").Value == "skipped" &&
                                               outcome.Attribute("category") != null &&
                                               outcome.Attribute("category").Value == status)
                                       select outcomeSummary).FirstOrDefault();

            if (val == null || val.Attribute("count") == null || string.IsNullOrEmpty(val.Attribute("count").Value))
                return 0;
            return int.Parse(val.Attribute("count").Value);
        }

        private TestResult GetTest(XElement test)
        {
            var testResult = new TestResult();
            testResult.Metadata = GetMetadata(test);
            testResult.Id = test.Attribute("id").Value;
            testResult.FullName = test.Attribute("fullName").Value;
            testResult.Name = test.Attribute("name").Value;

            var testPackage =
                _doc.Root.Element(_ns + "testPackageRun").Descendants(_ns + "testStepRun").FirstOrDefault(
                    x => x.Element(_ns + "testStep") != null && x.Element(_ns + "testStep").Attribute("testId").Value == testResult.Id);
            if (testPackage != null)
            {
                testResult.DebugTrace = GetDebugTrace(testPackage);
                testResult.StackTrace = GetStackTrace(testPackage);
                testResult.WarningsTrace = GetWarningsTrace(testPackage);
                testResult.Outcome = GetOutcome(testPackage);
                testResult.AssertCount = GetAssertCount(testPackage);
                testResult.Duration = GetDuration(testPackage);
            }

            var childrenTests = test.Elements(_ns + "children").Elements(_ns + "test");
            foreach(var childTest in childrenTests)
                testResult.Children.Add(GetTest(childTest));
            return testResult;
        }

        private string GetWarningsTrace(XElement testPackage)
        {
            var stackTraceNodes = testPackage.Descendants(_ns + "text").Where(x => x.Ancestors().Any(anc => anc.Attribute("name") != null && anc.Attribute("name").Value == "Warnings"));
            var stackTraceText = "";
            foreach (var stackTraceNode in stackTraceNodes)
                stackTraceText += stackTraceNode.Value;
            return stackTraceText;
        }

        private float GetDuration(XElement testPackage)
        {
            return float.Parse(testPackage.Element(_ns + "result").Attribute("duration").Value);
        }

        private int GetAssertCount(XElement testPackage)
        {
            return int.Parse(testPackage.Element(_ns + "result").Attribute("assertCount").Value);
        }

        private string GetStackTrace(XElement testStepRun)
        {
            var stackTraceNodes = testStepRun.Descendants(_ns + "text").Where(x => x.Ancestors().Any(anc => anc.Attribute("name") != null && anc.Attribute("name").Value == "Failures"));
            var stackTraceText = "";
            foreach (var stackTraceNode in stackTraceNodes)
                stackTraceText += stackTraceNode.Value;
            return stackTraceText;
        }

        private string GetDebugTrace(XElement testStepRun)
        {
            
            var debugStream = (from stream in testStepRun.Descendants(_ns + "stream")
                              where stream.Attribute("name").Value == "DebugTrace"
                              select stream).FirstOrDefault();

            if (debugStream == null)
                return "";
            return debugStream.Descendants(_ns + "text").First().Value;
        }

        private Dictionary<string, string> GetMetadata(XElement test)
        {
            var metadata = new Dictionary<string, string>();
            var entries = test.Element(_ns + "metadata").Descendants(_ns + "entry");
            foreach(var entry in entries)
                metadata.Add(entry.Attribute("key").Value, entry.Element(_ns+"value").Value);
            return metadata;
        }

        public bool HasOutcome(XElement testElement, TestOutcome testOutcome)
        {
            return GetOutcome(testElement) == testOutcome;
        }

        public TestOutcome GetOutcome(XElement testElement)
        {
            var outcome = testElement.Descendants().FirstOrDefault(
                    desc =>
                        desc.Name.LocalName == "outcome" && desc.Attribute("status") != null);
            if (outcome == null)
                return TestOutcome.UnknownOutcome;
            TestOutcome tryMe;
            var outcomeText = outcome.Attribute("category") != null ? outcome.Attribute("category").Value
                              : outcome.Attribute("status").Value;
            if (!Enum.TryParse(outcomeText, true, out tryMe))
            {
                Trace.WriteLine(string.Format("WARNING: I just found a test outcome that i don't have an enumeration for({0})", outcomeText));
                return TestOutcome.UnknownOutcome;
            }
            return (TestOutcome)Enum.Parse(typeof(TestOutcome), outcomeText, true);
        }
    }
}

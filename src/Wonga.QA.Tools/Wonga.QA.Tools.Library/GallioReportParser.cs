using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.Library
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

        private IEnumerable<TestResult> ReadTestResults()
        {
            var tests = GetTests(_doc);
            foreach (var test in tests)
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
                    Outcome = GetOutcome(test),
                    Metadata = GetMetadata(test)
                };
            }
        }

        public List<TestResult> Parse()
        {
            return new List<TestResult>(ReadTestResults());
        }

        private Dictionary<string, string> GetMetadata(XElement test)
        {
            var metadata = new Dictionary<string, string>();
            var testId = test.Descendants().First(x => x.Name.LocalName == "testStep").Attribute("testId").Value;
            var metadataNode =
                _doc.Descendants().First(
                    x =>
                    x.Name.LocalName == "metadata" &&
                    x.Ancestors().Any(anc => anc.Name.LocalName == "test" && anc.Attribute("id").Value == testId));
            var entries = metadataNode.Descendants().Where(x => x.Name.LocalName == "entry");
            foreach(var entry in entries)
                metadata.Add(entry.Attribute("key").Value, entry.Element(_ns+"value").Value);
            return metadata;
        }

        public IEnumerable<XElement> GetTests(XDocument doc)
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

        public bool HasOutcome(XElement testElement, TestOutcome testOutcome)
        {
            return GetOutcome(testElement) == testOutcome;
        }

        public TestOutcome GetOutcome(XElement testElement)
        {
            var outcome = testElement.Descendants().First(
                    desc =>
                        desc.Name.LocalName == "outcome" && desc.Attribute("status") != null);
            TestOutcome tryMe;
            var outcomeText = outcome.Attribute("status").Value;
            if (!Enum.TryParse(outcomeText, true, out tryMe))
            {
                Trace.WriteLine(string.Format("WARNING: I just found a test outcome that i don't have an enumeration for({0})", outcomeText));
                return TestOutcome.UnknownOutcome;
            }
            return (TestOutcome)Enum.Parse(typeof(TestOutcome), outcomeText, true);
        }
    }
}

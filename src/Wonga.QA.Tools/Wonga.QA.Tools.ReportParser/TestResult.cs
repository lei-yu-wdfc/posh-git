using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tools.ReportParser
{
    public enum TestOutcome
    {
        Passed,
        Failed,
        Ignored,
        Pending,
        Inconclusive,
        UnknownOutcome
    }

    public enum TestKind
    {
        Fixture,
        Test,
        Unknown
    }

    public class TestResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public TestOutcome Outcome { get; set; }
        public string DebugTrace { get; set; }
        public string StackTrace { get; set; }
        public string WarningsTrace { get; set; }
        public int AssertCount { get; set; }
        public double Duration { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public TestKind TestKind
        {
            get
            {
                var result = TestKind.Unknown;
                var testKindText = "";
                Metadata.TryGetValue("TestKind", out testKindText);
                Enum.TryParse(testKindText, true, out result);
                return result;
            }
        }

        public List<TestResult> Children { get; set; }

        public TestResult()
        {
            Metadata = new Dictionary<string, string>();
            Children = new List<TestResult>();
        }
    }
}

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
        Skipped,
        Inconclusive,
        UnknownOutcome
    }

    public class TestResult
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public TestOutcome Outcome { get; set; }
        public string DebugTrace { get; set; }
        public string StackTrace { get; set; }
    }
}

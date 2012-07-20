using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertGallioReportToHtml
{
    enum TestOutcome
    {
        Passed,
        Failed,
        Skipped
    }

    class TestResult
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public TestOutcome Outcome { get; set; }
        public string DebugTrace { get; set; }
        public string StackTrace { get; set; }
    }
}

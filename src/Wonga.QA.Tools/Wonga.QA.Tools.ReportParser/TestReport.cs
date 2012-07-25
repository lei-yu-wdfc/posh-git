using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tools.ReportParser
{
    public class TestReport
    {
        public List<TestResult> Results { get; set; }
        public TestStatistics Statistics { get; set; }
        public TestReport()
        {
            Results = new List<TestResult>();
            Statistics = new TestStatistics();
        }
    }
}

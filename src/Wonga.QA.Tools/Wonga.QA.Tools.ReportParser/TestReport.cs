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

        public List<TestResult> GetTests(List<TestResult> coll = null, bool includeTestFixtures = false)
        {
            List<TestResult> result = new List<TestResult>();
            if(coll == null)
                coll = Results;
            if(!includeTestFixtures)
                result.AddRange(coll.Where(x => x.TestKind == TestKind.Test));
            else result.AddRange(coll);
            var children = coll.SelectMany(x => x.Children).ToList();
            if(children.Count > 0)
                result.AddRange(GetTests(children, includeTestFixtures));
            return result.Distinct().ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.MigrationTests.Core
{
    public class AcceptanceTestResults
    {
        public int RunId { get; set; }
        public int BatchId { get; set; }
        public string TestName { get; set; }
        public MigratedUser MigratedUser { get; set; }
        public TestParameters TestParameters { get; set; }
        public DateTime TestStartDate { get; set; }
        public DateTime TestEndDate { get; set; }
        public byte TestResult { get; set; }

        public AcceptanceTestResults()
        {
            MigratedUser = new MigratedUser();
            TestParameters = new TestParameters();
        }
    }
}

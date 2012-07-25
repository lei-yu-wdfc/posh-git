using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tools.ReportParser
{
    
    public class TestStatistics
    {
        public int AssertCount { get; set; }
        /// <summary>
        /// In seconds
        /// </summary>
        public double Duration { get; set; }
        public int RunCount { get; set; }
        public int PassedCount { get; set; }
        public int FailedCount { get; set; }
        public int InconclusiveCount { get; set; }
        public int SkippedCount { get { return PendingCount + IgnoredCount; } }
        public int PendingCount { get; set; }
        public int IgnoredCount { get; set; }
        public int TestCount { get; set; }
        public int StepCount { get; set; }

    }
}

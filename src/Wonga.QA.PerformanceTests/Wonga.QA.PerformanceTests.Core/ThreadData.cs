using System;
using System.Threading;

namespace Wonga.QA.PerformanceTests.Core
{
    public class ThreadData
    {
        public Thread Thread;
        public String Name;
        public DateTime StartTime;
        public DateTime EndTime;
        public Status CurrentStatus;
        public String TestName;

        public ThreadData(Thread thread, String name, DateTime startTime, Status status, String testName)
        {
            Thread = thread;
            Name = name;
            StartTime = startTime;
            CurrentStatus = status;
            TestName = testName;
        }
    }
}

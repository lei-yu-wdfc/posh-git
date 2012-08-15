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
        public String Status;

        public ThreadData(Thread thread, String name, DateTime startTime, String status)
        {
            Thread = thread;
            Name = name;
            StartTime = startTime;
            Status = status;
        }
    }
}

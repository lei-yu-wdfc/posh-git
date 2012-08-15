using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wonga.QA.PerformanceTests.Load.Api;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.PerformanceTests.Core
{
    [TestClass]
    public class Helper
    {
        public int Users = 10;
        public int Iterations = 100;
        public int MaxWaitTime = 30;

        public List<ThreadData> ThreadPool = new List<ThreadData>();

        [TestMethod]
        public void ApiJourneyLoadTest()
        {
            var tests = 0;

            while (tests < Iterations)
            {
                for (var i = 0; i < Users; i++)
                {
                    var journey = new AllApiJourneys();
                    //Change here to run different API Tests
                    var thread = new Thread(journey.ApiL0JourneyAccepted) { IsBackground = true };
                    ThreadPool.Add(new ThreadData(thread, "Thread: " + i, DateTime.Now, "Started"));

                    thread.Start();
                }
                MonitorStatus();
                tests += Users;
            }

            PrintResults();
        }

        /// <summary>
        /// Print the results
        /// </summary>
        public void PrintResults()
        {
            Console.WriteLine("Thread Name :: Test Status :: Execution Time");
            foreach (var loadThread in ThreadPool)
            {
                Console.WriteLine(loadThread.Name + "::" + loadThread.Status + "::" + (loadThread.EndTime.Subtract(loadThread.StartTime).Seconds));
            }
        }
        /// <summary>
        /// Monitor the running threads
        /// </summary>
        private void MonitorStatus()
        {
            bool running;
            DateTime startTime = DateTime.Now;

            do
            {
                running = false;
                foreach (var loadThread in GetRunningThreads())
                {
                    if (loadThread.Thread.IsAlive)
                        running = true;
                    else
                    {
                        var endTime = DateTime.Now;
                        TimeSpan totalTimeTaken = endTime.Subtract(loadThread.StartTime);
                        Console.WriteLine("Execution Time: " + totalTimeTaken.Seconds);
                        ThreadPool[GetThreadIndex(loadThread.Thread)].EndTime = endTime;
                        ThreadPool[GetThreadIndex(loadThread.Thread)].Status = "Completed";
                    }
                }

                if (DateTime.Now.Subtract(startTime).Seconds <= MaxWaitTime) continue;
                Console.WriteLine("Killing " + GetRunningThreads().Count + " over running tests");
                KillAllRunningThreads();
                running = false;

            } while (running);
        }

        /// <summary>
        /// Kill all running threads
        /// </summary>
        private void KillAllRunningThreads()
        {
            foreach (var loadThread in GetRunningThreads())
            {
                try
                {
                    if (loadThread.Thread.IsAlive)
                    {
                        ThreadPool[GetThreadIndex(loadThread.Thread)].EndTime = DateTime.Now;
                        ThreadPool[GetThreadIndex(loadThread.Thread)].Status = "Aborted";

                        loadThread.Thread.Interrupt();
                        if (!loadThread.Thread.Join(2000))
                        {
                            loadThread.Thread.Abort();
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Thread Interrupted exception thrown.");
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Thread Abort exception thrown.");
                }
            }
        }

        /// <summary>
        /// retruns all running Threads
        /// </summary>
        /// <returns></returns>
        public List<ThreadData> GetRunningThreads()
        {
            var runningThreads = new List<ThreadData>();

            foreach (var loadThread in ThreadPool)
            {
                if (loadThread.Status.ToLower().Equals("started"))
                    runningThreads.Add(loadThread);
            }
            return runningThreads;
        }

        /// <summary>
        /// Returns thread's index in Thread pool
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public int GetThreadIndex(Thread thread)
        {
            var i = 0;
            foreach (var loadThread in ThreadPool)
            {
                if (loadThread.Thread.Equals(thread))
                    return i;
                i++;
            }
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.PerformanceTests.Core
{
    public class Helper
    {
        //Read Config values from App.config
        public int Users = int.Parse(ConfigurationManager.AppSettings["numberOfUsers"]);
        public int Iterations = int.Parse(ConfigurationManager.AppSettings["iterations"]);
        public int MaxWaitTime = int.Parse(ConfigurationManager.AppSettings["maxWaitTime"]);
        public String TestName = ConfigurationManager.AppSettings["tests"];
        public String DllName = ConfigurationManager.AppSettings["dllName"];
        public String TypeName = ConfigurationManager.AppSettings["namespace"];

        //All running, completed thread data saved here
        public List<ThreadData> ThreadPool = new List<ThreadData>();

        /// <summary>
        /// Setup the Test Target and Configuration Directory path
        /// </summary>
        public void Setup()
        {
            Config.Configure(testTarget: ConfigurationManager.AppSettings["aut"]);
            Config.Configure(configsDirectoryPath: ConfigurationManager.AppSettings["configDir"]);
        }

        /// <summary>
        /// Entry point from where the test will be trigerred
        /// </summary>
        public void StartTest()
        {
            Setup();

            var tests = 1;

            while (tests <= Iterations)
            {
                for (var user = 1; user <= Users; user++)
                {
                    var thread = new Thread(new TestRunner().Run) { IsBackground = true };
                    ThreadPool.Add(new ThreadData(thread, "User: " + user + " Iteration: " + tests++, DateTime.Now, Status.Running, TestName));

                    thread.Start();
                }
                MonitorStatus();
            }
            PrintResults();
            WriteResultsToCsv();
        }

        /// <summary>
        /// Print the results
        /// </summary>
        public void PrintResults()
        {
            Console.WriteLine("Thread Name :: Test Status :: Execution Time");
            foreach (var loadThread in ThreadPool)
            {
                Console.WriteLine(loadThread.Name + "::" + loadThread.CurrentStatus.ToString() + "::" + 
                    (loadThread.EndTime.Subtract(loadThread.StartTime).Seconds));
            }
        }

        /// <summary>
        /// Write the results in to a CSV file
        /// </summary>
        public void WriteResultsToCsv()
        {
            var csvFileName = string.Format(@"{0}.txt", Guid.NewGuid()) + ".csv";
            var data = GenerateResults();
            var sb = new StringBuilder();
            const string delimiter = ",";

            foreach (var stringse in data)
                sb.AppendLine(string.Join(delimiter, stringse));

            File.WriteAllText(ConfigurationManager.AppSettings["outputDir"] + "\\" + csvFileName , sb.ToString()); 
        }

        /// <summary>
        /// Generate Results to be able to write in to a CSV file
        /// </summary>
        /// <returns></returns>
        public List<string[]> GenerateResults()
        {
            var data = new List<string[]> { new string[4] { "Thread Name", "Test Name", "Test Status", "Execution Time (seconds)" } };

            foreach (var loadThread in ThreadPool)
            {
                data.Add(new string[4] { loadThread.Name, loadThread.TestName ,loadThread.CurrentStatus.ToString(), 
                    loadThread.EndTime.Subtract(loadThread.StartTime).Seconds.ToString()});
            }
            return data;
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
                        ThreadPool[GetThreadIndex(loadThread.Thread)].EndTime = endTime;
                        ThreadPool[GetThreadIndex(loadThread.Thread)].CurrentStatus = Status.Completed;
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
                        ThreadPool[GetThreadIndex(loadThread.Thread)].CurrentStatus = Status.Aborted;

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
                if (loadThread.CurrentStatus.Equals(Status.Running))
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

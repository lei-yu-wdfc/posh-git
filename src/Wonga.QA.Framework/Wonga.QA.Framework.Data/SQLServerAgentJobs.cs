using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Threading;

namespace Wonga.QA.Framework.Data
{
    /// <summary>
    /// This class will be used for working with SQL Server Agent jobs
    /// </summary>
    public static class SQLServerAgentJobs
    {

        /// <summary>
        /// Executes a job and returns if the job was successful or not.
        /// </summary>
        /// <param name="jobName">The name of the SQL Server agent job to execute</param>
        /// <returns>Returns if job has run</returns>
        public static bool Execute(string jobName)
        {
            // This DataDriver().NameOfHDSServer really needs to come in as a param
            Server srv = new Server(new DataDriver().Hds.Server);

            Job specificJob = new Job(srv.JobServer, jobName);
            specificJob.Refresh();

            DateTime lastRunDate = specificJob.LastRunDate;
            specificJob.Start();

            while (specificJob.LastRunDate == lastRunDate)
            {
                Thread.Sleep(1000);
                specificJob.Refresh();
            }

            return specificJob.LastRunOutcome.Equals(CompletionResult.Succeeded);
        }

        /// <summary>
        /// Used to return the date and time the job was last run
        /// </summary>
        /// <param name="jobName">The name of the job to check</param>
        /// <returns>The date and time of the previous execution</returns>
        public static DateTime? GetJobLastRunDateTime(string jobName)
        {

            try
            {
                Server srv = new Server(new DataDriver().Hds.Server);

                Job specificJob = new Job(srv.JobServer, jobName);
                specificJob.Refresh();

                // Returns 0001/01/01 00:00:00 if never run
                return specificJob.LastRunDate;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Used to check the current job status
        /// </summary>
        /// <param name="jobName">Name of the job to check the status of</param>
        /// <returns>The SMO/Agent Job Execution status</returns>
        public static JobExecutionStatus CheckJobStatus(string jobName)
        {

            Server srv = new Server(new DataDriver().Hds.Server);

            Job specificJob = new Job(srv.JobServer, jobName);
            specificJob.Refresh();

            return specificJob.CurrentRunStatus;

        }

        /// <summary>
        /// Used to check current job last job step status
        /// </summary>
        /// <param name="jobName">Name of the job to check the status of</param>
        /// <returns>The SMO/Agent Job Execution status</returns>
        public static CompletionResult LastRunOutcome(string jobName)
        {

            Server srv = new Server(new DataDriver().Hds.Server);

            Job specificJob = new Job(srv.JobServer, jobName);
            specificJob.Refresh();

            return specificJob.LastRunOutcome;

        }

        /// <summary>
        /// Disable a job
        /// </summary>
        /// <param name="jobName">Name of job to disable</param>
        public static void DisableJob(string jobName)
        {

            Server srv = new Server(new DataDriver().Hds.Server);

            Job specificJob = new Job(srv.JobServer, jobName);
            specificJob.Refresh();

            specificJob.IsEnabled = false;

            // After setting the property you need to alter the job
            specificJob.Alter();

        }

        /// <summary>
        /// Job to enable
        /// </summary>
        /// <param name="jobName">Name of job to enable</param>
        public static void EnableJob(string jobName)
        {
            Server srv = new Server(new DataDriver().Hds.Server);

            Job specificJob = new Job(srv.JobServer, jobName);
            specificJob.Refresh();

            specificJob.IsEnabled = true;

            // After setting the property you need to alter the job
            specificJob.Alter();

        }

        /// <summary>
        /// Check if the job is enabled. Returns "IsEnabled" setting
        /// </summary>
        /// <param name="jobName">Job name</param>
        /// <returns>The IsEnabled setting</returns>
        public static bool CheckIsJobEnabled(string jobName)
        {
            Server srv = new Server(new DataDriver().Hds.Server);

            Job specificJob = new Job(srv.JobServer, jobName);
            specificJob.Refresh();

            return specificJob.IsEnabled;
        }

        /// <summary>
        /// Wait until a new run of a job completes, but only after an existing run completes. Wait time specified either by the default 6 minutes or what is provided
        /// </summary>
        /// <param name="jobName">Name of job to check</param>
        /// <param name="waitTimeSecondsOverride">If you want a different value to 6 minutes</param>
        /// <param name="pollingIntervalMilliSeconds">How long to wait before checking job again</param>
        /// <returns>A true means the job has run, an exception is thrown if it is not</returns>
        public static bool WaitUntilJobComplete(string jobName, Int32 waitTimeSecondsOverride = 0, int pollingIntervalMilliSeconds = 1000)
        {
            bool loopUntilTrue = false;
            try
            {
                Int32 waitTimeSeconds = waitTimeSecondsOverride == 0 ? 60 * 6 * 1000 : waitTimeSecondsOverride; // Convert to 6 mins

                Server srv = new Server(new DataDriver().Hds.Server);
                
                Job specificJob = new Job(srv.JobServer, jobName);
                specificJob.Refresh();
                
                // wait for an existing job run to complete
                Int32 waitTimeSeconds2 = waitTimeSeconds;

                do
                {
                    Thread.Sleep(pollingIntervalMilliSeconds);

                    waitTimeSeconds2 -= pollingIntervalMilliSeconds;

                    if (waitTimeSeconds2 <= 0)
                    {
                        throw (new Exception("Existing Job run not finished in specified time"));
                    }

                    specificJob.Refresh();

                } while (specificJob.CurrentRunStatus != JobExecutionStatus.Idle);

                // Wait for a new run to complete
                DateTime? LastRunDate = specificJob.LastRunDate;

                do
                {
                    if (specificJob.LastRunDate > LastRunDate)
                    {
                        loopUntilTrue = true;
                    }
                    else
                    {
                        loopUntilTrue = false;
                    }

                    Thread.Sleep(pollingIntervalMilliSeconds);

                    waitTimeSeconds -= pollingIntervalMilliSeconds;

                    if (waitTimeSeconds <= 0)
                    {
                        throw (new Exception("Job not run in specified time"));
                    }

                    // Refresh the job specifics for the recheck
                    specificJob.Refresh();

                } while (loopUntilTrue == false);
            }
            catch (Exception e)
            {
                throw e;
            }
            return loopUntilTrue;
        }
    }
}

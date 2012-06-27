using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Threading;

namespace Wonga.QA.Framework.Data
{
    /// <summary>
    /// This class will be used for working with SQL Server Agent jobs
    /// </summary>
    public class SQLServerAgentJobs
    {

        /// <summary>
        /// Executes a job and returns if the job was successful or not.
        /// </summary>
        /// <param name="jobName">The name of the SQL Server agent job to execute</param>
        /// <returns>Returns if job has run</returns>
        public static bool Execute(string jobName)
        {
            // This DataDriver().NameOfHDSServer really needs to come in as a param
            Server srv = new Server(new DataDriver().NameOfHdsServer);
            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

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
        /// <param name="serverName">This comes from Drive.Data passed in</param>
        /// <param name="jobName">The name of the job to check</param>
        /// <returns>The date and time of the previous execution</returns>
        public static DateTime? GetJobLastRunDateTime(string jobName)
        {

            try
            {
                Server srv = new Server(new DataDriver().NameOfHdsServer);

                JobServer sqlServerAgent = srv.JobServer;
                Job specificJob = sqlServerAgent.Jobs[jobName];

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
        /// <param name="serverName">Name of the server to connect to</param>
        /// <param name="jobName">Name of the job to check the status of</param>
        /// <returns>The SMO/Agent Job Execution status</returns>
        public static Microsoft.SqlServer.Management.Smo.Agent.JobExecutionStatus CheckJobStatus(string jobName)
        {

            Server srv = new Server(new DataDriver().NameOfHdsServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            return specificJob.CurrentRunStatus;

        }

        /// <summary>
        /// Used to check current job last job step status
        /// </summary>
        /// <param name="serverName">Name of the server to connect to</param>
        /// <param name="jobName">Name of the job to check the status of</param>
        /// <returns>The SMO/Agent Job Execution status</returns>
        public static Microsoft.SqlServer.Management.Smo.Agent.CompletionResult LastRunOutcome(string jobName)
        {

            Server srv = new Server(new DataDriver().NameOfHdsServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            return specificJob.LastRunOutcome;

        }

        /// <summary>
        /// Check to see if the job has run after a particular time
        /// </summary>
        /// <param name="jobName">The name of the job to check</param>
        /// <param name="checkStartTime">The time to check to see if it has run after</param>
        /// <returns>true if the job has run after the input time</returns>
        public static bool CheckIfJobRunAfter(string jobName, DateTime checkStartTime)
        {

            Server srv = new Server(new DataDriver().NameOfHdsServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            if (specificJob.LastRunDate > checkStartTime)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Disable a job
        /// </summary>
        /// <param name="jobName">Name of job to disable</param>
        public static void DisableJob(string jobName)
        {

            Server srv = new Server(new DataDriver().NameOfHdsServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

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

            Server srv = new Server(new DataDriver().NameOfHdsServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

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

            Server srv = new Server(new DataDriver().NameOfHdsServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            return specificJob.IsEnabled;

        }

        /// <summary>
        /// Wait until a job is run specified either by the default 6 minutes or what is passed across
        /// </summary>
        /// <param name="jobName">Name of job to check</param>
        /// <param name="checkStartTime">Check the run time against this time. Once the run exceeds this value this method ends</param>
        /// <param name="waitTimeSecondsOverride">If you want a different value to 6 minutes</param>
        /// <returns>A true means the job has run, an exception is thrown if it is not</returns>
        public static bool WaitUntilJobRun(string jobName, DateTime checkStartTime, Int32 waitTimeSecondsOverride = 0)
        {

            bool loopUntilTrue = false;
            try
            {
                Int32 waitTimeSeconds = waitTimeSecondsOverride == 0 ? 60 * 6 * 1000 : waitTimeSecondsOverride; // Convert to 6 mins

                Server srv = new Server(new DataDriver().NameOfHdsServer);

                JobServer sqlServerAgent = srv.JobServer;
                Job specificJob = sqlServerAgent.Jobs[jobName];

                do
                {
                    // Check run time until it has run
                    if (specificJob.LastRunDate > checkStartTime)
                    {
                        loopUntilTrue = true;
                    }
                    else
                    {
                        loopUntilTrue = false;
                    }
                    Thread.Sleep(5000); // Pause 5 seconds and check again
                    waitTimeSeconds -= 5000;

                    // If we have exceeded the wait time then throw an exception
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Threading;


namespace Wonga.QA.Framework.SMO
{
    /// <summary>
    /// This class will be used for working with SQL Server Agent jobs
    /// </summary>
    public class Jobs
    {
        public static bool Execute(string jobName)
        {

            Server srv = new Server(Drive.Data.NameOfServer);
            Job jb = new Job(srv.JobServer, jobName);
 
            if (jb.IsEnabled == true)
            {
                jb.Start();
                while (jb.CurrentRunStatus != JobExecutionStatus.Suspended && jb.CurrentRunStatus != JobExecutionStatus.Idle )
                {
                    Thread.Sleep(100);
                }

            }
   
            return false;
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
                //DateTime? lastRun = null;
                Server srv = new Server(Drive.Data.NameOfServer);

                JobServer sqlServerAgent = srv.JobServer;
                Job specificJob = sqlServerAgent.Jobs[jobName];

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
            Server srv = new Server(Drive.Data.NameOfServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            return specificJob.CurrentRunStatus;

        }

        /// <summary>
        /// Check if the job is enabled
        /// </summary>
        /// <param name="jobName">Enter the job name</param>
        /// <returns></returns>
        public static bool CheckIsJobEnabled(string jobName)
        {

            Server srv = new Server(Drive.Data.NameOfServer);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            return specificJob.IsEnabled;

        }

        /// <summary>
        /// Check to see if a SQL Server agent job has run after a specific time
        /// </summary>
        /// <param name="jobName">The name of the job to check</param>
        /// <param name="checkStartTime">The time</param>
        /// <returns></returns>
        public static bool CheckIfJobRunAfter(string jobName, DateTime checkStartTime)
        {

            Server srv = new Server(Drive.Data.NameOfServer);

            return true;

        }

        /// <summary>
        /// Disable a particular job
        /// </summary>
        /// <param name="jobName">The name of the job to disable</param>
        public static void DisableJob(string jobName)
        {

            Server srv = new Server(Drive.Data.NameOfServer);
            Job jb = new Job(srv.JobServer, jobName);

            jb.IsEnabled = false;

        }

        /// <summary>
        /// Enable a particular job
        /// </summary>
        /// <param name="jobName">The name of the job to enable</param>
        public static void EnableJob(string jobName)
        {

            Server srv = new Server(Drive.Data.NameOfServer);
            Job jb = new Job(srv.JobServer, jobName);

            jb.IsEnabled = true;

        }

        /// <summary>
        /// Wait until a job has specifically run. This is a "wait" 
        /// So one example would be get the time, the check Wait until it has run after that time
        /// </summary>
        /// <param name="jobName">The name of the job to check</param>
        /// <param name="checkStartTime">Input a date time to check for after a job has run</param>
        /// <returns></returns>
        public static bool WaitUntilJobRun(string jobName, DateTime checkStartTime)
        {

            Server srv = new Server(Drive.Data.NameOfServer);
            Job jb = new Job(srv.JobServer, jobName);

            if (jb.LastRunDate > checkStartTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

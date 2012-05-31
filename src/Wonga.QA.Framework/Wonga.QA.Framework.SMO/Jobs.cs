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
        public static bool Execute(string serverName, string jobName)
        {

            Server srv = new Server(serverName);
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
        public static DateTime? GetJobLastRunDateTime(string serverName, string jobName)
        {

            try
            {
                //DateTime? lastRun = null;
                Server srv = new Server(serverName);

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
        public Microsoft.SqlServer.Management.Smo.Agent.JobExecutionStatus CheckJobStatus(string serverName, string jobName)
        {
            Server srv = new Server(serverName);

            JobServer sqlServerAgent = srv.JobServer;
            Job specificJob = sqlServerAgent.Jobs[jobName];

            return specificJob.CurrentRunStatus;

        }

        public static bool CheckIfJobRunAfter(string serverName, string jobName, DateTime checkStartTime)
        {

            Server srv = new Server(serverName);

            return true;

        }

        public static void DisableJob(string serverName, string jobName)
        {

            Server srv = new Server(serverName);
            Job jb = new Job(srv.JobServer, jobName);

            jb.IsEnabled = false;

        }

        public static void EnableJob(string serverName, string jobName)
        {

            Server srv = new Server(serverName);
            Job jb = new Job(srv.JobServer, jobName);

            jb.IsEnabled = true;

        }

        public static bool WaitUntilJobRun(string serverName, string jobName, DateTime checkStartTime)
        {

            Server srv = new Server(serverName);
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

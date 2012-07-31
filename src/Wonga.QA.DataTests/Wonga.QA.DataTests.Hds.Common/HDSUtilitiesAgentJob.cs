using System;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Microsoft.SqlServer.Management.Smo.Agent;

namespace Wonga.QA.DataTests.Hds.Common
{
    public class HdsUtilitiesAgentJob : HdsUtilitiesBase
    {
        public HdsUtilitiesAgentJob(WongaService wongaService)
            : base(wongaService)
        { }

        #region "Properties.Public"

        public static Int32 CdcWaitTimeMilliseconds = 5000;

        /// <summary>
        /// Return the CDC Staging Agent Job name base on the CDC Database Name
        /// </summary>
        public string CdcStagingAgentJob
        {
            get { return CDCDatabaseName + "_" + WongaServiceName + "Load"; }
        }

        /// <summary>
        /// Return the HDS Payments Load Job name base on the HDS Database Name
        /// </summary>
        public string HdsLoadAgentJob
        {
            get { return HDSDatabaseName + "_" + WongaServiceName + "Load"; }
        }

        /// <summary>
        /// Return the HDS Payments Initial Load Job name base on the HDS Database Name
        /// </summary>
        public string HdsInitialLoadAgentJob
        {
            get { return HDSDatabaseName + "_" + WongaServiceName + "InitialLoad"; }
        }

        /// <summary>
        /// Return the HDS reconcilliation Job name base on the HDS Database Name
        /// </summary>
        public string HdsReconcileAgentJob
        {
            get { return HDSDatabaseName + "_" + WongaServiceName + "Reconciliation"; }
        }

        #endregion "Properties.Public"

        #region "Methods.Private"

        /// <summary>
        /// Returns SSIS package name for a system component
        /// </summary>
        /// <param name="systemComponent">What system component to return the SSIS package for</param>
        /// <returns>name of package</returns>
        private string PackageName(SystemComponent systemComponent)
        {
            string returnValue;

            switch (systemComponent)
            {
                case SystemComponent.CDCStaging:
                    returnValue = "Load" + WongaServiceName + "_MainPackage";
                    break;
                case SystemComponent.HDS:
                    returnValue = "LoadHds_" + WongaServiceName + "Master";
                    break;
                default:
                    throw new Exception(string.Format("System component of [{0}] is unknown."));
            }

            return returnValue;
        }

        #endregion "Methods.Private"

        #region "Methods.Public"

        /// <summary>
        /// Disables SQL Agent job
        /// </summary>
        /// <param name="jobName">Job to disable</param>
        /// <exception cref="ArgumentNullException">jobName must be provided</exception>
        /// <returns>Flag indicating if the job was enabled before disabling it</returns>
        public bool DisableJob(string jobName)
        {
            bool jobWasEnabled;

            if (SQLServerAgentJobs.CheckIsJobEnabled(jobName))
            {
                SQLServerAgentJobs.DisableJob(jobName);

                jobWasEnabled = true;
            }
            else
            {
                jobWasEnabled = false;
            }

            return jobWasEnabled;
        }

        /// <summary>
        /// Enables SQL Agent job
        /// </summary>
        /// <param name="jobName">Job to enable</param>
        /// <exception cref="ArgumentNullException">jobName must be provided</exception>
        /// <returns>Flag indicating if the job was disabled before enabling it</returns>
        public bool EnableJob(string jobName)
        {
            bool jobWasDisabled;

            if (!SQLServerAgentJobs.CheckIsJobEnabled(jobName))
            {
                SQLServerAgentJobs.EnableJob(jobName);

                jobWasDisabled = true;
            }
            else
            {
                jobWasDisabled = false;
            }

            return jobWasDisabled;
        }
      
        /// <summary>
        /// Start SQL Agent Job if not already executing
        /// </summary>
        /// <param name="jobName">Job to start</param>
        /// <param name="checkJobExecutingFlag">Flag to check if job is executing after starting</param>
        /// <returns>Returns true if the job was idle before it was started</returns>
        public bool StartJob(string jobName, bool checkJobExecutingFlag = true)
        {
            bool jobWasIdle = SQLServerAgentJobs.CheckJobStatus(jobName) == JobExecutionStatus.Idle;

            if (SQLServerAgentJobs.CheckJobStatus(jobName) != JobExecutionStatus.Executing)
            {
                const int waitTime = 1000;
                int totalWaitTime = 30000;

                string message = string.Format("Job [{0}] did not become idle in [{1}] milliseconds.", jobName, totalWaitTime.ToString());

                // wait for the job to become idle if it is finishing up, e.g. PerformingCompletionAction
                while (SQLServerAgentJobs.CheckJobStatus(jobName) != JobExecutionStatus.Idle)
                {
                    System.Threading.Thread.Sleep(waitTime);

                    totalWaitTime -= waitTime;

                    if (totalWaitTime == 0)
                    {
                        throw new Exception(message + string.Format(" Current status is [{0}].", SQLServerAgentJobs.CheckJobStatus(jobName)));
                    }
                }

                SQLServerAgentJobs.Start(jobName);

                if (checkJobExecutingFlag)
                {
                    // wait for the job to start
                    totalWaitTime = 5000;
                    JobExecutionStatus jobExecutionStatus;

                    do
                    {
                        jobExecutionStatus = SQLServerAgentJobs.CheckJobStatus(jobName);

                        System.Threading.Thread.Sleep(waitTime);

                        totalWaitTime -= waitTime;

                        if (totalWaitTime == 0)
                        {
                            throw new Exception(string.Format(
                                "Job [{0}] could not be started, current status is [{1}].",
                                jobName, jobExecutionStatus));
                        }
                    } while (jobExecutionStatus != JobExecutionStatus.Executing);
                }
                else
                {
                    System.Threading.Thread.Sleep(waitTime*5);
                }
            }

            return jobWasIdle;
        }

        /// <summary>
        /// Stop Job if executing
        /// </summary>
        /// <param name="jobName">Job to stop</param>
        /// <returns>Returns true if the job was executing before it was stopped</returns>
        public bool StopJob(string jobName)
        {
            bool jobWasExecuting = SQLServerAgentJobs.CheckJobStatus(jobName) == JobExecutionStatus.Executing;

            const int waitTime = 1000;
            int totalWaitTime = 30000;

            string message = string.Format("Job [{0}] did not become idle in [{1}] milliseconds.", jobName, totalWaitTime.ToString());

            while (SQLServerAgentJobs.CheckJobStatus(jobName) != JobExecutionStatus.Idle)
            {
                if (SQLServerAgentJobs.CheckJobStatus(jobName) == JobExecutionStatus.Executing)
                {
                    SQLServerAgentJobs.Stop(jobName);
                }

                System.Threading.Thread.Sleep(waitTime);

                totalWaitTime -= waitTime;

                if (totalWaitTime == 0)
                {
                    throw new Exception(message + string.Format(" Current status is [{0}].", SQLServerAgentJobs.CheckJobStatus(jobName)));
                }
            }

            return jobWasExecuting;
        }

        /// <summary>
        /// Check LastSuccessfulLoad table for Load has completed an iteration
        /// </summary>
        /// <param name="systemComponent"> </param>
        public void WaitForLoadExecutionCycle(SystemComponent systemComponent)
        {
            string packageName = PackageName(systemComponent);

            dynamic targetDb;

            switch (systemComponent)
            {
                case SystemComponent.CDCStaging:
                    targetDb = Drive.Data.Cdc.Db;
                    break;
                case SystemComponent.HDS:
                    targetDb = Drive.Data.Hds.Db;
                    break;
                default:
                    throw new Exception(string.Format("System component of [{0}] is unknown.", systemComponent));
            }

            // check table to see if a load has occurred
            try
            {
                DateTime now =
                    targetDb.vw_SelectForTesting.All().Select(targetDb.vw_SelectForTesting.GetDate).ToScalar<DateTime>();

                var record = Do.With.Interval(2).Until(() => targetDb.LastSuccessfulLoad.Find(targetDb.LastSuccessfulLoad.PackageName == packageName && targetDb.LastSuccessfulLoad.StartRunDatetime >= now && targetDb.LastSuccessfulLoad.LastSuccessfulRunDatetime > targetDb.LastSuccessfulLoad.StartRunDatetime));
            }
            catch (Exception e)
            {

                throw new Exception(string.Format("Execution cycle for [{0}] SSIS package [{1}] did not occur. Check table dbo.LastSuccessfulLoad for last execution time. Exception is [{2}].", systemComponent, packageName, e));
            }
        }

        #endregion "Methods.Public"
    }
}


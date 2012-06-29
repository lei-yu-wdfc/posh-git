using System;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Smo.Agent;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.DataTests.Hds
{
    internal static class HdsUtilities
    {
        internal enum PaymentEntity
        {
            Application,
            BankAccountsBase
        }

        /// <summary>
        /// Define or retrieve the Region
        /// This will need to change when we have different set ups (like WB for ZA)
        /// </summary>
        internal static string Region { get { return Config.AUT == AUT.Wb ? "Uk" : Config.AUT.ToString(); } }
        /// <summary>
        /// Define or retreive the Product
        /// </summary>
        internal static string Product { get { return Config.AUT == AUT.Wb ? Config.AUT.ToString() : ""; } }

        /// <summary>
        /// Return the CDC Database Name
        /// </summary>
        internal static string CDCDatabaseName
        {
            get
            {
                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "CDCStaging";
            }
        }

        /// <summary>
        /// Return the HDS database Name
        /// </summary>
        internal static string HDSDatabaseName
        {
            get
            {
                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "WongaHDS";
            }
        }

        /// <summary>
        /// Return the CDC Staging Agent Job name base on the CDC Database Name
        /// </summary>
        internal static string CdcStagingAgentJob
        {
            get { return CDCDatabaseName + "_PaymentsLoad"; }
        }

        /// <summary>
        /// Return the HDS Payments Load Job name base on the HDS Database Name
        /// </summary>
        internal static string HdsLoadAgentJob
        {
            get { return HDSDatabaseName + "_PaymentsLoad"; }
        }

        /// <summary>
        /// Return the HDS Payments Initial Load Job name base on the HDS Database Name
        /// </summary>
        internal static string HdsInitialLoadAgentJob
        {
            get { return HDSDatabaseName + "_PaymentsInitialLoad"; }
        }

        /// <summary>
        /// Return the HDS reconcilliation Job name base on the HDS Database Name
        /// </summary>
        internal static string HdsReconcileAgentJob
        {
            get { return HDSDatabaseName + "_PaymentsReconciliation"; }
        }

        /// <summary>
        /// Checks CDC Staging for record 
        /// </summary>
        /// <param name="externalId">Record to find</param>
        /// <param name="entity">Entity to check</param>
        /// <exception cref="ArgumentNullException">externalId, entity must be provided</exception>
        internal static void CheckRecordInCdcStaging(Guid externalId, string entity)
        {
            Trace.WriteLine(String.Format("Checking [{0}] in CDC Staging for External Id in [{1}]", entity, externalId));

            try
            {
                // TODO: Extend to check a passed in entity, not just Application
                var recordInCdcStaging =
                    Do.Until(() => Drive.Data.Cdc.Db.Payment.Applications.FindBy(ExternalId: externalId));
            }
            catch (Exception e)
            {
                throw new Exception(
                    String.Format("Record not found in [{0}] in CDC Staging, Error = [{1}], Trace = [{2}].", entity,
                                  e.Message, e.ToString()));
            }
        }

        /// <summary>
        /// Checks HDS for record 
        /// </summary>
        /// <param name="externalId">Record to find</param>
        /// <param name="entity">Entity to check</param>
        /// <exception cref="ArgumentNullException">externalId, entity must be provided</exception>
        internal static void CheckRecordInHds(Guid externalId, string entity)
        {
            Trace.WriteLine(String.Format("Checking [{0}] in HDS for External Id in [{1}]", entity, externalId));

            try
            {
                // TODO: Extend to check a passed in entity, not just Application
                var recordInHds = Do.Until(() => Drive.Data.Hds.Db.Payment.Applications.FindBy(ExternalId: externalId));
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Record not found in [{0}] in HDS, Error = [{1}], Trace = [{2}].",
                                                  entity, e.Message, e.ToString()));
            }
        }

        /// <summary>
        /// Waits for a job to finish
        /// </summary>
        /// <param name="jobName">Job to check</param>
        /// <param name="waitTimeSecondsOverride">Time to wait before reporting error of job not finished</param>
        /// <exception cref="ArgumentNullException">jobName must be provided</exception>
        internal static void WaitUntilJobRun(string jobName, int waitTimeSecondsOverride = 0)
        {
            // Find the last run time
            DateTime? lastRunTime = SQLServerAgentJobs.GetJobLastRunDateTime(jobName);

            // wait for job to stop if running
            if (SQLServerAgentJobs.CheckJobStatus(jobName) != JobExecutionStatus.Idle)
            {
                SQLServerAgentJobs.WaitUntilJobRun(jobName, lastRunTime ?? DateTime.Now, waitTimeSecondsOverride);
            }

        }

        internal static void WaitUntilJobComplete(string jobName, int waitTimeSecondsOverride = 0)
        {
            // Find the last run time
            DateTime? lastRunTime = SQLServerAgentJobs.GetJobLastRunDateTime(jobName);

            // wait for job to stop if running
            if (SQLServerAgentJobs.CheckJobStatus(jobName) != JobExecutionStatus.PerformingCompletionAction)
            {
                SQLServerAgentJobs.WaitUntilJobRun(jobName, lastRunTime ?? DateTime.Now, waitTimeSecondsOverride);
            }
        }

        /// <summary>
        /// Disables SQL Agent job
        /// </summary>
        /// <param name="jobName">Job to disable</param>
        /// <exception cref="ArgumentNullException">jobName must be provided</exception>
        /// <returns>Flag indicating if the job was enabled before disabling it</returns>
        internal static bool DisableJob(string jobName)
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
        internal static bool EnableJob(string jobName)
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
    }
}

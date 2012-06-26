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

        //internal static string CdcStagingAgentJob { get { return "UK_CDCStagingLoadPayments"; }}
        //internal static string HdsLoadAgentJob { get { return "DataInsight - UK_WongaHDSpaymentsLoad"; } }
        // internal static string HdsInitialLoadAgentJob { get { return "DataInsight - UK_WongaHDS_paymentsInitialLoad"; }}
        // internal static string HdsReconcileAgentJob { get { return "DataInsight - UK_WongaHDS_paymentsReconciliation"; } }

        internal static string CdcStagingAgentJob
        {
            get { return "UK_CDCStaging_PaymentsLoad"; }
        }

        internal static string HdsLoadAgentJob
        {
            get { return "UK_WongaHDS_PaymentsLoad"; }
        }

        internal static string HdsInitialLoadAgentJob
        {
            get { return "UK_WongaHDS_PaymentsInitialLoad"; }
        }

        internal static string HdsReconcileAgentJob
        {
            get { return "UK_WongaHDS_PaymentsReconciliation"; }
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

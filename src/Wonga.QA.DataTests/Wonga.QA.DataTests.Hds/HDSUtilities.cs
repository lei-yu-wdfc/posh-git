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
        internal static Int32 CdcWaitTimeMilliseconds = 3000;

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

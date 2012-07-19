using System;
using System.Collections.Generic;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using MbUnit.Framework;

namespace Wonga.QA.DataTests.Hds.Common
{
    public class HdsUtilities
    {
        public static Int32 CdcWaitTimeMilliseconds = 3000;

        public enum WongaService
        {
            Ops = 1,
            Payments = 2,
            Comms = 3,
            Risk = 4
        }

        private WongaService WongaServiceUsed { get; set; }

        public String WongaServiceName
        {
            get { return this.WongaServiceUsed.ToString(); }
        }

        private HdsUtilities()
        {
        }

        public HdsUtilities(WongaService wongaService)
        {
            WongaServiceUsed = wongaService;
        }

        // TODO: Check still works with new config approach
        public string Service { get { return Config.AUT == AUT.Wb ? "Uk" : Config.AUT.ToString(); } }

        public string WongaServiceSchema
        {
            get
            {
                return (WongaServiceUsed == WongaService.Payments
                            ? WongaServiceName.Substring(0, WongaServiceName.Length - 1)
                            : WongaServiceName).ToLower();
            }
        }

        /// <summary>
        /// Define or retrieve the Region
        /// This will need to change when we have different set ups (like WB for ZA)
        /// </summary>
        public string Region { get { return Config.AUT == AUT.Wb ? "Uk" : Config.AUT.ToString(); } }
        /// <summary>
        /// Define or retreive the Product
        /// </summary>
        public string Product { get { return Config.AUT == AUT.Wb ? Config.AUT.ToString() : ""; } }

        /// <summary>
        /// Return the CDC Database Name
        /// </summary>
        public string CDCDatabaseName
        {
            get
            {
                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "CDCStaging";
            }
        }

        /// <summary>
        /// Return the HDS database Name
        /// </summary>
        public string HDSDatabaseName
        {
            get
            {
                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "WongaHDS";
            }
        }

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
        /// total number of record (cdc,hds,hds current view)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expectedRecoredCnt"></param>
        /// <param name="connection"></param>
        /// <param name="columnName"></param>
        /// <param name="dbName"></param>
        public void RecordCount(int key, int expectedRecoredCnt, dynamic connection, string columnName,string dbName)
        {
            var totalRecordCount = connection.FindAll(connection[columnName] == key).Count();
            Assert.IsTrue(totalRecordCount == expectedRecoredCnt, "Total Record count in [{0}] should be [{1}]", dbName, expectedRecoredCnt);
        }

        /// <summary>
        /// Select the updated record after source
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public dynamic UpdatedSourceRecord(dynamic connection, int key, string columnName)
        {
            var updatedSrcRecord = connection.Find(connection[columnName] == key);
            return updatedSrcRecord;

        }

        /// <summary>
        /// compare cdc record with source
        /// </summary>
        /// <param name="sourceRecord"></param>
        /// <param name="connection"></param>
        /// <param name="key"></param>
        /// <param name="columnName"></param>
        /// <param name="cdcOperation"></param>
        /// <returns></returns>
        public DateTime CompareCdcRecord(dynamic sourceRecord, dynamic connection, int key, string columnName, int cdcOperation)
        {
            var cdcRecord = connection.Find(connection[columnName] == key && connection.Operation == cdcOperation);
            foreach (string memberName in sourceRecord.GetDynamicMemberNames())
            {
                Assert.AreEqual(((IDictionary<string, object>)cdcRecord)[memberName], ((IDictionary<string, object>)sourceRecord)[memberName], "[{0}] in CDC should match with Source", memberName);
            }
            return cdcRecord.commit_time;
        }

        /// <summary>
        /// compare hds record with source
        /// </summary>
        /// <param name="sourceRecord"></param>
        /// <param name="connection"></param>
        /// <param name="key"></param>
        /// <param name="hdsfrmdt"></param>
        /// <param name="hdstodt"></param>
        /// <param name="columnName"></param>
        /// <param name="hdsDeleteFlag"></param>
         public void CompareHdscRecord(dynamic sourceRecord, dynamic connection, int key, DateTime hdsfrmdt, DateTime hdstodt, string columnName, int hdsDeleteFlag)
        {
            var hdsRecord = connection.Find(connection[columnName] == key && connection.HdsFromTms == hdsfrmdt && connection.HdsDeleteFlag == hdsDeleteFlag);
            foreach (string memberName in sourceRecord.GetDynamicMemberNames())
            {
                Assert.AreEqual(((IDictionary<string, object>)hdsRecord)[memberName], ((IDictionary<string, object>)sourceRecord)[memberName], "[{0}] in HDS should match with Source", memberName);
            }
            Assert.IsTrue(hdsRecord.HdsFromTms == hdsfrmdt, "HdsFromTms in Hds should match with hdsfrmdt.");
            Assert.IsTrue(hdsRecord.HdsToTms == hdstodt, "HdsToTms in Hds should match with hdstodt.");
        }

        /// <summary>
        /// compare hds view record with source
        /// </summary>
        /// <param name="sourceRecord"></param>
        /// <param name="connection"></param>
        /// <param name="key"></param>
        /// <param name="columnName"></param>
         public void CompareViewRecord(dynamic sourceRecord, dynamic connection, int key, string columnName)
        {
            var hdsCurrRecord = connection.Find(connection[columnName] == key);
            foreach (string memberName in sourceRecord.GetDynamicMemberNames())
            {
                Assert.AreEqual(((IDictionary<string, object>)hdsCurrRecord)[memberName], ((IDictionary<string, object>)sourceRecord)[memberName], "[{0}] in Hds View should match with Source", memberName);
            }
        }

         public byte[] StringToByteArray(string incomingString)
         {

             ASCIIEncoding encoded = new ASCIIEncoding();
             return encoded.GetBytes(incomingString);
         }

    }
}


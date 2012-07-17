//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Microsoft.SqlServer.Management.Smo.Agent;
//using Wonga.QA.Framework;
//using Wonga.QA.Framework.Core;
//using Wonga.QA.Framework.Data;
//using MbUnit.Framework;

//namespace Wonga.QA.DataTests.Hds
//{
//    internal class HdsUtilities
//    {
//        internal static Int32 CdcWaitTimeMilliseconds = 3000;

//        /// <summary>
//        /// Define or retrieve the Region
//        /// This will need to change when we have different set ups (like WB for ZA)
//        /// </summary>
//        internal static string Region { get { return Config.AUT == AUT.Wb ? "Uk" : Config.AUT.ToString(); } }
//        /// <summary>
//        /// Define or retreive the Product
//        /// </summary>
//        internal static string Product { get { return Config.AUT == AUT.Wb ? Config.AUT.ToString() : ""; } }

//        /// <summary>
//        /// Return the CDC Database Name
//        /// </summary>
//        internal static string CDCDatabaseName
//        {
//            get
//            {
//                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "CDCStaging";
//            }
//        }

//        /// <summary>
//        /// Return the HDS database Name
//        /// </summary>
//        internal static string HDSDatabaseName
//        {
//            get
//            {
//                return (Region.Length == 0 ? "" : Region + "_") + (Product.Length == 0 ? "" : Product + "_") + "WongaHDS";
//            }
//        }

//        /// <summary>
//        /// Return the CDC Staging Agent Job name base on the CDC Database Name
//        /// </summary>
//        internal static string CdcStagingAgentJob
//        {
//            get { return CDCDatabaseName + "_PaymentsLoad"; }
//        }

//        /// <summary>
//        /// Return the HDS Payments Load Job name base on the HDS Database Name
//        /// </summary>
//        internal static string HdsLoadAgentJob
//        {
//            get { return HDSDatabaseName + "_PaymentsLoad"; }
//        }

//        /// <summary>
//        /// Return the HDS Payments Initial Load Job name base on the HDS Database Name
//        /// </summary>
//        internal static string HdsInitialLoadAgentJob
//        {
//            get { return HDSDatabaseName + "_PaymentsInitialLoad"; }
//        }

//        /// <summary>
//        /// Return the HDS reconcilliation Job name base on the HDS Database Name
//        /// </summary>
//        internal static string HdsReconcileAgentJob
//        {
//            get { return HDSDatabaseName + "_PaymentsReconciliation"; }
//        }
       
//        /// <summary>
//        /// Disables SQL Agent job
//        /// </summary>
//        /// <param name="jobName">Job to disable</param>
//        /// <exception cref="ArgumentNullException">jobName must be provided</exception>
//        /// <returns>Flag indicating if the job was enabled before disabling it</returns>
//        internal static bool DisableJob(string jobName)
//        {
//            bool jobWasEnabled;

//            if (SQLServerAgentJobs.CheckIsJobEnabled(jobName))
//            {
//                SQLServerAgentJobs.DisableJob(jobName);

//                jobWasEnabled = true;
//            }
//            else
//            {
//                jobWasEnabled = false;
//            }

//            return jobWasEnabled;
//        }

//        /// <summary>
//        /// Enables SQL Agent job
//        /// </summary>
//        /// <param name="jobName">Job to enable</param>
//        /// <exception cref="ArgumentNullException">jobName must be provided</exception>
//        /// <returns>Flag indicating if the job was disabled before enabling it</returns>
//        internal static bool EnableJob(string jobName)
//        {
//            bool jobWasDisabled;

//            if (!SQLServerAgentJobs.CheckIsJobEnabled(jobName))
//            {
//                SQLServerAgentJobs.EnableJob(jobName);

//                jobWasDisabled = true;
//            }
//            else
//            {
//                jobWasDisabled = false;
//            }

//            return jobWasDisabled;
//        }

//        // total number of record (cdc,hds,hds current view)
//        internal static void RecordCount(int key, int expectedRecoredCnt, dynamic connection, string columnName,string dbName)
//        {
//            var totalRecordCount = connection.FindAll(connection[columnName] == key).Count();
//            Assert.IsTrue(totalRecordCount == expectedRecoredCnt, "Total Record count in [{0}] should be [{1}]", dbName, expectedRecoredCnt);
//        }

//        //Select the updated record after source
//        internal static dynamic UpdatedSourceRecord(dynamic connection, int key, string columnName)
//        {
//            var updatedSrcRecord = connection.Find(connection[columnName] == key);
//            return updatedSrcRecord;

//        }

//        //compare cdc record with source
//         internal static  DateTime CompareCdcRecord(dynamic sourceRecord, dynamic connection, int key, string columnName, int cdcOperation)
//          {
//            var cdcRecord = connection.Find(connection[columnName] == key && connection.Operation == cdcOperation);
//            foreach (string memberName in sourceRecord.GetDynamicMemberNames())
//            {
//                Assert.AreEqual(((IDictionary<string, object>)cdcRecord)[memberName], ((IDictionary<string, object>)sourceRecord)[memberName], "[{0}] in CDC should match with Source", memberName);
//            }
//            return cdcRecord.commit_time;
//        }

//        //compare hds record with source
//         internal static void CompareHdscRecord(dynamic sourceRecord, dynamic connection, int key, DateTime hdsfrmdt, DateTime hdstodt, string columnName, int hdsDeleteFlag)
//        {
//            var hdsRecord = connection.Find(connection[columnName] == key && connection.HdsFromTms == hdsfrmdt && connection.HdsDeleteFlag == hdsDeleteFlag);
//            foreach (string memberName in sourceRecord.GetDynamicMemberNames())
//            {
//                Assert.AreEqual(((IDictionary<string, object>)hdsRecord)[memberName], ((IDictionary<string, object>)sourceRecord)[memberName], "[{0}] in HDS should match with Source", memberName);
//            }
//            Assert.IsTrue(hdsRecord.HdsFromTms == hdsfrmdt, "HdsFromTms in Hds should match with hdsfrmdt.");
//            Assert.IsTrue(hdsRecord.HdsToTms == hdstodt, "HdsToTms in Hds should match with hdstodt.");
//        }

//        //compare hds view record with source
//         internal static void CompareViewRecord(dynamic sourceRecord, dynamic connection, int key, string columnName)
//        {
//            var hdsCurrRecord = connection.Find(connection[columnName] == key);
//            foreach (string memberName in sourceRecord.GetDynamicMemberNames())
//            {
//                Assert.AreEqual(((IDictionary<string, object>)hdsCurrRecord)[memberName], ((IDictionary<string, object>)sourceRecord)[memberName], "[{0}] in Hds View should match with Source", memberName);
//            }
//        }


//      }


//    }


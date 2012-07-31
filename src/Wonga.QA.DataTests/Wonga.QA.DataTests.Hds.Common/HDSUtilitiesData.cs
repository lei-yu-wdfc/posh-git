using System;
using System.Collections.Generic;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using MbUnit.Framework;
using Wonga.QA.Framework;

namespace Wonga.QA.DataTests.Hds.Common
{
    public class HdsUtilitiesData : HdsUtilitiesBase
    {
        public HdsUtilitiesData(WongaService wongaService) :base(wongaService)
        {}

        /// <summary>
        /// total number of record (cdc,hds,hds current view)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expectedRecoredCnt"></param>
        /// <param name="connection"></param>
        /// <param name="columnName"></param>
        /// <param name="dbName"></param>
        public void RecordCount(int key, int expectedRecoredCnt, dynamic connection, string columnName, string dbName)
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


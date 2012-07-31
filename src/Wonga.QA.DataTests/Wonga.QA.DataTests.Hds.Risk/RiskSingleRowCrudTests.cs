using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.DataTests.Hds.Risk
{
    [TestFixture(Order = 2)]
    [Category("Auto")]
    [Category("Risk")]
    [Parallelizable(TestScope.All)]
    class RiskSingleRowCrudTests
    {
        private static DateTime _dthdsDefaultTo = Convert.ToDateTime("31/12/9999");
        private static string _tablename = "CheckpointDefinitions";
        private static string _columnname = "CheckpointDefinitionId";
        //this part is required for insertupdatesameLSN test
        private static string _schemaName = "risk";
        dynamic _connection = Drive.Data.Risk.Db;

        private static string CurrentViewName = "vw_" + _tablename + "_Current";
        dynamic _serviceConnection = Drive.Data.Risk.Db.risk[_tablename];
        dynamic _cdcConnection = Drive.Data.Cdc.Db.risk[_tablename];
        dynamic _hdsConnection = Drive.Data.Hds.Db.risk[_tablename];
        dynamic _hdsCurrViewConnection = Drive.Data.Hds.Db.risk[CurrentViewName];

        private HdsUtilitiesAgentJob _hdsUtilitiesAgentJob = null;
        private HdsUtilitiesData _hdsUtilitiesData = null;
        private bool _cdcStagingAgentJobWasStopped;
        private bool _hdsAgentJobWasStopped;

        [FixtureSetUp]
        [Description("This is the text fixture setup for all tests")]
        public void FixtureSetup()
        {
            _hdsUtilitiesAgentJob = new HdsUtilitiesAgentJob(HdsUtilitiesBase.WongaService.Risk);

            _cdcStagingAgentJobWasStopped = _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob);
            _hdsAgentJobWasStopped = _hdsUtilitiesAgentJob.StartJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);

            _hdsUtilitiesData = new HdsUtilitiesData(HdsUtilitiesBase.WongaService.Risk);
        }

        [FixtureTearDown]
        [Description("This is the text fixture teardown for all tests")]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasStopped)
            {
                _hdsUtilitiesAgentJob.StopJob(_hdsUtilitiesAgentJob.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasStopped)
            {
                _hdsUtilitiesAgentJob.StopJob(_hdsUtilitiesAgentJob.HdsLoadAgentJob);
            }
        }

        /// <summary>
        /// insert new record 
        /// </summary>
        /// <returns></returns>
        private dynamic InsertNewRecord(dynamic connection)
        {
            var sourceRecord = connection.Insert(TypeName: Get.RandomString(200),
                                                 Name: Get.RandomString(200),
                                                 FailedDescription: Get.RandomString(128),
                                                 ResolutionAdviceKey: Get.RandomString(50),
                                                 CheckpointId:Guid.NewGuid());

            return sourceRecord;
        }


        //Update record
        private void UpdateRecord(dynamic connection, int key)
        {
            connection.UpdateByCheckpointDefinitionId(CheckpointDefinitionId: key,
                                                     Name: "ModifiedTest");
        }

        //Delete record
        private void DeleteRecord(dynamic connection, int key)
        {
            connection.DeleteByCheckpointDefinitionId(CheckpointDefinitionId: key);
        }

        
        [Test]
        [Description("Insert a record in to source db and check CDC and HDS")]
        public void InsertRecordAndConfirmItIsInCDCStagingAndHds()
        {

            Trace.WriteLine("Running InsertRecordAndConfirmItIsInCDCStagingAndHds.");

            //Inserting new record Payments.payment.CalendarDates
            var sourceRecord = InsertNewRecord(_serviceConnection);
            int key = (int)((IDictionary<string, object>)sourceRecord)[_columnname];

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            //Check total record count in Cdc 
            _hdsUtilitiesData.RecordCount(key, 1, _cdcConnection, _columnname, "CDCStaging");

            //Select the inserted record from cdcstaging and check it matches with source data
            DateTime cdcCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 2);

            //Check the Total Record count in HDS
            _hdsUtilitiesData.RecordCount(key, 1, _hdsConnection, _columnname, "Hds");

            // checking  record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, cdcCommittime, _dthdsDefaultTo, _columnname, 0);

            //Checking record count in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.RecordCount(key, 1, _hdsCurrViewConnection, _columnname, "HdsCurrentView");

            //Checking data in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.CompareViewRecord(sourceRecord, _hdsCurrViewConnection, key, _columnname);

        }

        [Test]
        [Description("Inser and update same load same LSN test")]
        public void InsertAndUpdateSameLoadSameLsn()
        {

            //insert and update within a single transaction and check cdc contain both inserted and updated record and hds contains only updated record
            Trace.WriteLine("Running Insert And Update Same Load Same Lsn.");

            // insert and update the same record
            using (var transaction = _connection.BeginTransaction())
            {
                var sourceRecord = InsertNewRecord(transaction[_schemaName][_tablename]);
                int key = (int)((IDictionary<string, object>)sourceRecord)[_columnname];
                UpdateRecord(transaction[_schemaName][_tablename], key);
                transaction.Commit();

                var updatedSrcRecord = _hdsUtilitiesData.UpdatedSourceRecord(_serviceConnection, key, _columnname);
                // Allow for CDC to pick up the data
                Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

                // Wait for hds and cdc load job to run
                _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
                _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

                //Check total record count in Cdc 
                _hdsUtilitiesData.RecordCount(key, 3, _cdcConnection, _columnname, "CDCStaging");

                //Select the inserted record from cdcstaging and check it matches with inserted source data
                _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 2);

                //Select the updated record from cdcstaging and check it matches with updated source data 
                DateTime updatedCommittime = _hdsUtilitiesData.CompareCdcRecord(updatedSrcRecord, _cdcConnection, key, _columnname, 4);

                //Check the Total Record count in HDS
                _hdsUtilitiesData.RecordCount(key, 1, _hdsConnection, _columnname, "Hds");

                //Checking  only updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
                _hdsUtilitiesData.CompareHdscRecord(updatedSrcRecord, _hdsConnection, key, updatedCommittime, _dthdsDefaultTo, _columnname, 0);

                //Checking record count in payment.vw_CalendarDates_Current view
                _hdsUtilitiesData.RecordCount(key, 1, _hdsCurrViewConnection, _columnname, "HdsCurrentView");

                //Checking data in payment.vw_CalendarDates_Current view
                _hdsUtilitiesData.CompareViewRecord(updatedSrcRecord, _hdsCurrViewConnection, key, _columnname);

            }
        }

        [Test]
        [Description("Insert and update same load with different LSN")]
        public void InsertAndUpdateSameLoadDifferentLsn()
        {
            //insert and update and check cdc contain both inserted and update record and also hds will contain inserted and updated record
            Trace.WriteLine("Running  Insert and Update Same Load Different Lsn test.");

            // insert New record
            var sourceRecord = InsertNewRecord(_serviceConnection);
            int key = (int)((IDictionary<string, object>)sourceRecord)[_columnname];

            //updating inserted record
            UpdateRecord(_serviceConnection, key);

            //selected the source record after update
            var updatedSrcRecord = _hdsUtilitiesData.UpdatedSourceRecord(_serviceConnection, key, _columnname);

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            //Check total record count in Cdc 
            _hdsUtilitiesData.RecordCount(key, 3, _cdcConnection, _columnname, "CDCStaging");

            //Select the inserted record from cdcstaging and check it matches inserted source data
            DateTime insertedCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 2);

            //Select the updated record from cdcstaging and check it matches with source data after update
            DateTime updatedCommittime = _hdsUtilitiesData.CompareCdcRecord(updatedSrcRecord, _cdcConnection, key, _columnname, 4);

            //Check the Total Record count in HDS
            _hdsUtilitiesData.RecordCount(key, 2, _hdsConnection, _columnname, "Hds");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, insertedCommittime, updatedCommittime, _columnname, 0);

            //Checking  updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(updatedSrcRecord, _hdsConnection, key, updatedCommittime, _dthdsDefaultTo, _columnname, 0);

            //Checking record count in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.RecordCount(key, 1, _hdsCurrViewConnection, _columnname, "HdsCurrentView");

            //Checking data in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.CompareViewRecord(updatedSrcRecord, _hdsCurrViewConnection, key, _columnname);

        }


        [Test]
        [Description("Insert and update with different load and different LSN")]
        public void InsertAndUpdateDifferentLoadDifferentLsn()
        {
            Trace.WriteLine("Running Insert and Update Different Load Different Lsn test.");


            //----------------------------------- First Load----------------------------------------------

            // insert New record
            var sourceRecord = InsertNewRecord(_serviceConnection);
            int key = (int)((IDictionary<string, object>)sourceRecord)[_columnname];

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            //----------------------------------- 2nd Load----------------------------------------------

            //updating inserted record
            UpdateRecord(_serviceConnection, key);

            //selected the source record after update
            var updatedSrcRecord = _hdsUtilitiesData.UpdatedSourceRecord(_serviceConnection, key, _columnname);

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            //----------------------------------- Check cdc and hds data----------------------------------------------
            //Check total record count in Cdc 
            _hdsUtilitiesData.RecordCount(key, 3, _cdcConnection, _columnname, "CDCStaging");


            //Select the inserted record from cdcstaging and check it matches inserted source data
            DateTime insertedCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 2);

            //Select the updated record from cdcstaging and check it matches with source data after update
            DateTime updatedCommittime = _hdsUtilitiesData.CompareCdcRecord(updatedSrcRecord, _cdcConnection, key, _columnname, 4);

            //Check the Total Record count in HDS
            _hdsUtilitiesData.RecordCount(key, 2, _hdsConnection, _columnname, "Hds");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, insertedCommittime, updatedCommittime, _columnname, 0);

            //Checking  updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(updatedSrcRecord, _hdsConnection, key, updatedCommittime, _dthdsDefaultTo, _columnname, 0);


            //Checking record count in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.RecordCount(key, 1, _hdsCurrViewConnection, _columnname, "HdsCurrentView");

            //Checking data in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.CompareViewRecord(updatedSrcRecord, _hdsCurrViewConnection, key, _columnname);

        }

        [Test]
        [Description("Insert and delete  with same load but different LSN")]
        public void InsertAndDeleteSameLoadDifferentLsn()
        {
            Trace.WriteLine("Running  Insert and Delete  Same Load Different Lsn test.");

            // insert New record
            var sourceRecord = InsertNewRecord(_serviceConnection);
            int key = (int)((IDictionary<string, object>)sourceRecord)[_columnname];


            //Delete the inserted record
            DeleteRecord(_serviceConnection, key);

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            //Check total record count in Cdc 
            _hdsUtilitiesData.RecordCount(key, 2, _cdcConnection, _columnname, "CDCStaging");


            //Select the inserted record from cdcstaging and check it matches source data
            DateTime insertedCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 2);


            //Select the delete record from cdcstaging and check it matches with source data 
            DateTime deletedCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 1);

            //Check the Total Record count in HDS
            _hdsUtilitiesData.RecordCount(key, 2, _hdsConnection, _columnname, "Hds");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, insertedCommittime, deletedCommittime, _columnname, 0);

            //Checking deleted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, deletedCommittime, _dthdsDefaultTo, _columnname, 1);

            //Checking record count in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.RecordCount(key, 0, _hdsCurrViewConnection, _columnname, "HdsCurrentView");

        }

        [Test]
        [Description("Insert and delete  with different load and different LSN")]
        public void InsertAndDeleteDifferentLoadDifferentLsn()
        {
            Trace.WriteLine("Running Insert and Delete  Same Load Different Lsn test.");

            //-------------------first load...................................................
            // insert New record
            var sourceRecord = InsertNewRecord(_serviceConnection);
            int key = (int)((IDictionary<string, object>)sourceRecord)[_columnname];

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);


            //-------------------2nd load.........................................................
            //Delete the inserted record
            DeleteRecord(_serviceConnection, key);

            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);

            // Wait for hds and cdc load job to run
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            _hdsUtilitiesAgentJob.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);

            //-------------------check hds and cdc record after insert and delete.........................................................
            //Check total record count in Cdc 
            _hdsUtilitiesData.RecordCount(key, 2, _cdcConnection, _columnname, "CDCStaging");

            //Select the inserted record from cdcstaging and check it matches source data
            DateTime insertedCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 2);

            //Select the delete record from cdcstaging and check it matches with source data 
            DateTime deletedCommittime = _hdsUtilitiesData.CompareCdcRecord(sourceRecord, _cdcConnection, key, _columnname, 1);

            //Check the Total Record count in HDS
            _hdsUtilitiesData.RecordCount(key, 2, _hdsConnection, _columnname, "Hds");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, insertedCommittime, deletedCommittime, _columnname, 0);

            //Checking deleted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            _hdsUtilitiesData.CompareHdscRecord(sourceRecord, _hdsConnection, key, deletedCommittime, _dthdsDefaultTo, _columnname, 1);

            //Checking record count in payment.vw_CalendarDates_Current view
            _hdsUtilitiesData.RecordCount(key, 0, _hdsCurrViewConnection, _columnname, "HdsCurrentView");

        }
    }
 }




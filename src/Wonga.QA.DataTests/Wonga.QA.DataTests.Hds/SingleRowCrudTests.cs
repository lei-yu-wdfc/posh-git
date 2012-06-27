using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    internal class SingleRowCrudTests
    {
        private bool _cdcStagingAgentJobWasDisabled;
        private bool _hdsAgentJobWasDisabled;
        private static DateTime _dthdsDefaultTo = Convert.ToDateTime("31/12/9999");
        bool boolIsBankHoliday = false;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);
            _hdsAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasDisabled)
            {
                HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasDisabled)
            {
                HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob);
            }
        }

        public void WaitForAgentJob()
        {
            //execute cdc and hds load
            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);

        }

        public static DateTime nextAvailabledateToInsert()
        {
            DateTime maxdate =Drive.Data.Payments.Db.payment.CalendarDates.All().Select(Drive.Data.Payments.Db.payment.CalendarDates.Date.Max()).ToScalarOrDefault<DateTime>();
            DateTime newAvailabledate = maxdate.AddDays(1);
            return newAvailabledate;
        }

        [Test]
        public void InsertRecordAndConfirmItIsInCDCStagingAndHds()
        {

            Trace.WriteLine("Running InsertRecordAndConfirmItIsInCDCStagingAndHds.");

            //Inserting new record Payments.payment.CalendarDates
            var sourceRecord = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                                   IsBankHoliday: 1,
                                                                                   CreatedOn: DateTime.Now);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();


            //Check total record count in Cdc 
            var totalRecordCountInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInCdc == 1, "Total Record count in CDC should be 1");


            //Select the inserted record from cdcstaging and check it matches with source data
            var cdcRecord = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(cdcRecord.Date == sourceRecord.Date, "Date in CDC should match with Source.");
            Assert.IsTrue(cdcRecord.IsBankHoliday == sourceRecord.IsBankHoliday,"IsBankHoliday in CDC should match with Source.");
            Assert.IsTrue(cdcRecord.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in CDC should match with Source.");


            //Check the Total Record count in HDS
            var totalRecordCountInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInHds == 1, "Total Record count in HDS should be 1");

            //Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var hdsRecord = Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(hdsRecord.Date == sourceRecord.Date, "Date in Hds should match with Source.");
            Assert.IsTrue(hdsRecord.IsBankHoliday == sourceRecord.IsBankHoliday,"IsBankHoliday in Hds should match with Source.");
            Assert.IsTrue(hdsRecord.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in Hds should match with Source.");
            Assert.IsTrue(hdsRecord.HdsFromTms == cdcRecord.commit_time,"HdsFromTms in Hds should match with commit_time in CDC.");
            Assert.IsTrue(hdsRecord.HdsToTms == _dthdsDefaultTo, "HdsToTms in Hds should match with DefaultTodate.");


            //Checking record count in payment.vw_CalendarDates_Current view
            var totalhdsCurrRecord =Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalhdsCurrRecord == 1, "Total Record count in HDS Current View  should be 1");


            //Checking data in payment.vw_CalendarDates_Current view
            var hdsCurrRecord = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(hdsCurrRecord.Date == sourceRecord.Date, "Date in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.IsBankHoliday == sourceRecord.IsBankHoliday,"IsBankHoliday in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds View should match with Source.");

        }

        [Test]
        public void CalendarDatesInsertAndUpdateSameLoadSameLsn()
        {

            //insert and update within a single transaction and check cdc contain both inserted and updated record and hds contains only updated record
            Trace.WriteLine("Running CalendarDatesInsertAndUpdateSameLoadSameLsn.");

            // insert and update the same record
            using (var transaction = Drive.Data.Payments.Db.BeginTransaction())
            {

                var sourceRecord = transaction.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                            IsBankHoliday: 1,
                                                                            CreatedOn: DateTime.Now);

                transaction.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: sourceRecord.CalendarDateId,
                                                                          IsBankHoliday: boolIsBankHoliday);
                transaction.Commit();

                // Wait for hds and cdc load job to run
                WaitForAgentJob();

                //Check total record count in Cdc 
                var totalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
                Assert.IsTrue(totalRecordCountInCdc == 3, "Total Record count in CDC should be 3");


                //Select the inserted record from cdcstaging and check it matches with inserted source data
                var insertedRecordInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 2);
                Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in inserted cdc Record should match with Source.");
                Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted cdc Record should match with inserted Source value.");
                Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted cdc Record should match with Source.");

                //Select the updated record from cdcstaging and check it matches with updated source data 
                var updatedRecordInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 4);
                Assert.IsTrue(updatedRecordInCdc.Date == sourceRecord.Date, "Date in updated cdc Record should match with Source.");
                Assert.IsTrue(updatedRecordInCdc.IsBankHoliday == boolIsBankHoliday, "IsBankHoliday in updated cdc Record should match with updated value.");
                Assert.IsTrue(updatedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in updated cdc Record should match with Source .");


                //Check the Total Record count in HDS
                var totalRecordCountInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
                Assert.IsTrue(totalRecordCountInHds == 1, "Total Record count in HDS should be 1");

                //Checking  only updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
                var insertedRecordInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
                Assert.IsTrue(insertedRecordInHds.Date == sourceRecord.Date, "Date in Hds should match with Source.");
                Assert.IsTrue(insertedRecordInHds.IsBankHoliday == boolIsBankHoliday, "IsBankHoliday in Hds should match with Source.");
                Assert.IsTrue(insertedRecordInHds.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds should match with Source.");
                Assert.IsTrue(insertedRecordInHds.HdsFromTms == updatedRecordInCdc.commit_time, "HdsFromTms in Hds should match with commit time updated cdc Record.");
                Assert.IsTrue(insertedRecordInHds.HdsToTms == _dthdsDefaultTo,"HdsToTms in Hds should match with DefaultTodate.");

                //Checking record count in payment.vw_CalendarDates_Current view
                var totalhdsCurrRecord =Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
                Assert.IsTrue(totalhdsCurrRecord == 1);

                //Checking data in payment.vw_CalendarDates_Current view
                var hdsCurrRecord =Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
                Assert.IsTrue(hdsCurrRecord.Date == sourceRecord.Date, "Date in Hds View should match with Source.");
                Assert.IsTrue(hdsCurrRecord.IsBankHoliday == boolIsBankHoliday, "IsBankHoliday in Hds View should match with Source.");
                Assert.IsTrue(hdsCurrRecord.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds View should match with Source.");

            }
        }


        [Test]
        public void CalendarDatesInsertAndUpdateSameLoadDifferentLsn()
        {
            //insert and update and check cdc contain both inserted and update record and also hds will contain inserted and updated record
            Trace.WriteLine("Running Canlenderdates Insert and Update Same Load Different Lsn test.");

            // insert New record
            var sourceRecord = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                                   IsBankHoliday: 1,
                                                                                   CreatedOn: DateTime.Now);

            //updating inserted record
            Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: sourceRecord.CalendarDateId,
                                                                                 IsBankHoliday: boolIsBankHoliday);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();

            //Check total record count in Cdc 
            var totalRecordCountInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInCdc == 3, "Total Record count in CDC should be 3");


            //Select the inserted record from cdcstaging and check it matches inserted source data
            var insertedRecordInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, Operation: 2);
            Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in inserted cdc Record should match with Source.");
            Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted cdc Record should match with inserted Source value.");
            Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted cdc Record should match with Source.");

            //Select the updated record from cdcstaging and check it matches with source data after update
            var updatedRecordInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 4);
            Assert.IsTrue(updatedRecordInCdc.Date == sourceRecord.Date, "Date in updated cdc Record should match with Source.");
            Assert.IsTrue(updatedRecordInCdc.IsBankHoliday == boolIsBankHoliday, "IsBankHoliday in updated cdc Record should match with updated value.");
            Assert.IsTrue(updatedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in updated cdc Record should match with Source .");

            //Check the Total Record count in HDS
            var totalRecordCountInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInHds == 2, "Total Record count in CDC should be 2");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var insertedRecordInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,
                                                                                      IsBankHoliday: sourceRecord.IsBankHoliday);

            Assert.IsTrue(insertedRecordInHds.Date == sourceRecord.Date, "Date in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.HdsFromTms == insertedRecordInCdc.commit_time, "HdsFromTms in inserted hds Record should match with commit_time in insertedRecordInCdc.");
            Assert.IsTrue(insertedRecordInHds.HdsToTms == updatedRecordInCdc.commit_time, "HdsToTms in inserted hds Record should match with commit time in updatedRecordInCdc.");

            //Checking  updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var updatedRecordInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, IsBankHoliday: boolIsBankHoliday);
            Assert.IsTrue(updatedRecordInHds.Date == sourceRecord.Date, "Date in Updated hds Record should match with Source.");
            Assert.IsTrue(updatedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in Updated hds Record should match with Source.");
            Assert.IsTrue(updatedRecordInHds.HdsFromTms == updatedRecordInCdc.commit_time, "HdsFromTms in Updated hds Record should match with commit time in updatedRecordInCdc.");
            Assert.IsTrue(updatedRecordInHds.HdsToTms == _dthdsDefaultTo, "HdsToTms in Updated hds Record should match with DefaultTodate.");


            //Checking record count in payment.vw_CalendarDates_Current view
            var totalhdsCurrRecord =Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalhdsCurrRecord == 1);

            //Checking data in payment.vw_CalendarDates_Current view
            var hdsCurrRecord =Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(hdsCurrRecord.Date == sourceRecord.Date, "Date in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.IsBankHoliday == boolIsBankHoliday,"IsBankHoliday in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds View should match with Source.");

        }

        [Test]
        public void CalendarDatesInsertAndUpdateDifferentLoadDifferentLsn()
        {
            Trace.WriteLine("Running Canlenderdates Insert and Update Different Load Different Lsn test.");


            //----------------------------------- First Load----------------------------------------------

            // insert New record
            var sourceRecord = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                                   IsBankHoliday: 1,
                                                                                   CreatedOn: DateTime.Now);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();

            //----------------------------------- 2nd Load----------------------------------------------

            //updating inserted record
           
            Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: sourceRecord.CalendarDateId, IsBankHoliday: boolIsBankHoliday);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();

            //----------------------------------- Check cdc and hds data----------------------------------------------
            //Check total record count in Cdc 
            var totalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInCdc == 3, "Total Record count in CDC should be 3");


            //Select the inserted record from cdcstaging and check it matches inserted source data
            var insertedRecordInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, Operation: 2);
            Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in inserted cdc Record should match with Source.");
            Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted cdc Record should match with inserted Source value.");
            Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted cdc Record should match with Source.");

            //Select the updated record from cdcstaging and check it matches with source data after update
            var updatedRecordInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, Operation: 4);
            Assert.IsTrue(updatedRecordInCdc.Date == sourceRecord.Date, "Date in updated cdc Record should match with Source.");
            Assert.IsTrue(updatedRecordInCdc.IsBankHoliday == boolIsBankHoliday, "IsBankHoliday in updated cdc Record should match with updated value.");
            Assert.IsTrue(updatedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in updated cdc Record should match with Source .");

            //Check the Total Record count in HDS
            var totalRecordCountInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInHds == 2, "Total Record count in CDC should be 2");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var insertedRecordInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,
                                                                                      IsBankHoliday: sourceRecord.IsBankHoliday);

            Assert.IsTrue(insertedRecordInHds.Date == sourceRecord.Date, "Date in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.HdsFromTms == insertedRecordInCdc.commit_time, "HdsFromTms in inserted hds Record should match with commit_time in insertedRecordInCdc.");
            Assert.IsTrue(insertedRecordInHds.HdsToTms == updatedRecordInCdc.commit_time, "HdsToTms in inserted hds Record should match with commit time in updatedRecordInCdc.");

            //Checking  updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var updatedRecordInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, IsBankHoliday: boolIsBankHoliday);
            Assert.IsTrue(updatedRecordInHds.Date == sourceRecord.Date, "Date in Updated hds Record should match with Source.");
            Assert.IsTrue(updatedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in Updated hds Record should match with Source.");
            Assert.IsTrue(updatedRecordInHds.HdsFromTms == updatedRecordInCdc.commit_time, "HdsFromTms in Updated hds Record should match with commit time in updatedRecordInCdc.");
            Assert.IsTrue(updatedRecordInHds.HdsToTms == _dthdsDefaultTo, "HdsToTms in Updated hds Record should match with DefaultTodate.");

            //Checking record count in payment.vw_CalendarDates_Current view
            var totalhdsCurrRecord =
                Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId)
                    .Count();
            Assert.IsTrue(totalhdsCurrRecord == 1);

            //Checking data in payment.vw_CalendarDates_Current view
            var hdsCurrRecord =
                Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(hdsCurrRecord.Date == sourceRecord.Date, "Date in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.IsBankHoliday == boolIsBankHoliday, "IsBankHoliday in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds View should match with Source.");

        }

        [Test]

        public void CalendarDatesInsertAndDeleteSameLoadDifferentLsn()
        {
            Trace.WriteLine("Running Canlenderdates Insert and Delete  Same Load Different Lsn test.");

            // insert New record
            var sourceRecord = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                                   IsBankHoliday: 1,
                                                                                   CreatedOn: DateTime.Now);

            //Delete the inserted record
            Drive.Data.Payments.Db.payment.CalendarDates.DeleteByCalendarDateId(CalendarDateId: sourceRecord.CalendarDateId);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();

            //Check total record count in Cdc 
            var totalRecordCountInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInCdc == 2, "Total Record count in CDC should be 2");


            //Select the inserted record from cdcstaging and check it matches source data
            var insertedRecordInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 2);
            Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in inserted cdc Record should match with Source.");
            Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted cdc Record should match with inserted Source value.");
            Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted cdc Record should match with Source.");

            //Select the delete record from cdcstaging and check it matches with source data 
            var deletedRecordInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 1);
            Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in deleted cdc Record should match with Source.");
            Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in deleted cdc Record should match with inserted Source value.");
            Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in deleted cdc Record should match with Source.");

            //Check the Total Record count in HDS
            var totalRecordCountInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInHds == 2, "Total Record count in CDC should be 2");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var insertedRecordInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, HdsDeleteFlag: 0);
            Assert.IsTrue(insertedRecordInHds.Date == sourceRecord.Date, "Date in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted hds Record  should match with Source.");
            Assert.IsTrue(insertedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.HdsFromTms == insertedRecordInCdc.commit_time, "HdsFromTms in inserted hds Record should match with commit_time in insertedRecordInCdc.");
            Assert.IsTrue(insertedRecordInHds.HdsToTms == deletedRecordInCdc.commit_time, "HdsToTms in inserted hds Record should match with commit time in deletedRecordInCdc.");

            //Checking deleted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var deletedRecordInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,HdsDeleteFlag: 1);
            Assert.IsTrue(deletedRecordInHds.Date == sourceRecord.Date, "Date in deleted hds Record should match with Source.");
            Assert.IsTrue(deletedRecordInHds.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in deleted hds Record should match with Source.");
            Assert.IsTrue(deletedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in deleted hds Record should match with Source.");
            Assert.IsTrue(deletedRecordInHds.HdsFromTms == deletedRecordInCdc.commit_time, "HdsFromTms in deleted hds Record should match with commit_time in DeletedRecordInCdc.");
            Assert.IsTrue(deletedRecordInHds.HdsToTms == _dthdsDefaultTo, "HdsToTms in deleted hds Record should match with DefaultTodate.");


            //Checking record count in payment.vw_CalendarDates_Current view
            var totalhdsCurrRecord =Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalhdsCurrRecord == 0,"No of record should be 0 as the record is deleted from source");

          }

        [Test]

        public void CalendarDatesInsertAndDeleteDifferentLoadDifferentLsn()
        {
            Trace.WriteLine("Running Canlenderdates Insert and Delete  Same Load Different Lsn test.");

            //-------------------first load...................................................
            // insert New record
            var sourceRecord = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                                   IsBankHoliday: 1,
                                                                                   CreatedOn: DateTime.Now);
            // Wait for hds and cdc load job to run
            WaitForAgentJob();


            //-------------------2nd load.........................................................
            //Delete the inserted record
            Drive.Data.Payments.Db.payment.CalendarDates.DeleteByCalendarDateId(CalendarDateId: sourceRecord.CalendarDateId);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();

            //-------------------check hds and cdc record after insert and delete.........................................................
            //Check total record count in Cdc 
            var totalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInCdc == 2, "Total Record count in CDC should be 2");


            //Select the inserted record from cdcstaging and check it matches source data
            var insertedRecordInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 2);
            Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in inserted cdc Record should match with Source.");
            Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted cdc Record should match with inserted Source value.");
            Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted cdc Record should match with Source.");

            //Select the delete record from cdcstaging and check it matches with source data 
            var deletedRecordInCdc =Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,Operation: 1);
            Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date, "Date in deleted cdc Record should match with Source.");
            Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in deleted cdc Record should match with inserted Source value.");
            Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in deleted cdc Record should match with Source.");

            //Check the Total Record count in HDS
            var totalRecordCountInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInHds == 2, "Total Record count in CDC should be 2");

            //Checking  inserted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var insertedRecordInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId, HdsDeleteFlag: 0);
            Assert.IsTrue(insertedRecordInHds.Date == sourceRecord.Date, "Date in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in inserted hds Record  should match with Source.");
            Assert.IsTrue(insertedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in inserted hds Record should match with Source.");
            Assert.IsTrue(insertedRecordInHds.HdsFromTms == insertedRecordInCdc.commit_time, "HdsFromTms in inserted hds Record should match with commit_time in insertedRecordInCdc.");
            Assert.IsTrue(insertedRecordInHds.HdsToTms == deletedRecordInCdc.commit_time, "HdsToTms in inserted hds Record should match with commit time in deletedRecordInCdc.");

            //Checking deleted record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var deletedRecordInHds =Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,HdsDeleteFlag: 1);
            Assert.IsTrue(deletedRecordInHds.Date == sourceRecord.Date, "Date in deleted hds Record should match with Source.");
            Assert.IsTrue(deletedRecordInHds.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in deleted hds Record should match with Source.");
            Assert.IsTrue(deletedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in deleted hds Record should match with Source.");
            Assert.IsTrue(deletedRecordInHds.HdsFromTms == deletedRecordInCdc.commit_time, "HdsFromTms in deleted hds Record should match with commit_time in DeletedRecordInCdc.");
            Assert.IsTrue(deletedRecordInHds.HdsToTms == _dthdsDefaultTo, "HdsToTms in deleted hds Record should match with DefaultTodate.");


            //Checking record count in payment.vw_CalendarDates_Current view
            var totalhdsCurrRecord = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalhdsCurrRecord == 0, "No of record should be 0 as the record is deleted from source");

        }
    }

}



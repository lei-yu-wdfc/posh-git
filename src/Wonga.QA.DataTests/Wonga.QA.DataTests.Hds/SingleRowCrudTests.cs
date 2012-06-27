using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
  
    class SingleRowCrudTests
    {
        private bool _cdcStagingAgentJobWasDisabled;
        private bool _hdsAgentJobWasDisabled;
        private static DateTime _dthdsDefaultTo = Convert.ToDateTime("31/12/9999");

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
            DateTime maxdate = Drive.Data.Payments.Db.payment.CalendarDates.All().Select(Drive.Data.Payments.Db.payment.CalendarDates.Date.Max()).ToScalarOrDefault<DateTime>();
            DateTime newAvailabledate = maxdate.AddDays(1);
            return newAvailabledate;
        }
        [Test]

        public void InsertRecordAndConfirmItIsInCDCStagingAndHds()
        {

            Trace.WriteLine("Running InsertRecordAndConfirmItIsInCDCStagingAndHds.");
            
            //Inserting new record Payments.payment.CalendarDates
           // DateTime newdate = nextAvailabledateToInsert();

            var sourceRecord = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                                   IsBankHoliday: 1,
                                                                                   CreatedOn: DateTime.Now);

            // Wait for hds and cdc load job to run
            WaitForAgentJob();
            

            //Check total record count in Cdc 
            var totalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInCdc == 1, "Total Record count in CDC should be 1");


            //Select the inserted record from cdcstaging and check it matches source data
            var cdcRecord  = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(cdcRecord.Date == sourceRecord.Date ,"Date in CDC should match with Source.");
            Assert.IsTrue(cdcRecord.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in CDC should match with Source.");
            Assert.IsTrue(cdcRecord.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in CDC should match with Source.");


            //Check the Total Record count in HDS
            var totalRecordCountInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalRecordCountInHds == 1, "Total Record count in HDS should be 1");

            //Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo 
            var hdsRecord = Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(hdsRecord.Date == sourceRecord.Date,"Date in Hds should match with Source.");
            Assert.IsTrue(hdsRecord.IsBankHoliday == sourceRecord.IsBankHoliday,"IsBankHoliday in Hds should match with Source.");
            Assert.IsTrue(hdsRecord.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds should match with Source.");
            Assert.IsTrue(hdsRecord.HdsFromTms == cdcRecord.commit_time,"HdsFromTms in Hds should match with commit_time in CDC.");
            Assert.IsTrue(hdsRecord.HdsToTms ==  _dthdsDefaultTo,"HdsToTms in Hds should match with DefaultTodate.");
           

            //Checking record count in payment.vw_CalendarDates_Current view
            var totalhdsCurrRecord = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
            Assert.IsTrue(totalhdsCurrRecord == 1, "Total Record count in HDS Current View  should be 1");


            //Checking data in payment.vw_CalendarDates_Current view
            var hdsCurrRecord = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
            Assert.IsTrue(hdsCurrRecord.Date == sourceRecord.Date,"Date in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.IsBankHoliday == sourceRecord.IsBankHoliday,"IsBankHoliday in Hds View should match with Source.");
            Assert.IsTrue(hdsCurrRecord.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in Hds View should match with Source.");

        }

        [Test]
       public void CalendarDatesInsertAndUpdateSameLoadSameLsn()
       {
         
           //insert and update within a single transaction and check cdc contain both inserted and update record and hds contains only updated record
           Trace.WriteLine("Running CalendarDatesInsertAndUpdateSameLoadSameLsn.");

          // insert and update the same record
           using (var transaction = Drive.Data.Payments.Db.BeginTransaction())
           {

               var sourceRecord = transaction.payment.CalendarDates.Insert(Date: nextAvailabledateToInsert(),
                                                                           IsBankHoliday: 1,
                                                                           CreatedOn: DateTime.Now);
               bool IsBankHoliday = false;

               var UpdatedRecord =
                   transaction.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: sourceRecord.CalendarDateId,
                                                                            IsBankHoliday: IsBankHoliday);

               transaction.Commit();
            
           // Wait for hds and cdc load job to run
              WaitForAgentJob();

           //Check total record count in Cdc 
           var totalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
           Assert.IsTrue(totalRecordCountInCdc == 3,"Total Record count in CDC should be 3");


           //Select the inserted record from cdcstaging and check it matches inserted source data
           var insertedRecordInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,
                                                                                        Operation: 2);
           Assert.IsTrue(insertedRecordInCdc.Date == sourceRecord.Date,"Date in CDC should match with Source after insert.");
           Assert.IsTrue(insertedRecordInCdc.IsBankHoliday == sourceRecord.IsBankHoliday, "IsBankHoliday in CDC should match with Source after insert.");
           Assert.IsTrue(insertedRecordInCdc.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in CDC should match with Source after insert.");

            //Select the inserted record from cdcstaging and check it matches with source data after update
            var UpdatedRecordInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId,
                                                                                         Operation: 4);
            Assert.IsTrue(UpdatedRecordInCdc.Date == sourceRecord.Date,"Date in CDC should match with Source after update.");
            Assert.IsTrue(UpdatedRecordInCdc.IsBankHoliday == IsBankHoliday,"IsBankHoliday in CDC should match with Source after update.");
            Assert.IsTrue(UpdatedRecordInCdc.CreatedOn == sourceRecord.CreatedOn,"CreatedOn in CDC should match with Source after update.");


           //Check the Total Record count in HDS
           var totalRecordCountInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
           Assert.IsTrue(totalRecordCountInHds == 1,"Total Record count in CDC should be 1");

           //Checking  only updated record exist in hds with correct hdslsn,hdsfrm and hdsTo 
           var insertedRecordInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
           Assert.IsTrue(insertedRecordInHds.Date == sourceRecord.Date,"Date in Hds should match with Source.");
           Assert.IsTrue(insertedRecordInHds.IsBankHoliday == IsBankHoliday, "IsBankHoliday in Hds should match with Source.");
           Assert.IsTrue(insertedRecordInHds.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in Hds should match with Source.");
           Assert.IsTrue(insertedRecordInHds.HdsFromTms == UpdatedRecordInCdc.commit_time, "HdsFromTms in Hds should match with cdc commit time.");
           Assert.IsTrue(insertedRecordInHds.HdsToTms == _dthdsDefaultTo,"HdsToTms in Hds should match with DefaultTodate.");

          //Checking record count in payment.vw_CalendarDates_Current view
           var totalhdsCurrRecord = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: sourceRecord.CalendarDateId).Count();
           Assert.IsTrue(totalhdsCurrRecord == 1);

           //Checking data in payment.vw_CalendarDates_Current view
           var hdsCurrRecord = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindBy(CalendarDateId: sourceRecord.CalendarDateId);
           Assert.IsTrue(hdsCurrRecord.Date == sourceRecord.Date,"Date in Hds View should match with Source.");
           Assert.IsTrue(hdsCurrRecord.IsBankHoliday == IsBankHoliday, "IsBankHoliday in Hds View should match with Source.");
           Assert.IsTrue(hdsCurrRecord.CreatedOn == sourceRecord.CreatedOn, "CreatedOn in Hds View should match with Source.");

          }
       }
          

    }
}

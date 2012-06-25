using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Hds;


//Steps for the test.
//Step0:  Seleting the max date from payments.payment.CalendarDates and incremented by 1 to insert into payments.payment.CalendarDates
//Step1:  Insert new record
//Step2:  Update IsBankHoliday column
//Step3:  Run CDC Staging load job
//Step4:  Select the inserted record from cdcstaging and store into a variable to compare with hds data
//Step5:  Select the updated  record from cdcstaging and store into a variable to compare with hds data  --the record with _$Operation=4
//Step6:  Check the record count in cdc staging which should be equal to 3 if not through exception
//Step7:  Run HDS load job
//Step8:  Check the total record count in HDS which should be equal to 2 because hds is not capturing the record where _$Operation=3 if not through exception
//Step9:  Checking record exist in hds with correct hdslsn=StartLsn of InsertedRecord in cdcstaging, hdsfrm = commitdate of insertedrecord in cdcstaging and hdsTo =commitdate of updatedRecord in cdcstaging after insert and also number of record should be 1
//Step10: Checking record exist in hds with correct hdslsn=StartLsn of UpdatedRecord in cdcstaging, hdsfrm = commitdate of updatedRecord in cdcstaging and hdsTo =defaultHdsTo after update and also number of record should be 1
//Step11: Checking total records in payment.vw_CalendarDates_Current view
//Step12: Checking payment.vw_CalendarDates_Current view data(updated record information should be access from the view)
//Step13: reset jobs to original state


namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    class CalendarDatesInsertAndUpdateSameLoadDifferentLsn
    {
        
        [Test]
        [AUT(AUT.Uk)]
        [Description("Test CalendarDates data flows from payment database->HDS database using incremental load")]

        public void CalendarDatesInsert()
        {
            Trace.WriteLine("Running Canlenderdates Insert and Update Same Load Different Lsn test.");
            Trace.WriteLine("Name of server connecting to is " + Drive.Data.NameOfServer);


            //
            //declare the variales
            //

            string strhdsDefaultTo = "31/12/9999";
            DateTime dthdsDefaultTo = Convert.ToDateTime(strhdsDefaultTo);

            //
            //Step0: Seleting the max date from payments.payment.CalendarDates and incremented by 1 to insert into payments.payment.CalendarDates
            //

            DateTime maxdate = Drive.Data.Payments.Db.payment.CalendarDates.All().Select(Drive.Data.Payments.Db.payment.CalendarDates.Date.Max()).ToScalarOrDefault<DateTime>();
            Trace.WriteLine("maximum date presnt in payments.payment.CalendarDates is " + maxdate);

            DateTime newdate = maxdate.AddDays(1);


            //
            //Step1:  Inserting new data into Payments.payment.CalendarDates table.
            //

            Trace.WriteLine("Inserting new data and updating existing data into Payments.payment.CalendarDates table.");

            var calDtInsert = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: newdate,
                                                                                 IsBankHoliday: 1,
                                                                                 CreatedOn: DateTime.Now);

            //
            //Step2: updating existing data into Payments.payment.CalendarDates table.
            //

            var calDtUpdate = Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: calDtInsert.CalendarDateId,
                                                                                                   IsBankHoliday: 0);

            //
            //Step3: Start the CDC Staging load job
            //

            Trace.WriteLine("Start the CDC Staging load job and check data once finished.");

            bool cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);

            //
            //Step4: Select the inserted record from cdcstaging and store into a variable to compare with hds data
            //

            var insertrecordInCdc = Do.Until(() => Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn, IsBankHoliday: calDtInsert.IsBankHoliday, Operation: 2));

            //
            //Step5: Select the updated  record from cdcstaging and store into a variable to compare with hds data  --the record with _$Operation=4
            //

            var updaterecordInCdc = Do.Until(() => Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn, Operation: 4));
            

            //
            //Step6:Checking  the Total  record count in cdc staging 
            //

            Trace.WriteLine("Checking  the total  record count in cdc staging.");

            int TotalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date).Count();
            if (TotalRecordCountInCdc != 3)
            {
                throw new Exception(String.Format("Should be 3 record in CDCStaging.payment.CalendarDates."));
            }


            //
            //Step7 : Start the HDS load job 
            //

            Trace.WriteLine("Start the HDS load job and check data once finished.");

            bool hdsLoadAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);


            //
            //Step8 : check the Total record count in HDS
            //

            Trace.WriteLine("Check the total record count in HDS.");

            int ToalrecordCountInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn).Count();
            if (ToalrecordCountInHds != 2)
            {
                throw new Exception(String.Format("Should be 2 record in Hds.payment.CalendarDates."));
            }

            //
            //Step9 :Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after insert and also number of record should be 1
            //

            Trace.WriteLine(String.Format("Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after insert and the record count"));

            int recordCountInHdsafterInsert = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, IsBankHoliday: calDtInsert.IsBankHoliday, CreatedOn: calDtInsert.CreatedOn, HdsLsn: insertrecordInCdc.Start_lsn, HdsFromTms: insertrecordInCdc.commit_time, HdsToTms: updaterecordInCdc.commit_time).Count();
            if (recordCountInHdsafterInsert != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.CalendarDates after insert"));
            }


            //
            //Step10: Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after update and also number of record should be 1
            //

            Trace.WriteLine(String.Format("Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after insert and the record count"));

            int recordCountInHdsafterUpdate = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn, HdsLsn: updaterecordInCdc.Start_lsn, HdsFromTms: updaterecordInCdc.commit_time, HdsToTms: dthdsDefaultTo).Count();
            if (recordCountInHdsafterUpdate != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.CalendarDates after insert"));
            }

            //
            //Step11: Checking total records in payment.vw_CalendarDates_Current view
            //

            Trace.WriteLine(String.Format("Checking total record in  payment.vw_CalendarDates_Current view "));

            var TotalrecordCountInHdsView = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn).Count();
            if (TotalrecordCountInHdsView != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.vw_CalendarDates_Current."));
            }

            //
            //Step12: Checking payment.vw_CalendarDates_Current view data
            //

            Trace.WriteLine(String.Format("Checking total record in  payment.vw_CalendarDates_Current view "));

            var TotalrecordCountInHdsViewAfterUpdate = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, IsBankHoliday: updaterecordInCdc.IsBankHoliday, CreatedOn: calDtInsert.CreatedOn).Count();
            if (TotalrecordCountInHdsViewAfterUpdate != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.vw_CalendarDates_Current."));
            }

            //
            //Step13 : reset jobs to original state
            //

            if (cdcStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }

            if (hdsLoadAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob); }
            
        }

    }
}

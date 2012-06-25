using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Hds;
using System.Threading;

//Steps for the test.

//Step0: Seleting the max date from payments.payment.CalendarDates and incremented by 1 to insert into payments.payment.CalendarDates
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
    class CalendarDatesInsertAndUpdateSameLoadDifferentLsnSameCommittime
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
            Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: calDtInsert.CalendarDateId,
                                                                                                   IsBankHoliday: 1);
            Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: calDtInsert.CalendarDateId,
                                                                                                   IsBankHoliday: 0);
            Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: calDtInsert.CalendarDateId,
                                                                                                   IsBankHoliday: 1);
            Drive.Data.Payments.Db.payment.CalendarDates.UpdateByCalendarDateId(CalendarDateId: calDtInsert.CalendarDateId,
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
            //Step13 : reset jobs to original state
            //

            if (cdcStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }

          

        }

    }
}


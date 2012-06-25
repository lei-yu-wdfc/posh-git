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
//Step2:  Run CDC Staging load job
//Step3:  Select the inserted record from cdcstaging and store into a variable to compare with hds data
//Step4:  Run HDS load job
//Step5:  Delete the inserted row
//Step6:  Run CDC Staging load job
//Step7:  Select the deleted  record from cdcstaging and store into a variable to compare with hds data  
//Step8:  Run CDC Staging load job
//Step9:  Check the record count in cdc staging which should be equal to 2 if not through exception
//Step10: Run HDS load job
//Step11: Check the total record count in HDS which should be equal to 2  if not through exception
//Step12: Checking record exist in hds with correct hdslsn=StartLsn of InsertedRecord in cdcstaging, hdsfrm = commitdate of insertedrecord in cdcstaging and hdsTo =commitdate of deletedRecord in cdcstaging after insert and also number of record should be 1
//Step13: Checking record exist in hds with correct hdslsn=StartLsn of deletedrecord in cdcstaging, hdsfrm = commitdate of deletedRecord in cdcstaging and hdsTo =defaultHdsTo after delete and also number of record should be 1
//Step14: Checking total records in payment.vw_CalendarDates_Current view and number of record should be 0
//Step15: reset jobs to original state

namespace Wonga.QA.DataTests.Hds
{
    class CalendarDatesInsertAndDeleteDifferentLoadDifferentLsn
    {
        [Test]
        [AUT(AUT.Uk)]
        [Description("Test CalendarDates data flows from payment database->HDS database using incremental load")]

        public void CalendarDatesInsert()
        {
            Trace.WriteLine("Running Canlenderdates Insert and Delete Different Load Different Lsn test.");
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

            //----------------------------------- First Load----------------------------------------------
            //
            //Step1:  Inserting new data into Payments.payment.CalendarDates table.
            //

            Trace.WriteLine("Inserting new data into Payments.payment.CalendarDates table.");

            var calDtInsert = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: newdate,
                                                                                 IsBankHoliday: 1,
                                                                                 CreatedOn: DateTime.Now);

            //
            //Step2: Start the 1st CDC Staging load 
            //

            Trace.WriteLine("Start the 1st CDC Staging load job.");

            bool cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);


            //
            //Step3: Select the inserted record from cdcstaging and store into a variable to compare with hds data
            //

            var insertrecordInCdc = Do.Until(() => Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn, IsBankHoliday: calDtInsert.IsBankHoliday, Operation: 2));


            //
            //Step4 : Start the 1st  HDS load 
            //

            Trace.WriteLine("Start the 1st HDS load job.");

            bool hdsLoadAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);


            //----------------------------------- 2nd Load----------------------------------------------

            //
            //Step5: Deleting existing data into Payments.payment.CalendarDates table.
            //

            var calDtDelete = Drive.Data.Payments.Db.payment.CalendarDates.DeleteByCalendarDateId(CalendarDateId: calDtInsert.CalendarDateId);

            //
            //Step6: Start the CDC Staging load job
            //

            Trace.WriteLine("Start the 2nd CDC Staging load job .");
            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);


            //
            //Step7: Select the updated  record from cdcstaging and store into a variable to compare with hds data  --the record with _$Operation=4
            //

            var deleterecordInCdc = Do.Until(() => Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn, Operation: 1));


            //
            //Step8 : Start the HDS load job 
            //

            Trace.WriteLine("Start the 2nd HDS load job.");

            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);

            //-----------------------------------------------Checking the record in CDCStaging and  Hds-----------------------------------------------------------------------

            //
            //Step9:Checking  the Total  record count in cdc staging 
            //

            Trace.WriteLine("Checking  the total  record count in cdc staging.");

            int TotalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date).Count();
            if (TotalRecordCountInCdc != 2)
            {
                throw new Exception(String.Format("Should be 2 record in CDCStaging.payment.CalendarDates."));
            }

            
            //
            //Step10 : check the Total record count in HDS
            //

            Trace.WriteLine("Check the total record count in HDS.");

            int ToalrecordCountInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn).Count();
            if (ToalrecordCountInHds != 2)
            {
                throw new Exception(String.Format("Should be 2 record in Hds.payment.CalendarDates."));
            }

            //
            //Step11 :Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after insert and also number of record should be 1
            //

            Trace.WriteLine(String.Format("Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after insert and the record count"));

            int recordCountInHdsafterInsert = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, IsBankHoliday: calDtInsert.IsBankHoliday, CreatedOn: calDtInsert.CreatedOn, HdsLsn: insertrecordInCdc.Start_lsn, HdsFromTms: insertrecordInCdc.commit_time, HdsToTms: deleterecordInCdc.commit_time).Count();
            if (recordCountInHdsafterInsert != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.CalendarDates after insert"));
            }


            //
            //Step12: Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after delete and HdsDeleteFlag  set 1 and also number of record should be 1
            //

            Trace.WriteLine(String.Format("Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo after delete and the record count"));

            int recordCountInHdsafterDelete = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn, HdsDeleteFlag: 1, HdsLsn: deleterecordInCdc.Start_lsn, HdsFromTms: deleterecordInCdc.commit_time, HdsToTms: dthdsDefaultTo).Count();
            if (recordCountInHdsafterDelete != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.CalendarDates after Delete"));
            }

            //
            //Step13: Checking total records in payment.vw_CalendarDates_Current view
            //

            Trace.WriteLine(String.Format("Checking total record in  payment.vw_CalendarDates_Current view "));

            var TotalrecordCountInHdsView = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: calDtInsert.CalendarDateId, Date: calDtInsert.Date, CreatedOn: calDtInsert.CreatedOn).Count();
            if (TotalrecordCountInHdsView != 0)
            {
                throw new Exception(String.Format("Should be No record in Hds.payment.vw_CalendarDates_Current because the record is deleted from service db."));
            }


            //
            //Step14 : reset jobs to original state
            //

            if (cdcStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }

            if (hdsLoadAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob); }

        }

    }
}

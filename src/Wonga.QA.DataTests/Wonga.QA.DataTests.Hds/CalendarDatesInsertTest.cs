using System;
using System.Diagnostics;
using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo.Agent;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.DataTests.Hds;
using System.Threading;


//Steps for the test.
//Step1:  Insert new record
//Step2:  Run CDC Staging load job
//Step3:  Select the inserted record from cdcstaging and store into a variable to compare with hds data
//Step4:  Check the record count in cdc staging which should be equal to 1 if not through exception
//Step5:  Run HDS load job
//Step6:  Check the total record count in HDS which should be equal to 1
//Step7:  Checking record exist in hds with correct hdsfrm = commitdate of insertedrecord in cdcstaging and hdsTo =defaultHdsTo after insert and also number of record should be 1
//Step8:  checking  data in payment.vw_CalendarDates_Current view
//Step9:  reset jobs to original state



namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    class CalendarDatesInsertTest
    {

        [Test]
        [AUT(AUT.Uk)]
        [Description("Test CalendarDates data flows from payment database->HDS database using incremental load")]

        public void CalendarDatesInsert()
        {
            Trace.WriteLine("Running CalendarDates Insert Test.");
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
            //delete existing record (payments.payment.CalendarDates ,UK_CDCStaging.payment.CalendarDates,UK_WongaHDS.payment.CalendarDates,payments.cdc.V00001_Payment_CalendarDates_CT)
            //

            //Trace.WriteLine("Clearing the CalendarDates tablesdata.");
            //Drive.Data.Payments.Db.payment.CalendarDates.DeleteAll();
            //Drive.Data.Cdc.Db.payment.CalendarDates.DeleteAll();
            //Drive.Data.Hds.Db.payment.CalendarDates.DeleteAll();
            //int milliseconds = 4000;
            //Thread.Sleep(milliseconds);
            //Do.Until(() =>Drive.Data.Payments.Db.cdc.V00001_Payment_CalendarDates_CT.DeleteAll());

            //
            //Step1 : Inserting new record Payments.payment.CalendarDates
            //

            Trace.WriteLine("Inserting new data into Payments.payment.CalendarDates table.");

            var calDt = Drive.Data.Payments.Db.payment.CalendarDates.Insert(Date: newdate,
                                                                            IsBankHoliday: 1,
                                                                            CreatedOn: DateTime.Now);

            //
            // Step2: Start the CDC Staging load job 
            //

            Trace.WriteLine(" Start the CDC Staging load job and check data once finished.");

            bool cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.CdcStagingAgentJob);

            //Step3:Select the inserted record from cdcstaging and store into a variable to compare with hds data

            var recordInCdc = Do.Until(() => Drive.Data.Cdc.Db.Payment.CalendarDates.FindBy(CalendarDateId: calDt.CalendarDateId, Date: calDt.Date, IsBankHoliday: calDt.IsBankHoliday, CreatedOn: calDt.CreatedOn));
            

            //
            //Step4: Check the Total record count in cdc staging 
            //

            Trace.WriteLine("Check the Total record count in cdc staging .");

            var TotalRecordCountInCdc = Drive.Data.Cdc.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDt.CalendarDateId,Date: calDt.Date, IsBankHoliday: calDt.IsBankHoliday, CreatedOn: calDt.CreatedOn).Count();
            if (TotalRecordCountInCdc != 1)
            {
                throw new Exception(String.Format("Should be 1 record in CDCStaging.payment.CalendarDates."));
            }


            //
            // Step5: Start the HDS load job 
            //

            Trace.WriteLine("Start the HDS load job and check data once finished.");

            bool hdsLoadAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);
            HdsUtilities.WaitUntilJobComplete(HdsUtilities.HdsLoadAgentJob);
            
          
            //
            //Step6 : Check the Total Record count in HDS
            //

            Trace.WriteLine("Check the Total Record count in HDS.");

            var TotalRecordCountInHds = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDt.CalendarDateId,Date: calDt.Date, IsBankHoliday: calDt.IsBankHoliday, CreatedOn: calDt.CreatedOn).Count();
            if (TotalRecordCountInHds != 1)
              {
                throw new Exception(String.Format("Should be 1 Record in Hds.payment.CalendarDates."));
             }

            //
            //Step7: Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo and also number of record should be 1
            //

            Trace.WriteLine(String.Format("Checking record exist in hds with correct hdslsn,hdsfrm and hdsTo and the record count"));

            var recordCountInHdsAfterInsert = Drive.Data.Hds.Db.Payment.CalendarDates.FindAllBy(CalendarDateId: calDt.CalendarDateId,Date: calDt.Date, IsBankHoliday: calDt.IsBankHoliday, CreatedOn: calDt.CreatedOn, HdsFromTms: recordInCdc.commit_time, HdsToTms: dthdsDefaultTo).Count();
            if (recordCountInHdsAfterInsert != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.CalendarDates."));
            }

            //
            //Step8: Checking data in payment.vw_CalendarDates_Current view
            //

            Trace.WriteLine(String.Format("Checking No of record in  payment.vw_CalendarDates_Current view "));

            var recordCountInHdsView = Drive.Data.Hds.Db.Payment.vw_CalendarDates_Current.FindAllBy(CalendarDateId: calDt.CalendarDateId,Date: calDt.Date, IsBankHoliday: calDt.IsBankHoliday, CreatedOn: calDt.CreatedOn).Count();
            if (recordCountInHdsView != 1)
            {
                throw new Exception(String.Format("Should be 1 record in Hds.payment.vw_CalendarDates_Current."));
            }

            //
            //Step9: reset jobs to original state
            //

            if (cdcStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }

            if (hdsLoadAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob); }
            
        }

        
    }
}



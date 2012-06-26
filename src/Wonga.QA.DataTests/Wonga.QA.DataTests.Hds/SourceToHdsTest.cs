using System;
using System.Diagnostics;
using MbUnit.Framework;
using Microsoft.SqlServer.Management.Smo.Agent;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data;


namespace Wonga.QA.DataTests.Hds
{
    [TestFixture]
    public class SourceToHdsTest
    {
        [Test]
        [AUT(AUT.Uk)]
        [Description("Test data flows from source database->CDCStaging->HDS database")]
        public void EndToEnd()
        {
            Trace.WriteLine("Running EndToEnd Test.");
            Trace.WriteLine("Name of server connecting to is " + Drive.Data.NameOfServer);

            var appl = Drive.Data.Payments.Db.payment.Applications.Insert(ExternalId: Get.GetId(),
                                                                            AccountId: Get.GetId(), ProductId: 1,
                                                                            Currency: 710,
                                                                            BankAccountGuid: Get.GetId(),
                                                                            PaymentCardGuid: Get.GetId(),
                                                                            ApplicationDate: DateTime.Now,
                                                                            CreatedOn: DateTime.Now);
            
            //
            // Start the CDC Staging load job and check data once finished
            //
            bool cdcStagingAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);

            HdsUtilities.CheckRecordInCdcStaging(appl.ExternalId, HdsUtilities.PaymentEntity.Application.ToString());

            //
            // Start the HDS load job and check data once finished
            //
            bool hdsLoadAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);

            HdsUtilities.CheckRecordInHds(appl.ExternalId, HdsUtilities.PaymentEntity.Application.ToString());

            // reset jobs to original state
            if (cdcStagingAgentJobWasDisabled)
            { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }

            if (hdsLoadAgentJobWasDisabled) 
            { HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob); }
        }

        [Test]
        [AUT(AUT.Uk)]
        [Description("Test data reconciles after HDS initial load and after incremental load")]
        public void ReconcileHds()
        {
            Trace.WriteLine("Running HDS Reconciliation Test.");

            bool cdcStagingAgentJobWasDisabled = HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob);
            bool hdsLoadAgentJobWasDisabled = HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob);

            //// Insert data into source
            // TODO: Start [insert data] thread            

            // Run HDS initial load
            bool hdsInitialLoadAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsInitialLoadAgentJob);

            // Not on a schedule so execute it
            if (SQLServerAgentJobs.CheckJobStatus(HdsUtilities.HdsInitialLoadAgentJob) == JobExecutionStatus.Idle) { SQLServerAgentJobs.Execute(HdsUtilities.HdsInitialLoadAgentJob); }

            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsInitialLoadAgentJob);

            // Run CDC Staging load
            HdsUtilities.EnableJob(HdsUtilities.CdcStagingAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.CdcStagingAgentJob);

            // Run HDS load
            HdsUtilities.EnableJob(HdsUtilities.HdsLoadAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsLoadAgentJob);

            // Run HDS Reconciliation job
            bool hdsReconcileAgentJobWasDisabled = HdsUtilities.EnableJob(HdsUtilities.HdsReconcileAgentJob);

            HdsUtilities.WaitUntilJobRun(HdsUtilities.HdsReconcileAgentJob);

            // Finish inserting data into source
            // TODO: Stop [insert data] thread

            // reset jobs to original state
            if (hdsInitialLoadAgentJobWasDisabled) { HdsUtilities.DisableJob(HdsUtilities.HdsInitialLoadAgentJob); }
            if (cdcStagingAgentJobWasDisabled) { HdsUtilities.DisableJob(HdsUtilities.CdcStagingAgentJob); }
            if (hdsLoadAgentJobWasDisabled) { HdsUtilities.DisableJob(HdsUtilities.HdsLoadAgentJob); }
            if (hdsReconcileAgentJobWasDisabled) { HdsUtilities.DisableJob(HdsUtilities.HdsReconcileAgentJob); }
        }
    }
}


//var appl2 = appl;

//SimpleRecord appl3 = appl2;

//Trace.WriteLine(appl3.ElementAt(0).Value);
//IDictionary<string,object> colNames = appl3;

//foreach (var colName in colNames)
//{
//    Trace.WriteLine(colName.ToString());
//}

//string testRes = (appl == appl2).ToString();
//Trace.WriteLine(testRes);
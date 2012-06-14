using System;
using System.Diagnostics;
using System.Dynamic;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Data;
using Microsoft.SqlServer.Management.Smo.Agent;
using Simple.Data;

namespace Wonga.QA.Tests.Hds
{
    [TestFixture]
    public class PrototypeTest
    {

        [Test]
        [AUT(AUT.Uk)]
        [JIRA("DI-689")]
        [Description("Used as a prototype test for CDC/HDS database testing")]
        public void FirstRealProtypeTest_DontCallMethodsThisName()
        {
            Trace.WriteLine("Testing");
            bool paymentsCaptureEnabled = true;
            bool hdsPaymentsEnabled = true;

            Trace.WriteLine("Check jobs enabled");
            if (SQLServerAgentJobs.CheckIsJobEnabled("UK_CDCStagingLoadPayments") == false)
            {
                paymentsCaptureEnabled = false;
                SQLServerAgentJobs.EnableJob("UK_CDCStagingLoadPayments");
            }
            if (SQLServerAgentJobs.CheckIsJobEnabled("DataInsight - UK_WongaHDSpaymentsLoad") == false)
            {
                hdsPaymentsEnabled = false;
                SQLServerAgentJobs.EnableJob("DataInsight - UK_WongaHDSpaymentsLoad");
            }

            Trace.WriteLine("Name of server connecting to is " + Drive.Data.NameOfServer);

            var appl = Drive.Data.Payments.Db.payment.Applications.Insert(ExternalId: Get.GetId(),
                   AccountId: Get.GetId(), ProductId: 1, Currency: 710, BankAccountGuid: Get.GetId(),
                   PaymentCardGuid: Get.GetId(), ApplicationDate: DateTime.Now,
                  CreatedOn: DateTime.Now
            );

            // Find the last run time of the base table to CDC job
            DateTime? lastRunTime = SQLServerAgentJobs.GetJobLastRunDateTime("UK_CDCStagingLoadPayments");

            SQLServerAgentJobs.WaitUntilJobRun("UK_CDCStagingLoadPayments", lastRunTime ?? DateTime.Now);

            // Check its added in to CDC
            var recordInCDC = Do.Until(() => Drive.Data.Cdc.Db.Payment.Applications.FindBy(ExternalId: appl.ExternalId));

            // Check its added in to HDS
            var recordInHDS = Do.Until(() => Drive.Data.Hds.Db.payment.Applications.FindBy(ExternalId: recordInCDC.ExternalId));

            if (hdsPaymentsEnabled == false)
            {
                SQLServerAgentJobs.DisableJob("DataInsight - UK_WongaHDSpaymentsLoad");                
            }

            if (paymentsCaptureEnabled == false)
            {
                SQLServerAgentJobs.DisableJob("cdc.Payments_capture");
            }
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
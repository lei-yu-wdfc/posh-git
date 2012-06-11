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
using Wonga.QA.Framework.SMO;
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
        public void SMOTest_CheckJobStatus()
        {
            Microsoft.SqlServer.Management.Smo.Agent.JobExecutionStatus jobStatus = Jobs.CheckJobStatus("CDCStagingLoad");

            Trace.WriteLine("Job status is " + jobStatus.ToString());

            bool jobEnabled = Jobs.CheckIsJobEnabled("CDCStagingLoad");

            Trace.WriteLine("Job is " + (jobEnabled == true ? "enabled" : "disabled"));
        }


        [Test]
        [AUT(AUT.Uk)]
        [JIRA("DI-689")]
        [Description("Used as a prototype test for HDS database testing")]
        public void FirstRealProtypeTest_DontCallMethodsThisName()
        {
            Trace.WriteLine("Testing");

            Trace.WriteLine("Name of server connecting to is " + Drive.Data.NameOfServer);

            var appl = Drive.Data.Payments.Db.payment.Applications.Insert(ExternalId: Get.GetId(),
                   AccountId: Get.GetId(), ProductId: 1, Currency: 710, BankAccountGuid: Get.GetId(),
                   PaymentCardGuid: Get.GetId(), ApplicationDate: DateTime.Now,
                  CreatedOn: DateTime.Now
            );

            var appl2 = appl;

            SimpleRecord appl3 = appl2;

            Trace.WriteLine(appl3.ElementAt(0).Value);
            IDictionary<string,object> colNames = appl3;

            foreach (var colName in colNames)
            {
                Trace.WriteLine(colName.ToString());
            }

            string testRes = (appl == appl2).ToString();
            Trace.WriteLine(testRes);
            // Check its added
            var recordInPayments = Do.Until(() => Drive.Data.Payments.Db.payment.Applications.FindBy(ApplicationId: appl.ApplicationId));

            //DateTime? me = Jobs.GetJobLastRunDateTime(Drive.Data.NameOfServer, "cdc.BI_Capture");

        }
    }
}


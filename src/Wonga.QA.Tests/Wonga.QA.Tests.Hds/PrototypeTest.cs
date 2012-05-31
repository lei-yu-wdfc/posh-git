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

namespace Wonga.QA.Tests.Hds
{
    [TestFixture]
    public class PrototypeTest
    {

        [Test]
        public void FirstTest()
        {

            // the best way to access a table looking for a row or multiple rows where ApplicationId = 1
            // var test2 = Do.Until(() => Drive.Data.Payments.Db.payment.Applications.FindBy(ApplicationId: 1));

            // call a stored procedure waiting 100 ms for it to run.
            // var test3 = Do.With.Timeout(100).Until(() => Drive.Data.Payments.Db.usp_HDSLoadPaymentArrears());

            // Calling a stored procedure where the results are returned in test4. 
            // the results are held in a dynamic variable test4
            //var test4 = Do.Until(() => Drive.Data.Payments.Db.payment.rdTestPayment());

            //// The return value
            //Console.WriteLine(test4.ReturnValue.ToString());

            //// Loop around each row
            //foreach (var v in test4)
            //{
            //    // Test1 is the name of a column
            //    Console.WriteLine(v.Test1.ToString());
            //}

            //// Test against HDS
            var test1 = Do.Until(() => Drive.Data.Hds.Db.payment.Applications.FindBy(ApplicationId: 1));

            Console.WriteLine("Test");
        }
  
        
        [Test, AUT(AUT.Uk), JIRA("Unknown", "This is a prototype test")]
        public void FirstRealProtypeTest_DontCallMethodsThisName()
        {
            Trace.WriteLine("Testing");

            Trace.WriteLine(Drive.Data.NameOfServer);
            
            var appl = Drive.Data.Payments.Db.payment.Applications.Insert(ExternalId: Get.GetId(),
                   AccountId: Get.GetId(), ProductId: 1, Currency: 710, BankAccountGuid: Get.GetId(),
                   PaymentCardGuid: Get.GetId(), ApplicationDate: DateTime.Now,
                  CreatedOn: DateTime.Now
            );

            // Check its added
            var recordInPayments = Do.Until(() => Drive.Data.Payments.Db.payment.Applications.FindBy(ApplicationId: appl.ApplicationId));

            DateTime? me = Jobs.GetJobLastRunDateTime(Drive.Data.NameOfServer, "cdc.BI_Capture");

        }
    }
}


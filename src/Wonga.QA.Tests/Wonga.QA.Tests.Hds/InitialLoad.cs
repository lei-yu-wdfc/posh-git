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
    public class InitialLoad
    {
        [Test]
        [AUT(AUT.Uk)]
        public void InitalLoadApplications()
        {
            // To be continued

            //for (int i = 0; i < 200; i++)
            //{
            //    var appl = Drive.Data.Payments.Db.payment.Applications.Insert(ExternalId: Get.GetId(),
            //                   AccountId: Get.GetId(), ProductId: 1, Currency: 710, BankAccountGuid: Get.GetId(),
            //                   PaymentCardGuid: Get.GetId(), ApplicationDate: DateTime.Now,
            //                  CreatedOn: DateTime.Now);
                
            //}
        }
    }
}

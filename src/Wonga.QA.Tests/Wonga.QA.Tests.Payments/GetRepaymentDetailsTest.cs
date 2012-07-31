using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    class GetRepaymentDetailsTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-1112"), Pending("Commented out until query is added to the API"), Owner(Owner.DenisRyzhkov)]
        public void GetRepaymentDetailsHandler()
        {
            //Guid aplId = Guid.Parse("AC36B516-6096-41D9-BD13-BC2661EC5A5B");

            //var parm =  Drive.Api.Queries.Post(new GetRepaymentDetailsQuery { ApplicationId = aplId });

            //var repDate = parm.Values["RepaymentDate"];ClosedApplicationBalance
            //var repAmount = (parm.Values["RepaymentAmount"]);

            //List<string > repDateArray = new List<string>();
            //foreach (var item in repDate)
            //{
            //    repDateArray.Add(item);
            //}

            //List<string> repAmountArray = new List<string>();
            //foreach (var it in repAmount)
            //{
            //    repAmountArray.Add(it);
            //}

            //Assert.IsNotNull(parm);
            //Assert.AreEqual("2012-05-27", repDateArray[0]);
            //Assert.AreEqual("2012-06-27", repDateArray[1]);
            //Assert.AreEqual("2012-07-27", repDateArray[2]);

            //Assert.AreEqual("50", repAmountArray[0]);
            //Assert.AreEqual("50", repAmountArray[1]);
            //Assert.AreEqual("50", repAmountArray[2]);

            //Assert.AreEqual(10.17m, Decimal.Parse(parm.Values["TransactionFee"].Single()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
   
    public class GetAvailabelCreditTest
    {
        [Test, AUT(AUT.Uk), JIRA("UK-92"), Owner(Owner.DenisRyzhkov), Pending("Commented out until query is added to the API")]
        public void AvailabelCreditHandler()
        {
            //const decimal available = 1000.0m;

            //ApiResponse parm = Drive.Api.Queries.Post(new GetAvailabelCreditQuery
            //                                              {
            //                                                  AccountId = "5C09C656-721A-4CDD-8EF8-410CC5343DE3",
            //                                             });
            
            //Assert.IsNotNull(parm);
            //Assert.AreEqual(available, decimal.Parse(parm.Values["AvailableCredit"].Single()));
        }
    }
}

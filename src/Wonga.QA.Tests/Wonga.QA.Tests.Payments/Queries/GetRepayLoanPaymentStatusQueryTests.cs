using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.Self)]
    public class GetRepayLoanPaymentStatusQueryTests
    {
        [Test, AUT(AUT.Uk)]
        public void TestStaticResponse()
        {
            var appId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            //Call Api Query
            var response = Drive.Api.Queries.Post(new GetRepayLoanPaymentStatusUkQuery{ ApplicationId = appId, RepaymentRequestId = requestId });

            Assert.AreEqual(appId.ToString(), response.Values["ApplicationId"].Single(), "ApplicationId incorrect");
        }
    }
}

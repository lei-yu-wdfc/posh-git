using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Tests.Payments.Helpers;
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

        [Test, AUT(AUT.Uk)]
        public void PaymentTaken()
        {
            var setup = new RepayLoanFunctions();
            var appId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            setup.RepayEarlyOnLoanStartDate(appId, paymentCardId, Guid.NewGuid(), Guid.NewGuid(), 400.00M);


            Drive.Api.Commands.Post(new RepayLoanViaCardCommand {ApplicationId = appId, PaymentRequestId = requestId, Amount = 5.50M, PaymentCardId = paymentCardId, PaymentCardCv2 = 123});
            
            // Check Repayment Exists in DB
            Do.With.Interval(1).Until(() => Drive.Data.Payments.Db.RepaymentRequestDetails.FindByExternalId(requestId));

            //Call Api Query
            var response2 = Drive.Api.Queries.Post(new GetRepayLoanPaymentStatusUkQuery { ApplicationId = appId, RepaymentRequestId = requestId });

            Assert.AreEqual(appId.ToString(), response2.Values["ApplicationId"].Single(), "ApplicationId incorrect");
        }
    }
}

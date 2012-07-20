using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
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
        [Test, AUT(AUT.Uk), Owner(Owner.CharlieBarker)]
        public void PaymentTaken()
        {
            var setup = new RepayLoanFunctions();
            var appId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            const decimal paymentAmount = 105.50M;

            setup.RepayEarlyOnLoanStartDate(appId, paymentCardId, Guid.NewGuid(), Guid.NewGuid(), 400.00M);

            var response = Drive.Api.Commands.Post(new RepayLoanViaCardCommand { ApplicationId = appId, PaymentRequestId = requestId, Amount = paymentAmount, PaymentCardId = paymentCardId, PaymentCardCv2 = 123 });
            Assert.AreEqual(0, response.GetErrors().Count(), "RepayLoanViaCardCommand was rejected by API");

            // Check Repayment Exists in DB
            Do.With.Interval(1).Until(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindByExternalId(requestId));
            
            //Call Api Query
            ApiResponse response2=null;
            Do.Until(() => (response2=Drive.Api.Queries.Post(new GetRepayLoanPaymentStatusUkQuery { ApplicationId = appId, RepaymentRequestId = requestId })).Values["PaymentStatus"].Single() != "Pending");

            Assert.AreEqual(appId.ToString(), response2.Values["ApplicationId"].Single(), "ApplicationId incorrect");
            Assert.AreEqual("PaymentTaken", response2.Values["PaymentStatus"].Single(), "PaymentStatus incorrect");
        }

        [Test, AUT(AUT.Uk), Owner(Owner.CharlieBarker)]
        public void PartPaymentTaken()
        {
            var setup = new RepayLoanFunctions();
            var appId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            const decimal paymentAmount = 5.50M;

            setup.RepayEarlyOnLoanStartDate(appId, paymentCardId, Guid.NewGuid(), Guid.NewGuid(), 400.00M);

            var response = Drive.Api.Commands.Post(new RepayLoanViaCardCommand { ApplicationId = appId, PaymentRequestId = requestId, Amount = paymentAmount, PaymentCardId = paymentCardId, PaymentCardCv2 = 123 });
            Assert.AreEqual(0, response.GetErrors().Count(), "RepayLoanViaCardCommand was rejected by API");

            // Check Repayment Exists in DB
            Do.With.Interval(1).Until(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindByExternalId(requestId));

            //Call Api Query
            ApiResponse response2 = null;
            Do.Until(() => (response2 = Drive.Api.Queries.Post(new GetRepayLoanPaymentStatusUkQuery { ApplicationId = appId, RepaymentRequestId = requestId })).Values["PaymentStatus"].Single() != "Pending");

            Assert.AreEqual(appId.ToString(), response2.Values["ApplicationId"].Single(), "ApplicationId incorrect");
            Assert.AreEqual("PartPaymentTaken", response2.Values["PaymentStatus"].Single(), "PaymentStatus incorrect");
        }

        [Test, AUT(AUT.Uk), Owner(Owner.CharlieBarker)]
        public void CustomerOwesLessThanMinimumAmount()
        {
            var setup = new RepayLoanFunctions();
            var appId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            const decimal paymentAmount = 104.50M;
            const string sliderMinAmount = "1.00";
            
            setup.RepayEarlyOnLoanStartDate(appId, paymentCardId, Guid.NewGuid(), Guid.NewGuid(), 400.00M);

            var response = Drive.Api.Commands.Post(new RepayLoanViaCardCommand { ApplicationId = appId, PaymentRequestId = requestId, Amount = paymentAmount, PaymentCardId = paymentCardId, PaymentCardCv2 = 123 });
            Assert.AreEqual(0, response.GetErrors().Count(), "RepayLoanViaCardCommand was rejected by API");

            // Check Repayment Exists in DB
            Do.With.Interval(1).Until(() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindByExternalId(requestId));

            //Call Api Query
            ApiResponse response2 = null;
            Do.Until(() => (response2 = Drive.Api.Queries.Post(new GetRepayLoanPaymentStatusUkQuery { ApplicationId = appId, RepaymentRequestId = requestId })).Values["PaymentStatus"].Single() != "Pending");

            Assert.AreEqual(appId.ToString(), response2.Values["ApplicationId"].Single(), "ApplicationId incorrect");
            Assert.AreEqual("PartPaymentTaken", response2.Values["PaymentStatus"].Single(), "PaymentStatus incorrect");

            //Call Api Quote Query
            var response3 = Drive.Api.Queries.Post(new GetRepayLoanQuoteUkQuery {ApplicationId = appId});

            Assert.AreEqual(sliderMinAmount, response3.Values["SliderMinAmount"].SingleOrDefault(), "Slider Minimum Amount is incorrect");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Payments
{
    /// <summary>
    /// Tests to check if PromoCodeGuids are stored in the Payment.Applications DB when a promo code is supplied
    /// and a TransmissionFeeDiscount is applied to FixedTermLoanApplications 
    /// </summary>
    [TestFixture, Parallelizable(TestScope.Self)]
    public class PromoCodeUsagePaymentsTests
    {
        
        [Test, AUT(AUT.Uk), Owner(Owner.MohammadRashid), Pending("Pending on pushing issue8 branch of Payments/Marketing/Ops to master")]
        public void CreateFixedTermLoanApplication_PromoCodeIdIsSaved()
        {
            var promoCodeId = Guid.NewGuid();
            Customer cust = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithPromoCode(promoCodeId).Build();

            var app = Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id));

            Assert.AreEqual(app.PromoCodeGuid, promoCodeId);
        }

        [Test, AUT(AUT.Uk), Owner(Owner.MohammadRashid), Pending("Pending on pushing issue8 branch of Payments/Marketing/Ops to master")]
        public void CreateFixedTermLoanApplication_TransmissionFeeDiscountIsApplied()
        {
            decimal transmissionFee = Drive.Data.Payments.Db.Products.FindByProductId(1).TransmissionFee;
            const decimal transmissionFeeDiscount = 50;
            Customer cust = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithPromoCode(Guid.NewGuid()).WithTransmissionFeeDiscount(transmissionFeeDiscount).Build();

            var app = Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id));

            //check when application is added to the FixedTermLoanApplications DB
            var appDetails = Do.Until(() => Drive.Data.Payments.Db.FixedTermLoanApplications.FindByApplicationId(app.ApplicationId));

            Assert.AreEqual(appDetails.TransmissionFee, transmissionFee/2);
        }
    }
}

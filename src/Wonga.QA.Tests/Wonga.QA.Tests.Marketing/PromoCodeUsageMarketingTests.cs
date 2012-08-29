﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Core;
﻿using Wonga.QA.Framework.Msmq;
﻿using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
﻿using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Marketing
{
    /// <summary>
    /// Tests to check if used promo codes are de-activated in the PromoCode repository and
    /// a new record for promo code usage is added in the PromoCodeUsage repository
    /// </summary>
    [TestFixture, Parallelizable(TestScope.Self)]
    public class PromoCodeUsageMarketingTests
    {
        private dynamic promoCampaign;
        private dynamic promoCode;
        private Guid promoCodeId;

        private void Setup()
        {
            promoCampaign = new ExpandoObject();
            promoCode = new ExpandoObject();
            promoCodeId = Guid.NewGuid();

            //set up promo code campaign
            promoCampaign.ExternalId = Guid.NewGuid();
            promoCampaign.CampaignName = Get.RandomString(20);
            promoCampaign.StartDate = new System.DateTime(1996, 6, 3, 22, 15, 0);
            promoCampaign.InterestDiscount = 1;
            promoCampaign.TransferFeeDiscount = 1;
            promoCampaign.Reward = 1;
            promoCampaign.IsActive = true;

            //set up promo code data
            promoCode.ExternalId = Guid.NewGuid();
            promoCode.Code = Get.RandomString(10);
            promoCode.IsActive = true;
            promoCode.CreatedOn = DateTime.UtcNow;
            promoCode.PromoCodeGuid = promoCodeId;

            //insert promo campaign
            Drive.Data.Marketing.Db.PromoCampaigns.Insert(promoCampaign);

            //find promo campaign ==> use ID for promoCode
            var promoCampaigninDB = Drive.Data.Marketing.Db.PromoCampaigns.FindByExternalId(promoCampaign.ExternalId);
            promoCode.PromoCampaignId = promoCampaigninDB.PromoCampaignId;

            //insert promo code
            Drive.Data.Marketing.Db.PromoCodes.Insert(promoCode);
        }

        [Test, AUT(AUT.Uk), Owner(Owner.MohammadRashid)]
        public void PromoCodeIsMarkedAsUsed_WhenFixedTermLoanApplicationIsStartedWithPromoCode()
        {
            Setup();

            //build application using the promo code
            Customer cust = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(cust)
                .WithExpectedDecision(ApplicationDecisionStatus.Accepted)
                .WithOutSigning()
                .WithPromoCode(promoCodeId)
                .WithTransmissionFeeDiscount(5).Build();

            //check application has been created
            Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id));

            //sign application
            Drive.Api.Commands.Post(new SignApplicationCommand { AccountId = cust.Id, ApplicationId = application.Id });

            //check if promocode has been de-activated and added to usage repository
            Do.Until(() => Drive.Data.Marketing.Db.PromoCodesUsage.FindByPromoCodeGuid(promoCodeId));

            bool isActive = Drive.Data.Marketing.Db.PromoCodes.FindByPromoCodeGuid(promoCodeId).IsActive;
            Assert.IsFalse(isActive);
            Console.WriteLine("PromoCode " + promoCodeId.ToString() + " IsActive :" + isActive);
        }

        [Test, AUT(AUT.Uk), Owner(Owner.MohammadRashid), Pending("Pending API command in Payments to Cancel Application / other alternative")]
        public void PromoCodeIsUnused_WhenFixedTermLoanApplicationIsCancelled()
        {
            Setup();

            Customer cust = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(cust)
                .WithOutSigning()
                .WithPromoCode(promoCodeId)
                .WithTransmissionFeeDiscount(5).Build();

            //check application has been created
            Do.Until(() => Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id));

            Drive.Msmq.Payments.Send(new CancelLoanApplicationMessage
                                        {
                                            ApplicationId = application.Id, 
                                        });

            //check if promocode is still active and not added to usage repository
            Do.Until(() => Drive.Data.Marketing.Db.PromoCodes.FindByPromoCodeGuidAndIsActive(promoCodeId, true));
            Assert.IsNull(Drive.Data.Marketing.Db.PromoCodesUsage.FindByPromoCodeGuid(promoCodeId));
        }
    }
}

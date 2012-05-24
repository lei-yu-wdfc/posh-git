using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Prepaid
{
    [TestFixture, Parallelizable(TestScope.All)]
    class PrepaidCardRewardsTests
    {
        [Test, AUT(AUT.Uk), JIRA("PP-55")]
        public void GiveRewardForCustomerWithStandardCardTest()
        {
            Customer eligibleCustomer = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            CustomerOperations.CreateMarketingEligibility(eligibleCustomer.Id, true);
            CustomerOperations.CreatePrepaidCardForCustomer(eligibleCustomer.Id, false);

            Application application = ApplicationBuilder.New(eligibleCustomer).Build();

            var applicationEntity = Drive.Data.Payments.Db.Applications;
            var appRecord =
                Do.Until(
                    () =>
                    applicationEntity.Find(applicationEntity.ExternalId == application.Id));

            var cardHolderDetailsEntity = Drive.Data.PrepaidCard.Db.CardHolderDetails;
            var cardRecord =
                Do.Until(
                    () =>
                    cardHolderDetailsEntity.Find(cardHolderDetailsEntity.CustomerExternalId == eligibleCustomer.Id));

            var cardDetailsEntity = Drive.Data.PrepaidCard.Db.CardDetails;
            Do.Until(() => cardDetailsEntity.UpdateByCardHolderExternalId(
                     CardHolderExternalId: cardRecord.ExternalId,
                     ExternalId: appRecord.PaymentCardGuid));
            
            application.RepayOnDueDate();

            var cashBackEntity = Drive.Data.PrepaidCard.Db.CashBack;
            var addedRecord = Do.Until(() => cashBackEntity.Find((cashBackEntity.CustomerId == eligibleCustomer.Id)));

            Assert.IsNotNull(addedRecord);
            Assert.IsNotEmpty(addedRecord);
        }
    }
}
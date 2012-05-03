using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Prepaid
{
    class PrepaidCardTest
    {
        private Customer _eligibleCustomer = null;
        private Customer _nonEligibleCustomer = null;
        private Customer _nonEligibleCustomerInArrears = null;
        private Customer _customerWithWromgEmail = null;

        private static readonly dynamic _prepaidCardDb = Drive.Data.PrepaidCard.Db;

        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().WithEmailAddress(Get.GetEmailLessFiftyChars()).Build();
            _nonEligibleCustomer = CustomerBuilder.New().WithEmailAddress(Get.GetEmailLessFiftyChars()).Build();
            _nonEligibleCustomerInArrears = CustomerBuilder.New().WithEmailAddress(Get.GetEmailLessFiftyChars()).Build();
            _customerWithWromgEmail = CustomerBuilder.New().Build();

            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id, true);
            CustomerOperations.CreateMarketingEligibility(_nonEligibleCustomerInArrears.Id, false);
            CustomerOperations.CreateMarketingEligibility(_customerWithWromgEmail.Id, true);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-8")]
        public void CustomerShouldapplyForPrepaidCard()
        {

            Drive.Api.Commands.Post(new SignupCustomerForStandardCardCommand
            {
                CustomerExternalId = _eligibleCustomer.Id
            });

            CheckOnAddingRecords(_eligibleCustomer.Id);


            Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new SignupCustomerForStandardCardCommand
            {
                CustomerExternalId = _nonEligibleCustomer.Id,
            }));

            Assert.Throws<ValidatorException>(() => Drive.Api.Commands.Post(new SignupCustomerForStandardCardCommand
            {
                CustomerExternalId = _nonEligibleCustomerInArrears.Id
            }));

            Drive.Api.Commands.Post(new SignupCustomerForStandardCardCommand
            {
                CustomerExternalId = _customerWithWromgEmail.Id
            });

            Assert.Throws<Exception>(() => CheckOnAddingRecords(_customerWithWromgEmail.Id));
        }



        private void CheckOnAddingRecords(Guid customerId)
        {
            var cardHolderId = Do.Until(() => _prepaidCardDb.CardHolderDetails.FindByCustomerExternalId(CustomerExternalId: customerId));
            Do.Until(() => _prepaidCardDb.CardDetails.FindByCardHolderExternalId(CardHolderExternalId: cardHolderId.ExternalId));
        }
    }
}

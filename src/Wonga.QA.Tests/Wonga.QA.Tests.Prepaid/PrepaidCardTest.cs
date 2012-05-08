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

        private SignupCustomerForStandardCardCommand _validRequest = null;
        private SignupCustomerForStandardCardCommand _requestWithInvalidCustomerId = null;
        private SignupCustomerForStandardCardCommand _requestWithCustomerInArrears = null;
        private SignupCustomerForStandardCardCommand _requestWithInvalidCustomerEmail = null;

        private static readonly int VALID_ACCOUNT_NUMBER_LENGTH = 14;
        private static readonly int VALID_SERIAL_NUMBER_LENGTH = 10;


        private static readonly dynamic _prepaidCardDb = Drive.Data.PrepaidCard.Db;

        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _nonEligibleCustomer = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _nonEligibleCustomerInArrears = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _customerWithWromgEmail = CustomerBuilder.New().Build();

            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id, true);
            CustomerOperations.CreateMarketingEligibility(_nonEligibleCustomerInArrears.Id, false);
            CustomerOperations.CreateMarketingEligibility(_customerWithWromgEmail.Id, true);

            _validRequest = new SignupCustomerForStandardCardCommand();
            _validRequest.CustomerExternalId = _eligibleCustomer.Id;

            _requestWithInvalidCustomerId = new SignupCustomerForStandardCardCommand();
            _requestWithInvalidCustomerId.CustomerExternalId = _nonEligibleCustomer.Id;
            
            _requestWithCustomerInArrears = new SignupCustomerForStandardCardCommand();
            _requestWithCustomerInArrears.CustomerExternalId = _nonEligibleCustomerInArrears.Id;

            _requestWithInvalidCustomerEmail = new SignupCustomerForStandardCardCommand();
            _requestWithInvalidCustomerEmail.CustomerExternalId = _customerWithWromgEmail.Id;
        }

        [Test, AUT(AUT.Uk), JIRA("PP-8","PP-150")]
        public void CustomerShouldApplyForPrepaidCard()
        {

            ExecuteAPICommand(_validRequest,false);
            ExecuteAPICommand(_requestWithInvalidCustomerId,true);
            ExecuteAPICommand(_requestWithCustomerInArrears,true);
            ExecuteAPICommand(_requestWithInvalidCustomerEmail,false);

            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomer.Id);
            Assert.Throws<Exception>(() => CheckOnAddingRecordsToPrepaidCard(_customerWithWromgEmail.Id));
   
        }

        private void CheckOnAddingRecordsToPrepaidCard(Guid customerId)
        {
            var cardHolderId = Do.Until(() => _prepaidCardDb.CardHolderDetails.FindByCustomerExternalId(CustomerExternalId: customerId));
            var cardDetails = Do.Until(() => _prepaidCardDb.CardDetails.FindByCardHolderExternalId(CardHolderExternalId: cardHolderId.ExternalId));

            String cardAccountNumber = cardDetails.AccountNumber;
            String cardSerialnumber = cardDetails.SerialNumber;

            Assert.IsNotNull(cardDetails.AccountNumber);
            Assert.IsNotNull(cardDetails.SerialNumber);
            Assert.IsTrue(cardAccountNumber.Length <= VALID_ACCOUNT_NUMBER_LENGTH);
            Assert.IsTrue(cardSerialnumber.Length <= VALID_SERIAL_NUMBER_LENGTH);
            Assert.IsNotNull(cardDetails.CardPan);

        }

        private void ExecuteAPICommand(ApiRequest request,bool isThrowException)
        {
            if(isThrowException.Equals(true))
            {
                Assert.Throws<Exception>(() => Drive.Api.Commands.Post(request));   
            }
            else
            {
                Drive.Api.Commands.Post(request);
            }
        }


    }
}

using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Prepaid
{
    [TestFixture, Parallelizable(TestScope.All)]
    class PrepaidCardPPSTest
    {
        private Customer _eligibleCustomerForStandardCard = null;
        private Customer _invalidCustomer = null;
        private Customer _eligibleCustomerForPremiumCard = null;

        private static readonly int VALID_ACCOUNT_NUMBER_LENGTH = 14;
        private static readonly int VALID_SERIAL_NUMBER_LENGTH = 10;

        private static readonly String STANDARD_CARD_TEMPLATE_NAME = "34327";
        private static readonly String PREMIUM_CARD_TEMPLATE_NAME = "34328";

        private static readonly String CARD_STATUS_ACTIVE = "2";
        private static readonly String OPERATION_SUCCESS_STATUS = "CWS0000";
		
        private static readonly dynamic _prepaidCardDb = Drive.Data.PrepaidCard.Db;
        private static readonly dynamic _qaDataDb = Drive.Data.QaData.Db;

        [SetUp]
        public void Init()
        {
            _eligibleCustomerForStandardCard = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _eligibleCustomerForPremiumCard = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _invalidCustomer = CustomerBuilder.New().Build();

            CustomerOperations.CreateMarketingEligibility(_eligibleCustomerForStandardCard.Id, true);
            CustomerOperations.CreateMarketingEligibility(_eligibleCustomerForPremiumCard.Id,true);
            
            ExecuteCommonPPSCommands();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-8", "PP-150")]
        public void CustomerShouldApplyForPrepaidCard()
        {
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomerForStandardCard.Id, CustomerOperations.STANDARD_CARD_TYPE);
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomerForPremiumCard.Id, CustomerOperations.PREMIUM_CARD_TYPE);

            Assert.Throws<Exception>(() => CheckOnAddingRecordsToPrepaidCard(_invalidCustomer.Id, CustomerOperations.PREMIUM_CARD_TYPE));
        }

        [Test, AUT(AUT.Uk), JIRA("PP-79")]
        public void CustomerShouldSendRequestForResetPINCode()
        {

            var validRequestForStandardCard = new GetPrePaidPinResetCodeQuery();
            var invalidRequest = new GetPrePaidPinResetCodeQuery();
            var validRequestForPremiumCard = new GetPrePaidPinResetCodeQuery();

            validRequestForStandardCard.CustomerExternalId = _eligibleCustomerForStandardCard.Id;
            invalidRequest.CustomerExternalId = _invalidCustomer.Id;
            validRequestForPremiumCard.CustomerExternalId = _eligibleCustomerForPremiumCard.Id;

            var successResponseForStandard = Drive.Api.Queries.Post(validRequestForStandardCard);
            var successResponseForPremium = Drive.Api.Queries.Post(validRequestForPremiumCard);
            
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequest));
            Assert.IsNotNull(successResponseForStandard.Values["ResetCode"]);
            Assert.IsNotNull(successResponseForPremium.Values["ResetCode"]);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-11")]
        public void CustomerShouldReceiveEmailWhenApplyPrepaidCard()
        {
            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress: _eligibleCustomerForStandardCard.GetEmail(), TemplateName: STANDARD_CARD_TEMPLATE_NAME));
            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress: _eligibleCustomerForPremiumCard.GetEmail(), TemplateName: PREMIUM_CARD_TEMPLATE_NAME));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-34,PP-35")]
        public void CustomerShouldApplyPrepaidCardAndSetPrepaidFunds()
        {
            Application appForStandard = ApplicationBuilder.New(_eligibleCustomerForStandardCard).Build();
            Application appForPremium = ApplicationBuilder.New(_eligibleCustomerForPremiumCard).Build();

            CustomerOperations.SetFundsForCustomer(appForStandard.Id,true);
            CustomerOperations.SetFundsForCustomer(appForPremium.Id,true);

            Assert.Throws<Exception>(() => CustomerOperations.SetFundsForCustomer(Guid.Empty, true));
        }

        [Test, AUT(AUT.Uk), JIRA("PP-34,PP-35")]
        public void CustomerShouldApplyPrepaidCardAndSetDefaultFunds()
        {
            Application appForStandard = ApplicationBuilder.New(_eligibleCustomerForStandardCard).Build();
            Application appForPremium = ApplicationBuilder.New(_eligibleCustomerForPremiumCard).Build();

            CustomerOperations.SetFundsForCustomer(appForStandard.Id,false);
            CustomerOperations.SetFundsForCustomer(appForPremium.Id,false);

            Assert.Throws<Exception>(() => CustomerOperations.SetFundsForCustomer(Guid.Empty, false));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-31")]
        public void CustomerShouldUpdatePersonalDetailsAndUpdateItOnPPS()
        {

            CustomerOperations.UpdateAddress(_eligibleCustomerForStandardCard.Id);
            CustomerOperations.UpdateEmail(_eligibleCustomerForStandardCard.Id);
            CustomerOperations.UpdateMobilePhone(_eligibleCustomerForPremiumCard.Id);

            CustomerOperations.UpdateAddress(_eligibleCustomerForPremiumCard.Id);
            CustomerOperations.UpdateEmail(_eligibleCustomerForPremiumCard.Id);
            CustomerOperations.UpdateMobilePhone(_eligibleCustomerForPremiumCard.Id);

            var operationsForStandardCardUser = Do.Until(() => _prepaidCardDb.OperationsLogs.FindAllBy(CustomerExternalId: _eligibleCustomerForStandardCard.Id,
                                                                                                       StatusCode: OPERATION_SUCCESS_STATUS));

            var operationsForPremiumCardUser = Do.Until(() => _prepaidCardDb.OperationsLogs.FindAllBy(CustomerExternalId: _eligibleCustomerForPremiumCard.Id,
                                                                                                      StatusCode: OPERATION_SUCCESS_STATUS));

            Assert.IsTrue(operationsForStandardCardUser.Count() == operationsForPremiumCardUser.Count());
        }

        [Test,AUT(AUT.Uk),JIRA("PP-215")]
        public void CustomerShouldGetTransactionListFromPPS()
        {

            var validRequestForStandardCard = new GetPrepaidCardTransactionsQuery();
            validRequestForStandardCard.AccountId = _eligibleCustomerForStandardCard.Id;

            var invalidRequestForNonExistingAccount = new GetPrepaidCardTransactionsQuery();
            invalidRequestForNonExistingAccount.AccountId = Guid.Empty;

            var invalidRequest = new GetPrepaidCardTransactionsQuery();
            invalidRequest.AccountId = _invalidCustomer.Id;

            var validRequestForPremiumCard = new GetPrepaidCardTransactionsQuery();
            validRequestForPremiumCard.AccountId = _eligibleCustomerForPremiumCard.Id;

            Drive.Api.Queries.Post(validRequestForStandardCard);
            Drive.Api.Queries.Post(validRequestForPremiumCard);

            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequestForNonExistingAccount));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequest));
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomerForStandardCard.Id);
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomerForPremiumCard.Id);
        }

        private void CheckOnAddingRecordsToPrepaidCard(Guid customerId,String cardType)
        {
            var cardHolderId = Do.Until(() => _prepaidCardDb.CardHolderDetails.FindByCustomerExternalId(customerId));
            var cardDetails = Do.Until(() => _prepaidCardDb.CardDetails.FindBy(CardHolderExternalId: cardHolderId.ExternalId, CardType: cardType));
          
            String cardAccountNumber = cardDetails.AccountNumber;
            String cardSerialnumber = cardDetails.SerialNumber;
            String cardStatus = cardDetails.CardStatus;

            Assert.IsTrue(cardAccountNumber.Length <= VALID_ACCOUNT_NUMBER_LENGTH);
            Assert.IsTrue(cardSerialnumber.Length <= VALID_SERIAL_NUMBER_LENGTH);
            Assert.IsTrue(cardStatus.Equals(CARD_STATUS_ACTIVE));
            Assert.IsNotNull(cardDetails.CardPan);
        }

        private void ExecuteCommonPPSCommands()
        {
            CustomerOperations.CreatePrepaidCardForCustomer(_eligibleCustomerForStandardCard.Id,false);
            CustomerOperations.CreatePrepaidCardForCustomer(_eligibleCustomerForPremiumCard.Id,true);
        }
    }
}
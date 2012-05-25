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
        private TestLocal<Customer> _eligibleCustomerForStandardCard = new TestLocal<Customer>();
        private TestLocal<Customer> _invalidCustomer = new TestLocal<Customer>();
        private TestLocal<Customer> _eligibleCustomerForPremiumCard =new TestLocal<Customer>();

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
            _eligibleCustomerForStandardCard.Value = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _eligibleCustomerForPremiumCard.Value = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
            _invalidCustomer.Value = CustomerBuilder.New().Build();

            CustomerOperations.CreateMarketingEligibility(_eligibleCustomerForStandardCard.Value.Id, true);
            CustomerOperations.CreateMarketingEligibility(_eligibleCustomerForPremiumCard.Value.Id,true);
            
            ExecuteCommonPPSCommands();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-8", "PP-150")]
        public void CustomerShouldApplyForPrepaidCard()
        {
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomerForStandardCard.Value.Id, CustomerOperations.STANDARD_CARD_TYPE);
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomerForPremiumCard.Value.Id, CustomerOperations.PREMIUM_CARD_TYPE);

            Assert.Throws<Exception>(() => CheckOnAddingRecordsToPrepaidCard(_invalidCustomer.Value.Id, CustomerOperations.PREMIUM_CARD_TYPE));
        }

        [Test, AUT(AUT.Uk), JIRA("PP-79")]
        public void CustomerShouldSendRequestForResetPINCode()
        {

            var validRequestForStandardCard = new GetPrePaidPinResetCodeQuery();
            var invalidRequest = new GetPrePaidPinResetCodeQuery();
            var validRequestForPremiumCard = new GetPrePaidPinResetCodeQuery();

            validRequestForStandardCard.CustomerExternalId = _eligibleCustomerForStandardCard.Value.Id;
            invalidRequest.CustomerExternalId = _invalidCustomer.Value.Id;
            validRequestForPremiumCard.CustomerExternalId = _eligibleCustomerForPremiumCard.Value.Id;

            var successResponseForStandard = Drive.Api.Queries.Post(validRequestForStandardCard);
            var successResponseForPremium = Drive.Api.Queries.Post(validRequestForPremiumCard);
            
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequest));
            Assert.IsNotNull(successResponseForStandard.Values["ResetCode"]);
            Assert.IsNotNull(successResponseForPremium.Values["ResetCode"]);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-11")]
        public void CustomerShouldReceiveEmailWhenApplyPrepaidCard()
        {
            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress: _eligibleCustomerForStandardCard.Value.GetEmail(), TemplateName: STANDARD_CARD_TEMPLATE_NAME));
            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress: _eligibleCustomerForPremiumCard.Value.GetEmail(), TemplateName: PREMIUM_CARD_TEMPLATE_NAME));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-34,PP-35")]
        public void CustomerShouldApplyPrepaidCardAndSetPrepaidFunds()
        {
            Application appForStandard = ApplicationBuilder.New(_eligibleCustomerForStandardCard.Value).Build();
            Application appForPremium = ApplicationBuilder.New(_eligibleCustomerForPremiumCard.Value).Build();

            CustomerOperations.SetFundsForCustomer(appForStandard.Id,true);
            CustomerOperations.SetFundsForCustomer(appForPremium.Id,true);

            Assert.Throws<Exception>(() => CustomerOperations.SetFundsForCustomer(Guid.Empty, true));
        }

        [Test, AUT(AUT.Uk), JIRA("PP-34,PP-35")]
        public void CustomerShouldApplyPrepaidCardAndSetDefaultFunds()
        {
            Application appForStandard = ApplicationBuilder.New(_eligibleCustomerForStandardCard.Value).Build();
            Application appForPremium = ApplicationBuilder.New(_eligibleCustomerForPremiumCard.Value).Build();

            CustomerOperations.SetFundsForCustomer(appForStandard.Id,false);
            CustomerOperations.SetFundsForCustomer(appForPremium.Id,false);

            Assert.Throws<Exception>(() => CustomerOperations.SetFundsForCustomer(Guid.Empty, false));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-31")]
        public void CustomerShouldUpdatePersonalDetailsAndUpdateItOnPPS()
        {

            CustomerOperations.UpdateAddress(_eligibleCustomerForStandardCard.Value.Id);
            CustomerOperations.UpdateEmail(_eligibleCustomerForStandardCard.Value.Id);
            CustomerOperations.UpdateMobilePhone(_eligibleCustomerForPremiumCard.Value.Id);

            CustomerOperations.UpdateAddress(_eligibleCustomerForPremiumCard.Value.Id);
            CustomerOperations.UpdateEmail(_eligibleCustomerForPremiumCard.Value.Id);
            CustomerOperations.UpdateMobilePhone(_eligibleCustomerForPremiumCard.Value.Id);

            var operationsForStandardCardUser = Do.Until(() => _prepaidCardDb.OperationsLogs.FindAllBy(
                                                                    CustomerExternalId: _eligibleCustomerForStandardCard.Value.Id,
                                                                    StatusCode: OPERATION_SUCCESS_STATUS));

            var operationsForPremiumCardUser = Do.Until(() => _prepaidCardDb.OperationsLogs.FindAllBy(
                                                                    CustomerExternalId: _eligibleCustomerForPremiumCard.Value.Id,
                                                                    StatusCode: OPERATION_SUCCESS_STATUS));
               

            Assert.IsTrue(operationsForStandardCardUser.Count() == operationsForPremiumCardUser.Count());
        }

        [Test,AUT(AUT.Uk),JIRA("PP-215")]
        public void CustomerShouldGetTransactionListFromPPS()
        {

            var validRequestForStandardCard = new GetPrepaidCardTransactionsQuery();
            validRequestForStandardCard.AccountId = _eligibleCustomerForStandardCard.Value.Id;

            var invalidRequestForNonExistingAccount = new GetPrepaidCardTransactionsQuery();
            invalidRequestForNonExistingAccount.AccountId = Guid.Empty;

            var invalidRequest = new GetPrepaidCardTransactionsQuery();
            invalidRequest.AccountId = _invalidCustomer.Value.Id;

            var validRequestForPremiumCard = new GetPrepaidCardTransactionsQuery();
            validRequestForPremiumCard.AccountId = _eligibleCustomerForPremiumCard.Value.Id;

            Drive.Api.Queries.Post(validRequestForStandardCard);
            Drive.Api.Queries.Post(validRequestForPremiumCard);

            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequestForNonExistingAccount));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequest));
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomerForStandardCard.Value.Id);
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomerForPremiumCard.Value.Id);

            _eligibleCustomerForPremiumCard.Value = null;
            _eligibleCustomerForStandardCard.Value = null;
            _invalidCustomer.Value = null;
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
            CustomerOperations.CreatePrepaidCardForCustomer(_eligibleCustomerForStandardCard.Value.Id,false);
            CustomerOperations.CreatePrepaidCardForCustomer(_eligibleCustomerForPremiumCard.Value.Id,true);
        }
    }
}
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
        private Customer _eligibleCustomer = null;
        private Customer _nonEligibleCustomer = null;
        private Customer _nonEligibleCustomerInArrears = null;
        private Customer _customerWithWromgEmail = null;

        private SignupCustomerForStandardCardCommand _validRequest = null;
        private SignupCustomerForStandardCardCommand _requestWithInvalidCustomerId = null;
        private SignupCustomerForStandardCardCommand _requestWithCustomerInArrears = null;
        private SignupCustomerForStandardCardCommand _requestWithInvalidCustomerEmail = null;

        private SignupCustomerForPremiumCardCommand _validPremiumCardRequest = null;
        private SignupCustomerForPremiumCardCommand _invalidPremiumCardRequest = null;

        private static readonly int VALID_ACCOUNT_NUMBER_LENGTH = 14;
        private static readonly int VALID_SERIAL_NUMBER_LENGTH = 10;

        private static readonly String STANDARD_CARD_TEMPLATE_NAME = "34327";
        private static readonly String PREMIUM_CARD_TEMPLATE_NAME = "34328";
        private static readonly String STANDARD_CARD_TYPE = "0";
        private static readonly String PREMIUM_CARD_TYPE = "1";

        private static readonly String CARD_STATUS_ACTIVE = "2";

        private static readonly dynamic _prepaidCardDb = Drive.Data.PrepaidCard.Db;
        private static readonly dynamic _qaDataDb = Drive.Data.QaData.Db;

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

            _validPremiumCardRequest = new SignupCustomerForPremiumCardCommand();
            _validPremiumCardRequest.CustomerExternalId = _eligibleCustomer.Id;

            _invalidPremiumCardRequest = new SignupCustomerForPremiumCardCommand();
            _invalidPremiumCardRequest.CustomerExternalId = _nonEligibleCustomer.Id;
        }

        [Test, AUT(AUT.Uk), JIRA("PP-8", "PP-150")]
        public void CustomerShouldApplyForPrepaidCard()
        {
            ExecuteCommonPPSCommands();
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomer.Id);
            Assert.Throws<Exception>(() => CheckOnAddingRecordsToPrepaidCard(_customerWithWromgEmail.Id));
        }

        [Test, AUT(AUT.Uk), JIRA("PP-79")]
        public void CustomerShouldsendRequestForResetPINCode()
        {
            ExecuteCommonPPSCommands();
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomer.Id);

            var validRequest = new GetPrepaidResetCodeQuery();
            var invalidRequest = new GetPrepaidResetCodeQuery();
            var invalidRequestForCustomerInArrears = new GetPrepaidResetCodeQuery();
            var invalidRequestCustomerWithWrongEmail = new GetPrepaidResetCodeQuery();

            validRequest.CustomerExternalId = _eligibleCustomer.Id;
            invalidRequest.CustomerExternalId = _nonEligibleCustomer.Id;
            invalidRequestForCustomerInArrears.CustomerExternalId = _nonEligibleCustomerInArrears.Id;
            invalidRequestCustomerWithWrongEmail.CustomerExternalId = _nonEligibleCustomerInArrears.Id;

            var successResponse = Drive.Api.Queries.Post(validRequest);
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequest));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequestForCustomerInArrears));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(invalidRequestCustomerWithWrongEmail));
            Assert.IsNotNull(successResponse.Values["ResetCode"]);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-11")]
        public void CustomerShouldReceiveEmailWhenApplyPrepaidCard()
        {
            ExecuteCommonPPSCommands();
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomer.Id);

            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress: _eligibleCustomer.GetEmail(), TemplateName: STANDARD_CARD_TEMPLATE_NAME));
            Do.Until(() => _qaDataDb.Email.FindBy(EmailAddress: _eligibleCustomer.GetEmail(), TemplateName: PREMIUM_CARD_TEMPLATE_NAME));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-34"),Pending("Command not fully implemented ")]
        public void CustomerShouldApplyPrepaidCardAndSetPrepaidFunds()
        {
            ExecuteCommonPPSCommands();
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomer.Id);
            Application application = ApplicationBuilder.New(_eligibleCustomer).Build();

            var setFundsCommand = new SetFundsTransferMethodCommand();
            setFundsCommand.ApplicationId = application.Id;
            setFundsCommand.TransferMethod = FundsTransferEnum.SendToPrepaidCard;

            Drive.Api.Commands.Post(setFundsCommand);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-34"), Pending("Command not fully implemented")]
        public void CustomerShouldApplyPrepaidCardAndSetDefaultFunds()
        {
            ExecuteCommonPPSCommands();
            CheckOnAddingRecordsToPrepaidCard(_eligibleCustomer.Id);
            Application application = ApplicationBuilder.New(_eligibleCustomer).Build();

            var setFundsCommand = new SetFundsTransferMethodCommand();
            setFundsCommand.ApplicationId = application.Id;
            setFundsCommand.TransferMethod = FundsTransferEnum.DefaultAutomaticallyChosen;

            Drive.Api.Commands.Post(setFundsCommand);
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomer.Id);
            CustomerOperations.DeleteMarketingEligibility(_nonEligibleCustomerInArrears.Id);
            CustomerOperations.DeleteMarketingEligibility(_customerWithWromgEmail.Id);
        }

        private void CheckOnAddingRecordsToPrepaidCard(Guid customerId)
        {
            var cardHolderId = Do.Until(() => _prepaidCardDb.CardHolderDetails.FindAllByCustomerExternalId(customerId));
            var standardCardDetails = Do.Until(() => _prepaidCardDb.CardDetails.FindBy(CardHolderExternalId: cardHolderId.First().ExternalId, CardType: STANDARD_CARD_TYPE));
            var premiumCardDetails = Do.Until(() => _prepaidCardDb.CardDetails.FindBy(CardHolderExternalId: cardHolderId.OrderByIdDescending().First().ExternalId, CardType: PREMIUM_CARD_TYPE));
           
            String standardCardAccountNumber = standardCardDetails.AccountNumber;
            String standardCardSerialnumber = standardCardDetails.SerialNumber;
            String standardCardStatus = standardCardDetails.CardStatus;

            String premiumCardAccountNumber = premiumCardDetails.AccountNumber;
            String premiumCardSerialnumber = premiumCardDetails.SerialNumber;
            String premiumCardStatus = premiumCardDetails.CardStatus;

            Assert.IsNotNull(standardCardDetails.AccountNumber);
            Assert.IsNotNull(premiumCardDetails.AccountNumber);
            Assert.IsNotNull(standardCardDetails.SerialNumber);
            Assert.IsNotNull(premiumCardDetails.SerialNumber);

            Assert.IsTrue(standardCardAccountNumber.Length <= VALID_ACCOUNT_NUMBER_LENGTH);
            Assert.IsTrue(premiumCardAccountNumber.Length <= VALID_ACCOUNT_NUMBER_LENGTH);
            Assert.IsTrue(standardCardSerialnumber.Length <= VALID_SERIAL_NUMBER_LENGTH);
            Assert.IsTrue(premiumCardSerialnumber.Length <= VALID_SERIAL_NUMBER_LENGTH);

            Assert.IsTrue(standardCardStatus.Equals(CARD_STATUS_ACTIVE));
            Assert.IsTrue(premiumCardStatus.Equals(CARD_STATUS_ACTIVE));

            Assert.IsNotNull(standardCardDetails.CardPan);
            Assert.IsNotNull(premiumCardDetails.CardPan);
        }

        private void ExecuteAPICommand(ApiRequest request, bool isThrowException)
        {
            if (isThrowException.Equals(true))
            {
                Assert.Throws<Exception>(() => Drive.Api.Commands.Post(request));
            }
            else
            {
                Drive.Api.Commands.Post(request);
            }
        }

        private void ExecuteCommonPPSCommands()
        {
            ExecuteAPICommand(_validRequest, false);
            ExecuteAPICommand(_requestWithInvalidCustomerId, true);
            ExecuteAPICommand(_requestWithCustomerInArrears, true);
            ExecuteAPICommand(_requestWithInvalidCustomerEmail, false);

            ExecuteAPICommand(_validPremiumCardRequest,false);
            ExecuteAPICommand(_invalidPremiumCardRequest,true);
        }
    }
}
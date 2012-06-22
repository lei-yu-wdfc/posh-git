using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using PaymentTransactionEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;


namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    public class GetAccountOptionsTests
    {

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario01ExistingCustomerWithoutLiveLoan()
        {
            const decimal trustRating = 400.00M;
            var applicationId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var setupData = new AccountSummarySetupFunctions();

            setupData.Scenario01Setup(accountId, applicationId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating});
            Assert.AreEqual(1, int.Parse(response.Values["ScenarioId"].Single()));
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario02CustomerWithLiveLoanWithAvailableCreditTooEarlyToExtend()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();

            setupData.Scenario02Setup(appId, paymentCardId, bankAccountId, accountId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(2,int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario03CustomerWithLiveLoanWithAvailableCreditCanExtendLoan()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            var setupData = new AccountSummarySetupFunctions();
            const decimal trustRating = 400.00M;

            setupData.Scenario03Setup(appId, paymentCardId, bankAccountId, accountId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(3, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

       [Test, AUT(AUT.Uk), JIRA("UK-823"), Repeat(2)]
        public void Scenario04CustomerWithLiveLoanWithAvailableCreditCantExtendLoanDueTooMaxExtensions()
        {
                var accountId = Guid.NewGuid();
                var bankAccountId = Guid.NewGuid();
                var paymentCardId = Guid.NewGuid();
                var appId = Guid.NewGuid();
                var setupData = new AccountSummarySetupFunctions();
                const decimal trustRating = 400.00M;

                setupData.Scenario04SetupMaxExtensionsReached(paymentCardId, appId, bankAccountId, accountId, trustRating);

                var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
                Assert.AreEqual(4, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario05CustomerWithLiveLoanWithoutAvailableCreditCantExtendTooEarly()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 100.00M;

            var setupData = new AccountSummarySetupFunctions(); 
            setupData.Scenario05Setup(paymentCardId, appId, bankAccountId, accountId, trustRating);


            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(5, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }    

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario06CustomerWithLiveLoanWithoutAvailableCreditCanExtend()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 100.00M;

            var setupData = new AccountSummarySetupFunctions(); 
            setupData.Scenario06Setup(appId, paymentCardId, bankAccountId, accountId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(6, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823"), Repeat(2)]
        public void Scenario07CustomerWithLiveLoanWithoutAvailableCreditCannotExtendTooManyExts()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 100.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario07Setup(paymentCardId, bankAccountId, appId, accountId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(7, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

		[Test, AUT(AUT.Uk), JIRA("UK-1904")]
		public void Scenario07CustomerWithLiveLoanWithAvailableCreditCantExtendOnDueDate()
		{
			var accountId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			var appId = Guid.NewGuid();
			var setupData = new AccountSummarySetupFunctions();
			const decimal trustRating = 400.00M;

			setupData.Scenario04SetupCannotExtendOnDueDate(paymentCardId, appId, bankAccountId, accountId, trustRating);

			var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
			Assert.AreEqual(7, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
		}

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario08CustomerRepaidLoanTodayViaScheduledPayment()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 100.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario08Setup(paymentCardId, bankAccountId, accountId, appId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(8, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario09CustomerWithLiveLoanOnPromiseDateScheduledPaymentFailed()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario09Setup(requestId2, requestId1, accountId, paymentCardId, appId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(9, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario10CustomerWithLiveLoanWithMissedPaymentFeeOneDayInArrears()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario10Setup(requestId1, requestId2, appId, bankAccountId, accountId, paymentCardId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(10, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario11CustomerWithLiveLoanThreeDaysInArrears()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario11Setup(requestId1, requestId2, appId, bankAccountId, accountId, paymentCardId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(11, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario12CustomerWithLiveLoanThirtyOneDaysInArrears()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario12Setup(requestId1, requestId2, appId, bankAccountId, accountId, paymentCardId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(12, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario13CustomerWithLiveLoanSixtyOneDaysInArrears()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario13Setup(requestId1, requestId2, appId, bankAccountId, accountId, paymentCardId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(13, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario14CustomerInArrearsWithRepaymentArrangementInGoodOrder()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario14Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(14, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario15CustomerInArrearsWithRepaymentArrangementWithMissedPayment()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario15Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(15, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario16CustomerInArrearsWithBrokenRepaymentArrangement()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario16Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(16, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario17NewCustomerWithApplicationInProgress()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario17Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(17, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario20NewCustomerLastApplicationDeclined()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario20Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(20, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        [Test, AUT(AUT.Uk), JIRA("UK-823")]
        public void Scenario21CustomerApplicationAwaitingSignature()
        {
            var accountId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario21Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(21, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");
        }

        #region "Helpers"

            private Guid CreateLoanAdvanceTransaction(Guid appId)
            {
                var trnGuid1 = Guid.NewGuid();

                Drive.Msmq.Payments.Send(new CreateTransactionCommand()
                {
                    ApplicationId = appId,
                    ExternalId = trnGuid1,
                    Amount = 100.00M,
                    Type = PaymentTransactionEnum.CashAdvance,
                    Currency = CurrencyCodeIso4217Enum.GBP,
                    Mir = 30.0M,
                    PostedOn = DateTime.Now,
                    Scope = PaymentTransactionScopeEnum.Debit,
                    Reference = "Test Cash Advance"
                });
                return trnGuid1;
            }

            private Guid CreateTransmissionFeeTransaction(Guid appId)
            {
                var trnGuid1 = Guid.NewGuid();

                Drive.Msmq.Payments.Send(new CreateTransactionCommand()
                {
                    ApplicationId = appId,
                    ExternalId = trnGuid1,
                    Amount = 5.50M,
                    Type = PaymentTransactionEnum.Fee,
                    Currency = CurrencyCodeIso4217Enum.GBP,
                    Mir = 30.0M,
                    PostedOn = DateTime.Now,
                    Scope = PaymentTransactionScopeEnum.Debit,
                    Reference = "Test Transmission fee"
                });
                return trnGuid1;
            }

            private Guid CreateExtensionFeeTransaction(Guid appId)
            {
                var trnGuid1 = Guid.NewGuid();

                Drive.Msmq.Payments.Send(new CreateTransactionCommand()
                {
                    ApplicationId = appId,
                    ExternalId = trnGuid1,
                    Amount = 20.00M,
                    Type = PaymentTransactionEnum.LoanExtensionFee,
                    Currency = CurrencyCodeIso4217Enum.GBP,
                    Mir = 30.0M,
                    PostedOn = DateTime.Now,
                    Scope = PaymentTransactionScopeEnum.Debit,
                    Reference = "Test Extension fee"
                });
                return trnGuid1;
            }

            private Guid CreateMissedPaymentChargeTransaction(Guid appId)
            {
                var trnGuid1 = Guid.NewGuid();

                Drive.Msmq.Payments.Send(new CreateTransactionCommand()
                {
                    ApplicationId = appId,
                    ExternalId = trnGuid1,
                    Amount = 20.00M,
                    Type = PaymentTransactionEnum.DefaultCharge,
                    Currency = CurrencyCodeIso4217Enum.GBP,
                    Mir = 30.0M,
                    PostedOn = DateTime.Now,
                    Scope = PaymentTransactionScopeEnum.Debit,
                    Reference = "Test Missed Payment Charge"
                });
                return trnGuid1;
            }
		#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk.Wb
{
    class RiskTests
    {
        [Test]
        public void CreateBusinessFixedInstallmentLoanApplication_LoanApplicationIsCreatedSuccessfully()
        {
            var applicationId = Guid.NewGuid();
            var organisationId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();

            Driver.Api.Commands.Post(new ApiRequest[]
                                         {
                                             new AddPaymentCardCommand()
                                                 {
                                                     AccountId = accountId,
                                                     PaymentCardId = paymentCardId,
                                                     CardType = "VISA",
                                                     Number = "4444333322221111",
                                                     HolderName = "Test Holder",
                                                     ExpiryDate =
                                                         DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
                                                     IssueNo = "123",
                                                     SecurityCode = "123",
                                                     IsCreditCard = false,
                                                     StartDate =
                                                         DateTime.Today.AddMonths(-1).ToDate(DateFormat.YearMonth),
                                                     IsPrimary = true
                                                 },
                                             new AddBankAccountUkCommand()
                                                 {
                                                     AccountId = accountId,
                                                     BankAccountId = bankAccountId,
                                                     BankName = "HSBC",
                                                     BankCode = "309894",
                                                     AccountNumber = "14690568",
                                                     HolderName = "Test Holder",
                                                     AccountOpenDate = DateTime.Today.AddMonths(-6).ToDate(DateFormat.DateTime),
                                                     //This does not need ToDate because it's identical.
                                                     CountryCode = "UK",
                                                     IsPrimary = false
                                                 },
                                         }
                );

            Do.Until(() => Driver.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId));
            Do.Until(() => Driver.Db.Payments.BankAccountsBases.Single(x => x.ExternalId == bankAccountId));

            var response = Driver.Api.Commands.Post(CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
            {
                r.AccountId = accountId;
                r.ApplicationId = applicationId;
                r.BusinessPaymentCardId = paymentCardId;
                r.OrganisationId = organisationId;
                r.BusinessBankAccountId = bankAccountId;
            }));
            Assert.IsNotNull(response);
        }
    }
}

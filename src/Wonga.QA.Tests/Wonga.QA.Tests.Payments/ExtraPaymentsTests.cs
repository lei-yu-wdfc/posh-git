using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture]
    public class ExtraPaymentsTests
    {
        private BusinessApplication application;
        private PaymentPlanEntity paymentPlan;

        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New().WithPrimaryApplicant(customer).Build();

            application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build() as BusinessApplication;
            paymentPlan = application.GetPaymentPlan();
        }

        private void CreateExtraPayment(decimal extraPaymentAmount)
        {
            Driver.Msmq.Payments.Send(new CreateTransactionCommand
                                          {
                                              Amount = extraPaymentAmount,
                                              ApplicationId = application.Id,
                                              Currency = CurrencyCodeIso4217Enum.GBP,
                                              ExternalId = Guid.NewGuid(),
                                              ComponentTransactionId = Guid.Empty,
                                              PostedOn = DateTime.Now,
                                              Scope = PaymentTransactionScopeEnum.Credit,
                                              Source = PaymentTransactionSourceEnum.System,
                                              Type = PaymentTransactionEnum.Cheque
                                          });
        }

        private bool IsInArrears()
        {
            var accountId = Do.Until(() => Driver.Db.Payments.AccountsApplications.Single(a => a.ApplicationEntity.ExternalId == application.Id).AccountId);
            var response = Driver.Api.Queries.Post(new GetBusinessAccountSummaryWbUkQuery
            {
                AccountId = accountId
            });

            var arrears = 0M;
            var arrearsString = response.Values["Arrears"].SingleOrDefault();
            if (!string.IsNullOrEmpty(arrearsString))
            {
                decimal.TryParse(arrearsString, out arrears);
            }
            return arrears > 0M;
        }

        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadePaymentsShouldAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = paymentPlan.RegularAmount / 2M;
            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.LessThan(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenWhenItIsMoreThanArrearsMadePaymentsShouldAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = paymentPlan.RegularAmount * 2M;
            application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.LessThan(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            // Check that app is in not arrears
            Assert.IsFalse(IsInArrears());
        }

        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenWhenItIsLessThanArrearsMadePaymentsShouldNotAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = paymentPlan.RegularAmount / 2M;
            application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            // Check that app is in arrears
            Assert.IsTrue(IsInArrears());
        }

        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenWhenItIsEqualArrearsMadePaymentsShouldNotAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = paymentPlan.RegularAmount;
            application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            // Check that app is in not arrears
            Assert.IsFalse(IsInArrears());
        }
    }
}

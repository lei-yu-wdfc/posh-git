using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture]
    public class ExtraPaymentsTests
    {
        private BusinessApplication _application;
        private PaymentPlanEntity _paymentPlan;

        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();

            _application = ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build() as BusinessApplication;
            _paymentPlan = _application.GetPaymentPlan();
        }

        private void CreateExtraPayment(decimal extraPaymentAmount)
        {
            Driver.Msmq.Payments.Send(new CreateTransactionCommand
                                          {
                                              Amount = extraPaymentAmount,
                                              ApplicationId = _application.Id,
                                              Currency = CurrencyCodeIso4217Enum.GBP,
                                              ExternalId = Guid.NewGuid(),
                                              ComponentTransactionId = Guid.Empty,
                                              PostedOn = DateTime.Now,
                                              Scope = PaymentTransactionScopeEnum.Credit,
                                              Source = PaymentTransactionSourceEnum.System,
                                              Type = PaymentTransactionEnum.Cheque
                                          });

            // Wait for the TX to be processed
            Thread.Sleep(15000);
        }

        /// <summary>
        /// Given the repayment plan is up to date 
        /// When an extra payment has been made 
        /// Then update the weekly repayment plan amount 
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadePaymentsShouldAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = _paymentPlan.RegularAmount / 2M;
            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = _application.GetPaymentPlan();
            Assert.LessThan(newPaymentPlan.RegularAmount, _paymentPlan.RegularAmount);
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == _application.Id
                                                        && t.Type == PaymentTransactionEnum.InterestAdjustment.ToString()));
        }

        /// <summary>
        /// Given the repayment plan is in arrears 
        /// When an extra payment has been made that is more than the arrears amount. 
        /// Then apply the payment to the arrears amount first, and any amount that is left should be considered an over payment and the weekly repayment should be amended. 
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsMoreThanArrearsPaymentsShouldAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = _paymentPlan.RegularAmount * 2M;
            _application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = _application.GetPaymentPlan();

            Assert.LessThan(newPaymentPlan.RegularAmount, _paymentPlan.RegularAmount);
            Do.Until(() => Driver.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == _application.Id
                                            && t.Type == PaymentTransactionEnum.InterestAdjustment.ToString()));
            // Check that app is in not arrears
            Assert.IsFalse(_application.IsInArrears());
        }

        /// <summary>
        /// Given the repayment plan is in arrears 
        /// When an extra payment has been made that is less than the arrears amount. 
        /// Then do not amend the weekly repayment, and still consider the application to be in arrears. 
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsLessThanArrearsPaymentsShouldNotAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = _paymentPlan.RegularAmount / 2M;
            _application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = _application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, _paymentPlan.RegularAmount);
            // Check that app is in arrears
            Assert.IsTrue(_application.IsInArrears());
        }

        /// <summary>
        /// Given the repayment plan is in arrears 
        /// When an extra payment has been made that is equal to the arrears amount. 
        /// Then do not amend the weekly repayment, but consider the plan to no longer be in arrears.
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsEqualArrearsPaymentsShouldNotAmmendWeeklyRepayment()
        {
            decimal extraPaymentAmount = _paymentPlan.RegularAmount;
            _application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = _application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, _paymentPlan.RegularAmount);
            // Check that app is in not arrears
            Assert.IsFalse(_application.IsInArrears());
        }

        /// <summary>
        /// Given the repayment plan is in arrears s
        /// When an extra payment has been made that is equal to the arrears amount+outstanding fees. 
        /// Then do not amend the weekly repayment, but consider the plan to no longer be in arrears.
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsEqualArrearsAndFeesPaymentsShouldNotAmmendWeeklyRepayment()
        {
            // arrears + default charges
            decimal extraPaymentAmount = _paymentPlan.RegularAmount + 10;
            _application.PutApplicationIntoArrears();

            CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = _application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, _paymentPlan.RegularAmount);
            // Check that app is in not arrears
            Assert.IsFalse(_application.IsInArrears());
            //Assert.IsFalse(application.HasOutstandingCharges());
        }
    }
}

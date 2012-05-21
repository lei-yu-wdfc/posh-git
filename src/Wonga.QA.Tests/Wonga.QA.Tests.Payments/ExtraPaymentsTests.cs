using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.All)]
    public class ExtraPaymentsTests
    {
        private static void InitApplication(out PaymentPlanEntity paymentPlan, out BusinessApplication application)
        {
            var customer = CustomerBuilder.New().Build();
            var organization = OrganisationBuilder.New(customer).Build();

            application =
                ApplicationBuilder.New(customer, organization).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build()
                as BusinessApplication;
            paymentPlan = application.GetPaymentPlan();
        }

        /// <summary>
        /// Given the repayment plan is up to date 
        /// When an extra payment has been made 
        /// Then update the weekly repayment plan amount 
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadePaymentsShouldAmmendWeeklyRepayment()
        {
            PaymentPlanEntity paymentPlan;
            BusinessApplication application;
            InitApplication(out paymentPlan, out application);

            decimal extraPaymentAmount = paymentPlan.RegularAmount / 2M;
            application.CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();
            Assert.LessThan(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            Do.Until(() => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == application.Id
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
            PaymentPlanEntity paymentPlan;
            BusinessApplication application;
            InitApplication(out paymentPlan, out application);

            decimal extraPaymentAmount = paymentPlan.RegularAmount * 2M;
            application.PutApplicationIntoArrears();

            application.CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.LessThan(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            Do.Until(() => Drive.Db.Payments.Transactions.Single(t => t.ApplicationEntity.ExternalId == application.Id
                                            && t.Type == PaymentTransactionEnum.InterestAdjustment.ToString()));
            // Check that app is in not arrears
            Assert.IsFalse(application.IsInArrears());
        }

        /// <summary>
        /// Given the repayment plan is in arrears 
        /// When an extra payment has been made that is less than the arrears amount. 
        /// Then do not amend the weekly repayment, and still consider the application to be in arrears. 
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsLessThanArrearsPaymentsShouldNotAmmendWeeklyRepayment()
        {
            PaymentPlanEntity paymentPlan;
            BusinessApplication application;
            InitApplication(out paymentPlan, out application);

            decimal extraPaymentAmount = paymentPlan.RegularAmount / 2M;
            application.PutApplicationIntoArrears();

            application.CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            // Check that app is in arrears
            Assert.IsTrue(application.IsInArrears());
        }

        /// <summary>
        /// Given the repayment plan is in arrears 
        /// When an extra payment has been made that is equal to the arrears amount. 
        /// Then do not amend the weekly repayment, but consider the plan to no longer be in arrears.
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsEqualArrearsPaymentsShouldNotAmmendWeeklyRepayment()
        {
            PaymentPlanEntity paymentPlan;
            BusinessApplication application;
            InitApplication(out paymentPlan, out application);

            decimal extraPaymentAmount = paymentPlan.RegularAmount;
            application.PutApplicationIntoArrears();

            application.CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            // Check that app is in not arrears
            Assert.IsFalse(application.IsInArrears());
        }

        /// <summary>
        /// Given the repayment plan is in arrears s
        /// When an extra payment has been made that is equal to the arrears amount+outstanding fees. 
        /// Then do not amend the weekly repayment, but consider the plan to no longer be in arrears.
        /// </summary>
        [Test, AUT(AUT.Wb), JIRA("SME-800")]
        public void GivenExtraPaymentHasBeenMadeWhenItIsEqualArrearsAndFeesPaymentsShouldNotAmmendWeeklyRepayment()
        {
            PaymentPlanEntity paymentPlan;
            BusinessApplication application;
            InitApplication(out paymentPlan, out application);

            // arrears + default charges
            decimal extraPaymentAmount = paymentPlan.RegularAmount + 10;
            application.PutApplicationIntoArrears();

            application.CreateExtraPayment(extraPaymentAmount);
            var newPaymentPlan = application.GetPaymentPlan();

            Assert.AreEqual(newPaymentPlan.RegularAmount, paymentPlan.RegularAmount);
            // Check that app is in not arrears
            Assert.IsFalse(application.IsInArrears());
            //Assert.IsFalse(application.HasOutstandingCharges());
        }
    }
}

using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Payments
{
    public class PaymentsTestsViaBankGatewayScotia
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentCreditedToCustomerAccount()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int earlyOnlineRepaymentTerm = 5;
            SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            application.Rewind(earlyOnlineRepaymentTerm);

            //create online bill payment file for customer.. date = loanCreated - 5 days...
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, earlyOnlineRepaymentTerm);

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));
            SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentDateRealignment()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int earlyOnlineRepaymentTerm = 5;
            SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            application.Rewind(earlyOnlineRepaymentTerm);

            //create online bill payment file for customer.. date = loanCreated - 5 days + 2 extra days... i.e file not processed till 7 days into loan
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, earlyOnlineRepaymentTerm);

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));
            SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentRecodredToDbForUnrecognisedCustomer()
        {
            //create online bill payment file for customer.. date = loanCreated - 5 days + 2 extra days... i.e file not processed till 7 days into loan
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file
            //verify file recorded db
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentOverRepaymentIsRecordedToDb()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int earlyOnlineRepaymentTerm = 5;
            SetPaymentFunctions.SetDelayBeforeApplicationClosed(0);

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            application.Rewind(earlyOnlineRepaymentTerm);

            //create online bill payment file for customer.. amount should be greater than balance....
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, earlyOnlineRepaymentTerm);

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));
            SendPaymentFunctions.SendPaymentTakenForRepayLoan(applicationId);

            //verify recorded in db that customer has overpaid... 
        }
    }
}

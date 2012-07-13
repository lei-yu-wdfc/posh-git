using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, Parallelizable(TestScope.All)]
    class FixedTermLoanExtensionLoanCalculation
    {
        private const int InitialLoanTerm = 4;
        private const int ExtendFirstTermDays = 4;
        private const int ExtendSecondTermDays = 4;
        private static Guid _paymentCard;
        private Application _application;
        private bool _isLoanExtended;
        private int _numberOfDaysToCalculateInterest;
        private const decimal LoanExtensionFee = 10.00m;
        private const decimal TransmissionFee = 5.50m;
        private const decimal LoanAmount = 100m;
        private const decimal InterestPerDay = 0.00986301369863m;
        private const int OneDay = 1;
        private dynamic _fixedTermApplication;
        private readonly dynamic _fixedTermLoanApplication = Drive.Data.Payments.Db.FixedTermLoanApplications;
        private readonly dynamic _loanExtensions = Drive.Data.Payments.Db.LoanExtensions;
        private readonly dynamic _loanTransactions = Drive.Data.Payments.Db.Transactions;


        [Pending, Test, AUT(AUT.Uk), JIRA("UkOPS-540")]
        public void FessForMultipleExtensionLoan()
        {

            var customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(customer).WithLoanAmount(LoanAmount).WithLoanTerm(InitialLoanTerm).Build();
            _paymentCard = customer.GetPaymentCard();
            RewindApplicationToMakeDueDayTommorow(InitialLoanTerm - OneDay);
            ExtendLoanAndVerifyPrepayAmount(ExtendFirstTermDays);
            SetLoanExtended();
            RewindApplicationToMakeDueDayTommorow(ExtendFirstTermDays);
            ExtendLoanAndVerifyPrepayAmount(ExtendSecondTermDays);
            RepayLoanAndVerifyLoanAmountPaid();
        }

        private void RewindApplicationToMakeDueDayTommorow(int rewinddays)
        {
            _numberOfDaysToCalculateInterest = rewinddays;
            _application.RewindApplicationDatesForDays(rewinddays);
        }

        private void ExtendLoanAndVerifyPrepayAmount(int extendLoanByDays)
        {

            var paymentAmount = CalculatePrepayAmountUsingDaysAmountFees();
            GetFixedTermApplicationRow();
            Date extendToDate = GetExtendToDateByAddingDaysToCurrentDueDate(extendLoanByDays);
            ExtendFixedTermLoanCommand(extendToDate, paymentAmount);
            VerifyPrePayAmountForLoanExtension(paymentAmount);
        }

        private decimal CalculatePrepayAmountUsingDaysAmountFees()
        {
            var interest = (1 + _numberOfDaysToCalculateInterest * InterestPerDay);
            var totalRepayable = _isLoanExtended ? (LoanAmount + LoanExtensionFee) * interest : (LoanAmount + TransmissionFee) * interest;
            var interstOnly = totalRepayable - LoanAmount;
            return Math.Round(interstOnly, 2);
        }


        private void GetFixedTermApplicationRow()
        {
            _fixedTermApplication =
                _fixedTermLoanApplication.FindAll(_fixedTermLoanApplication.Applications.ExternalId == _application.Id &&
                                                 _fixedTermLoanApplication.ApplicationId ==
                                                 _fixedTermLoanApplication.Applications.ApplicationId).Single();
        }

        private Date GetExtendToDateByAddingDaysToCurrentDueDate(int extendLoanByDays)
        {

            return new Date(_fixedTermApplication.NextDueDate + TimeSpan.FromDays(extendLoanByDays));
        }

        private void ExtendFixedTermLoanCommand(Date fixedTermApplication, Decimal paymentAmount)
        {
            Guid extensionId = Guid.NewGuid();
            Drive.Cs.Commands.Post(new CsExtendFixedTermLoanCommand
            {
                SalesForceUser = "bob@a.com",
                ApplicationId = _application.Id,
                LoanExtensionId = extensionId,
                CV2 = "121",
                ExtensionDate = fixedTermApplication,
                PaymentCardId = _paymentCard,
                PartPaymentAmount = paymentAmount
            });

             Do.Until(
                () =>
                _loanExtensions.FindAll(_loanExtensions.ApplicationId == _fixedTermApplication.ApplicationId &&
                                       _loanExtensions.PartPaymentAmount == paymentAmount).Single());
        }

        private void VerifyPrePayAmountForLoanExtension(decimal paymentAmount)
        {
            paymentAmount = MakeAmountNegative(paymentAmount);
            var loanExtension =
                _loanExtensions.FindAll(_loanExtensions.ApplicationId == _fixedTermApplication.ApplicationID &&
                                       _loanExtensions.PartPaymentAmount == paymentAmount).First();
            Assert.IsTrue(loanExtension.PartPaymentAmount == paymentAmount);

        }

        private void RepayLoanAndVerifyLoanAmountPaid()
        {
            _numberOfDaysToCalculateInterest = ExtendSecondTermDays + OneDay;
            decimal finalRepayToClearLoan = CalculatePrepayAmountUsingDaysAmountFees() + LoanAmount;
            _application.RepayOnDueDate();
            finalRepayToClearLoan = MakeAmountNegative(finalRepayToClearLoan);
            Assert.IsTrue(
                        _loanTransactions.FindAll(_loanTransactions.ApplicationId == _fixedTermApplication.ApplicationID &&
                                                  _loanTransactions.Amount == finalRepayToClearLoan).Single()
                );
        }

        private void SetLoanExtended()
        {
            _isLoanExtended = true;
        }

        private decimal MakeAmountNegative(decimal amount)
        {
            return amount * -1;
        }


    }
}

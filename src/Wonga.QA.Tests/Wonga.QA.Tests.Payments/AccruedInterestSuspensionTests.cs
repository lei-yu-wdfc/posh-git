using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
    public class AccruedInterestSuspensionTests
    {
        private const string InArrearsMaxInterestDaysKey = "Payments.InArrearsMaxInterestDays";
        private const decimal LoanAmount = 100m;
        private const int LoanTerm = 10;
        private const decimal DefaultFees = 20m;
        
        /// <summary>
        /// Transactions db table
        /// </summary>
        private dynamic _transactionsTable;
        private dynamic _serviceConfigTable;
        private dynamic _applicationsTable;
        private dynamic _fixedTermApplicationsTable;

        [SetUp]
        public void Setup()
        {
            _transactionsTable = Drive.Data.Payments.Db.Transactions;
            _serviceConfigTable = Drive.Data.Ops.Db.ServiceConfigurations;
            _fixedTermApplicationsTable = Drive.Data.Payments.Db.FixedTermLoanApplications;
            _applicationsTable = Drive.Data.Payments.Db.Applications;
        }

        [Test,JIRA("UKOPS-414" ), Owner(Owner.PiotrWalat),Pending("Loan calculator bug") ]
        public void GoingIntoArrears_Creates_SuspendInterestTransaction_InTheFuture()
        {
            var  _application = CreateLiveApplication();
            var intMaximumArrearDays = MaximumArrearDays();
            int arrearDays = intMaximumArrearDays + 10;
            _application.PutIntoArrears((uint)arrearDays);
            dynamic suspendTransaction = null;
            dynamic application = null;
            dynamic fixedTermApplication = null;
            

            Do.Until(() => application = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => fixedTermApplication = _fixedTermApplicationsTable.FindBy(ApplicationId: application.ApplicationId));
            DateTime expectedDate = ((DateTime)fixedTermApplication.NextDueDate).AddDays(intMaximumArrearDays);

            Do.Until(() => suspendTransaction = _transactionsTable
                                                    .FindBy(Type: PaymentTransactionEnum.SuspendInterestAccrual,
                                                            ApplicationId: application.ApplicationId, 
                                                            PostedOn: expectedDate));
            Assert.IsNotNull(suspendTransaction);
        }

        [Test, JIRA("UKOPS-414"), Owner(Owner.AnilKrishnamaneni)]
        public void ApplicationsInArrearsMoreThanMaximumDaysHaveSameDueAmount()
        {
            var application = CreateLiveApplication();
            var secondApplication = CreateLiveApplication();
            var intMaximumArrearDays = MaximumArrearDays();
            application.PutIntoArrears((uint)intMaximumArrearDays);
            secondApplication.PutIntoArrears((uint)intMaximumArrearDays + 10);
            var applicationCurrentLoanAmountDue = ApplicationCurrentLoanAmountDue(application);
            var secondApplicationCurrentLoanAmountDue = ApplicationCurrentLoanAmountDue(secondApplication);
            Assert.AreEqual(applicationCurrentLoanAmountDue, secondApplicationCurrentLoanAmountDue);
        }

        [Test, JIRA("UKOPS-414"), Owner(Owner.AnilKrishnamaneni),Pending("Calculator not Working properly for Arrears")]
        public void CheckLoanAmountDueInArrears()
        {
            var application = CreateLiveApplication();
            var fees = Drive.Data.Payments.Db.Products.FindByName("WongaFixedLoan").TransmissionFee;
            var accountSummary = Drive.Api.Queries.Post(new GetAccountSummaryQuery() { AccountId = application.AccountId });
            var currentLoanAmountDue = accountSummary.Values["CurrentLoanAmountDueToday"].Single();
            var amountDueOnDueDateWithoutInterest = ApplicationOperations.GetRepayAmountWithInterest(LoanAmount, LoanTerm) - fees;
            var intMaximumArrearDays = MaximumArrearDays();
            var repayAmount = ApplicationOperations.GetRepayAmountWithInterest(amountDueOnDueDateWithoutInterest, intMaximumArrearDays) + DefaultFees;
            Assert.AreEqual(currentLoanAmountDue, repayAmount.ToString());
        }

        #region Helpers
        private static string ApplicationCurrentLoanAmountDue(Application application)
        {
            var accountSummary = Drive.Api.Queries.Post(new GetAccountSummaryQuery() {AccountId = application.AccountId});
            var applicationCurrentLoanAmountDue = accountSummary.Values["CurrentLoanAmountDueToday"].Single();
            return applicationCurrentLoanAmountDue;
        }

        private static Application CreateLiveApplication()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).WithLoanTerm(LoanTerm).WithLoanAmount(LoanAmount).Build();
            return application;
        }

        private int MaximumArrearDays()
        {
            string maximumArrearDays = _serviceConfigTable.FindBy(Key: InArrearsMaxInterestDaysKey).Value.ToString();
            int intMaximumArrearDays = Int32.Parse(maximumArrearDays);
            return intMaximumArrearDays;
        }
        #endregion
    }
}
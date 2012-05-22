using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    [AUT(AUT.Uk)]
    [Parallelizable(TestScope.All)]
    public class ApplicationStatusHistoryTests
    {
        private dynamic appStatusHistoryRepo = Drive.Data.BiCustomerManagement.Db.ApplicationStatusHistory;
        private Guid _applicationId;
        private Guid _accountId;
        private Customer _customer;
        private Application _application;
        [SetUp]
        public void SetUp()
        {
            _customer = CustomerBuilder.New().Build();
            Do.Until(_customer.GetBankAccount);
            Do.Until(_customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            _application = ApplicationBuilder.New(_customer)
                                             .WithLoanAmount(loanAmount)
                                             .WithLoanTerm(7)
                                             .Build();

            _applicationId = _application.Id;
            _accountId = _application.AccountId;
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1477")]
        [Description("Verifies that when an application status is 'Live' the status history will have entries for the pre-live events")]
        public void Live_Application_Has_PreLive_Status_History()
        {
            //force the application to move to live by sending the IFundsTransferredEvent.
            Drive.Msmq.Payments.Send(new IFundsTransferredEvent { AccountId = _accountId,
                                                                  ApplicationId = _applicationId,
                                                                  TransactionId = Guid.NewGuid() });

            ConfirmStatusHistory(new string[] { "Accepted", "TermsAgreed", "Live" });
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1477")]
        [Description("Verifies that when an application has been paid off the status history will have PaidInFull as the current status.")]
        public void Paid_Application_Has_Paid_In_FullStatus()
        {
            Live_Application_Has_PreLive_Status_History();

            _application.MakeDueToday(); //This will pay off the loan
            
            ConfirmStatusHistory(new string[] { "DueToday", "PaidInFull" });
        }

        //Checks to ensure that each of the supplied status values appears at least once for that application as the current status.
        //There must also be at least one record that has a PreviousStatus with a value of New.
        private void ConfirmStatusHistory(IEnumerable<string> statuses)
        {
            Do.With.Timeout(2).Interval(5).Until(() => appStatusHistoryRepo.FindAll(ApplicationId: _applicationId, PreviousStatus: "New") != null);

            Do.With.Timeout(2).Interval(5).Until(() => statuses.All(stat => { var records = appStatusHistoryRepo.FindAll(appStatusHistoryRepo.ApplicationId == _applicationId &&
                                                                                                                         appStatusHistoryRepo.CurrentStatus == stat);
                                                                              return records.Count() >= 1; }));
        }
    }
}
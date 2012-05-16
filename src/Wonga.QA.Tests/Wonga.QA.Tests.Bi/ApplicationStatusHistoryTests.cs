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
    public class ApplicationStatusHistoryTests
    {
        private dynamic appStatusHistoryRepo = Drive.Data.BiCustomerManagement.Db.ApplicationStatusHistory;
        private Guid _applicationId;
        private Guid _accountId;

        [SetUp]
        public void SetUp()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer)
                                                        .WithLoanAmount(loanAmount)
                                                        .WithLoanTerm(7)
                                                        .Build();

            _applicationId = application.Id;
            _accountId = application.AccountId;
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

            ConfirmStatusHistory(_applicationId, new string[] { "Accepted", "TermsAgreed", "Live" });
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-1477")]
        [Description("Verifies that when an application status is 'Live' the status history will have entries for the pre-live events")]
        public void Due_Application_Has_Due_Status_History()
        {
            Live_Application_Has_PreLive_Status_History();

            Drive.Msmq.Payments.Send(new ILoanIsDueEvent { AccountId = _accountId, 
                                                           ApplicationId = _applicationId, 
                                                           LoanDueDate = DateTime.UtcNow + TimeSpan.FromDays(7) });

            ConfirmStatusHistory(_applicationId, new string[] { "DueToday" });
        }

        //Checks to ensure that each of the supplied status values appears at least once for that application as the current status.
        //There must also be at least one record that has a PreviousStatus with a value of New.
        private void ConfirmStatusHistory(Guid appId, IEnumerable<string> statuses)
        {
            Do.With.Timeout(2).Interval(5).Until(() => appStatusHistoryRepo.FindAll(ApplicationId: appId, PreviousStatus: "New") != null);

            Do.With.Timeout(2).Interval(5).Until(() => statuses.All(stat => { var records = appStatusHistoryRepo.FindAll(appStatusHistoryRepo.ApplicationId == appId &&
                                                                                                                         appStatusHistoryRepo.CurrentStatus == stat);
                                                                              return records.Count() >= 1; }));
        }
    }
}
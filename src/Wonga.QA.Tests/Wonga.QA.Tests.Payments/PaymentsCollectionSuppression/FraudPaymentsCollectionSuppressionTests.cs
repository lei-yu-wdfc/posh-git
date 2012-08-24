using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
    [TestFixture]
    [Parallelizable(TestScope.All), Pending("Depends on salesforce tc so will not run reliably on RC")]
    [AUT(AUT.Uk)]
    public class FraudPaymentsCollectionSuppressionTests
    {
        private Customer _customer;
        private Application _application;

        private dynamic _paymentsSuppressionsTable;
        private dynamic _applicationsTable;

        [SetUp]
        public void Setup()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer).Build();

            _paymentsSuppressionsTable = Drive.Data.Payments.Db.PaymentCollectionSuppressions;
            _applicationsTable = Drive.Data.Payments.Db.Applications;
        }

        [Test, Owner(Owner.PiotrWalat)]
        public void Application_NotFraudConfirmed_UnsuppressesPayments()
        {
            Guid newCaseID = Guid.NewGuid();

            //Suspect fraud through cs api
            var suspectFraudCommand = new SuspectFraudCommand()
            {
                AccountId = _customer.Id,
                CaseId = newCaseID
            };

            Drive.Cs.Commands.Post(suspectFraudCommand);
            dynamic suppression = null;

            dynamic app = null;
            Do.Until(() => app = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => suppression = _paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId,
                FraudSuppression: true));
            var confirmNotFraudCommand = new ConfirmNotFraudCommand()
                                             {
                                                 AccountId = _customer.Id,
                                                 CaseId = newCaseID
                                             };
            Drive.Cs.Commands.Post(confirmNotFraudCommand);
            Do.Until(() => suppression = _paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId,
                FraudSuppression:false));
        }

        [Test]
        public void Application_FraudConfirmed_SuppressesPayments()
        {
            Guid newCaseID = Guid.NewGuid();

            //Suspect fraud through cs api
            var suspectFraudCommand = new SuspectFraudCommand()
            {
                AccountId = _customer.Id,
                CaseId = newCaseID
            };

            Drive.Cs.Commands.Post(suspectFraudCommand);

            dynamic suppression = null;
            dynamic app = null;
            Do.Until(() => app = _applicationsTable.FindBy(ExternalId: _application.Id));
            Do.Until(() => suppression = _paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId, FraudSuppression: true));

            var confirmFraudCommand = new ConfirmFraudCommand()
            {
                AccountId = _customer.Id,
                CaseId = newCaseID
            };
            Drive.Cs.Commands.Post(confirmFraudCommand);
            Do.Until(() => suppression = _paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId, FraudSuppression: true));
        }

		[Test, AUT(AUT.Uk), Pending]
		public void ShouldNotTakeManualPayment_WhenPaymentCollectionsAreSuppressedDueToSuspectedFraud()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build().PutIntoArrears();

			var applicationId = Drive.Data.Payments.Db.Applications.FindByExternalId(application.Id).ApplicationId;

			//Suspect fraud through cs api
			Drive.Cs.Commands.Post(new SuspectFraudCommand
			{
				AccountId = customer.Id,
				CaseId = Guid.NewGuid()
			});

			dynamic app = null;
			Do.Until(() => app = _applicationsTable.FindBy(ExternalId: application.Id));
			Do.Until(() =>  _paymentsSuppressionsTable.FindBy(ApplicationId: app.ApplicationId, FraudSuppression: true));

			Drive.Cs.Commands.Post(new TakePaymentManualCommand
			{
				Amount = 100.00m,
				ApplicationId = application.Id,
				Currency = CurrencyCodeEnum.GBP,
				PaymentCardId = customer.GetPaymentCard(),
				PaymentId = Guid.NewGuid(),
				SalesforceUser = "test.user@wonga.com"
			});

			var paymentRequests = Drive.Data.Payments.Db.PaymentCardRepaymentRequests;

			Do.Until(() => paymentRequests.FindAll(paymentRequests.ApplicationId == applicationId &&
												   paymentRequests.StatusDescription == "PaymentCollectionsSuppressed"));
		}
    }
}

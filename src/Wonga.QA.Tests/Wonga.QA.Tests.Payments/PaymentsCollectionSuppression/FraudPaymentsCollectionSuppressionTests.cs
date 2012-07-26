using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments.PaymentsCollectionSuppression
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
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
    }
}

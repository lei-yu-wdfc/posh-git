using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
    public class InArrearsNoticeTests
    {
        private Guid _accountId;
        private Application _application;
        private string _savedBankGatewayTestMode;
        private const string BankgatewayIstestmode = "BankGateway.IsTestMode";        
        
        [SetUp]
        public void Setup()
        {
            var customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            _application = ApplicationBuilder.New(customer).Build();
            _accountId = customer.Id;

            _savedBankGatewayTestMode = Drive.Db.GetServiceConfiguration(BankgatewayIstestmode).Value;
            ConfigurationFunctions.SetBankGatewayTestMode(false);
        }

        [TearDown]
        public void TearDown()
        {
            Drive.Db.SetServiceConfiguration(BankgatewayIstestmode, _savedBankGatewayTestMode);
        }

        [Test, JIRA("ZA-2099")]
        public void ApplicationInArrearsSagaCreatedTest()
        {
            _application.PutApplicationIntoArrears(20);
            //check if saga created.
            var saga =
                Do.Until(() => Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Single(e => e.AccountId == _accountId));
            Assert.AreEqual(0, saga.DaysInArrears);
        }

        [Test, JIRA("ZA-2099")]
        public void ApplicationClosedSagaCompleteTest()
        {
            _application.PutApplicationIntoArrears(20);
            //check if saga created.
            var saga =
                Do.Until(() => Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Single(e => e.AccountId == _accountId));
            Assert.AreEqual(0, saga.DaysInArrears);

            //staff message for application is closed.
            Drive.Msmq.Payments.Send(new IApplicationClosedEvent()
                                         {
                                             ApplicationId = _application.Id
                                         });
            Do.Until(() => ! Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Any(e => e.AccountId == _accountId));
        }
    }
}

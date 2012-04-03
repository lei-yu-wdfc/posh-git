using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Cs
{
    [Parallelizable(TestScope.All)]
    public class CsApiTests
    {
        private bool _bankGatewayTestModeOriginal;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
            ConfigurationFunctions.SetBankGatewayTestMode(false);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
        }

        [Test]
        public void GetScheduledPaymentsTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();

            application.PutApplicationIntoArrears(20);

            var response = Drive.Cs.Queries.Post(new GetScheduledPaymentsQuery
                                {
                                    ApplicationGuid = application.Id
                                });
            Assert.IsTrue(response.Body.Contains(application.Id.ToString()));
        }
    }
}

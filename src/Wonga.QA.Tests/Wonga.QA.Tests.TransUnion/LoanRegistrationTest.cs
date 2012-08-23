using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Comms.Helpers;
using Wonga.QA.Tests.Core;
using Assert = MbUnit.Framework.Assert;

namespace Wonga.QA.Tests.TransUnion
{
    [TestFixture, Parallelizable(TestScope.Descendants), AUT(AUT.Za)] //Can be only on level 3 because it changes configuration
    public class LoanRegistrationTest
    {
        private Customer _customer;
        private Application _application;

        private bool _bankGatewayTestModeOriginal;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
            ConfigurationFunctions.SetBankGatewayTestMode(true);
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
        }

        [Test, AUT(AUT.Za), JIRA("ZA-2766")]
        public void LoanOpenTest()
        {
            _customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(_customer).Build();

            dynamic regEntry = Drive.Data.TransUnion.Db.LoanRegistration.FindByApplicationGuid(_application.Id);

            Assert.IsNotNull(regEntry);
            Assert.AreEqual(_application.Id, (Guid)regEntry.ApplicationGuid);
        }

        [Test, AUT(AUT.Za), JIRA("ZA-2766"), DependsOn(@"LoanOpenTest")]
        public void loanCloseTest()
        {
            _application.RepayOnDueDate();
            var entrys = Drive.Data.TransUnion.Db.LoanRegistration.FindAllByApplicationGuid(_application.Id)
                .OrderByCreationDate().ToList();

            Assert.AreEqual(2, entrys.Count);
        }
    }
}

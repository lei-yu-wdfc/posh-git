using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.UI;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.MigrationTests.V2Selenium.Pages;
using Wonga.QA.Framework;
using System.Threading;

namespace Wonga.QA.MigrationTests
{
    class ScorecardTesting
    {
        private UiClient _client;
        private const String V2FrontEndUrl = "http://www.wonga.com";

        [SetUp]
        public void SetUp()
        {
            _client= new UiClient();
            _client.Driver.Navigate().GoToUrl(V2FrontEndUrl);
            var homepage = new V2HomePage(_client);

            homepage.EmailTextField.Clear();
            homepage.PasswordTextField.Clear();

            Thread.Sleep(1000);
            homepage.EmailTextField.SendValue("dsdsdsds");
            homepage.PasswordTextField.SendValue("dkjhskjds");

        }

        [Test]
        [Pending]
        public void TestV2LnSeleniumJourney()
        {
            
        }

        private void GivenAListOfExistingUsersEmails()
        {
            //Parse the txt file and get the emails
        }

        private void ICreateAListOfCustomersForBothSystems()
        {
            //Given the list of email => create customers
        }

        private void AndDoV2LnSeleniumJourney()
        {
            //Run V2
        }

        private void AndDoV3LnApiJourney()
        {
            //Run V3
        }
    }
}

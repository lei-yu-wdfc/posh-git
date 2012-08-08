using System.Collections.Generic;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Helpers
{
    [TestFixture]
    [Description("Prepare L0, Ln users for various testing purposes")]
    [Explicit("The tests are to be launched manually only - to prepare test data (L0/Ln users)")] 
    public class DataPreparationHelpers
    {
        private Dictionary<string, string> _users = new Dictionary<string, string> { };
        
        [FixtureSetUp]
        public void SetUp()
        {
            
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), Owner(Owner.StanDesyatnikov)]
        public void CreateL0User()
        {
            Customer customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            Assert.IsFalse(application.IsClosed);
            _users.Add("L0_User", customer.Email);
        }

        [Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), Owner(Owner.StanDesyatnikov)]
        public void CreateLnUser()
        {
            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            application.RepayOnDueDate();
            Assert.IsTrue(application.IsClosed);
            _users.Add("Ln_User", customer.Email);
        }

        [FixtureTearDown]
        private void TearDown()
        {
            // Output the emails of the created users
            Debug.WriteLine("**************");
            foreach (var user in _users)
            {
                Debug.WriteLine("{0}:{1}", user.Key, user.Value);
            }
            Debug.WriteLine("**************");
        }
    }
}

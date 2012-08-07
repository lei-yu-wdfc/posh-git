using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [Parallelizable(TestScope.All), JIRA("UKWEB-343", "UKWEB-373"), AUT(AUT.Uk)]
    public class HomePageL0Tests : UiTest
    {
        private string _emailL0;
        private string _url;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _emailL0 = Get.RandomEmail();
            Console.WriteLine("Email={0}", _emailL0);
        }

        [Test, JIRA("UKWEB-370"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void HomePage_NewUser()
        {
            var homePage = Client.Home();
            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            _url = Client.Home().Url;

            Assert.AreEqual("Welcome to Wonga. We can deposit up to £400 in your bank account by " +
                            DateTime.Now.AddMinutes(23).ToShortTimeString() +
                            homePage.GetWelcomeMessageDay(),
                            homePage.GetWelcomeHeaderMessageText(),
                            "The Welcome text is wrong");
            Assert.AreEqual("Existing customers may be able to borrow up to £1,000, depending on your current trust rating.",
                            homePage.GetWelcomeSubMessageText(),
                            "Welcome submessage is wrong");
            Assert.AreEqual(_url + "login", homePage.GetExistingCustomersLink());                   //Ensure 'Existing Customers' link has correct URL
            Assert.AreEqual(_url + "money/about-trust", homePage.GetAboveSlidersTrustRatingLink()); //Ensure 'Trust rating' link above sliders has correct URL
            // TODO: Change AddMinutes(23) to AddMinutes(24) when the code is fixed
        }
    }
}
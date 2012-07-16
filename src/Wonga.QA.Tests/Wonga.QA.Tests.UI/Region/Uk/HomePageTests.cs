using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Region.Uk
{
    [Parallelizable(TestScope.Descendants)]
    public class HomePageTests: UiTest
    {
        private string _email;
        private string _firsName;
        //private string _time;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _email = Get.RandomEmail();
            _firsName = Get.GetName();
            Console.WriteLine("email={0}, first name={1}", _email, _firsName);

            Customer customer = CustomerBuilder.New().WithEmailAddress(_email).WithForename(_firsName).Build();
            ApplicationBuilder.New(customer).Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void HomePagePersonalisedNewUserTest()
        {
            var homePage = Client.Home();
            Assert.AreEqual("Welcome to Wonga", homePage.Headers[1], "The Header should be 'Welcome to Wonga'"); // user hasn't logged in before
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-344", "UKWEB-345"), MultipleAsserts, Owner(Owner.OrizuNwokeji), Pending("In development")]
        public void HomePageRefactorTest()
        {
            var homePage = Client.Home();

            Assert.IsTrue(homePage.IsNewBodyFrameworkExists());
            var awards = homePage.GetPromoBoxes();
            var seoLinks = homePage.GetSeoLinks();
            var promoBoxes = homePage.GetPromoBoxes();

            Assert.IsTrue(homePage.Source.Contains(promoBoxes));
            Assert.IsTrue(homePage.Source.Contains(awards));
            Assert.IsTrue(homePage.Source.Contains(seoLinks));
        }
        
        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development, and waiting for functionality"), DependsOn("HomePagePersonalisedNewUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0HomePagePersonalisedLoggedInUserTest()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var homePage = Client.Home();
            //Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here)", homePage.Headers[1]); // user has logged in
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development, and waiting for functionality"), DependsOn("HomePagePersonalisedLoggedInUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0HomePagePersonalisedCookiedUserTest()
        {
            var homePage2 = Client.Home();
            //Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here)", homePage.Headers[1]); // user has being cookied
        }

        /*[Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development, and waiting for functionality"), DependsOn("HomePagePersonalisedNewUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnHomePagePersonalisedLoggedInUserTest()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
               .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
               .WithEmail(_email);

            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var homePage3 = Client.Home();
            //Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here)", homePage.Headers[1]); // user has logged in
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development, and waiting for functionality"), DependsOn("HomePagePersonalisedLoggedInUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnHomePagePersonalisedCookiedUserTest()
        {
            var homePage4 = Client.Home();
            //Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here)", homePage.Headers[1]); // user has being cookied
        } */

        /*
            _time = DateTime.Now.AddMinutes(24).ToShortTimeString();
            Console.WriteLine("time={0}", _time);
            //Console.WriteLine("email={0}", _email);

            //var page = homePage.Login.LoginAs(_email, Get.GetPassword());
            //Console.WriteLine("Trust Rating={0:0.00}", trustRating);
         */
    }
}

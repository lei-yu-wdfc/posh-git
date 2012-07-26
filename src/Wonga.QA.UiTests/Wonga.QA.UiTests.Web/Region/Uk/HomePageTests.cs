using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
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

            // User hasn't logged in before. Verify the Welcome message
            Assert.AreEqual("Welcome to Wonga. We can deposit up to £400 in your bank account by " + DateTime.Now.AddMinutes(23).ToShortTimeString() + homePage.GetWelcomeMessageDay(), 
                            homePage.GetWelcomeHeaderMessageText(), "The Header should be 'Welcome to Wonga'");

            // Verify Sub message
            Assert.AreEqual("Existing customers may be able to borrow up to £1,000, depending on your current trust rating.", homePage.GetWelcomeSubMessageText(), 
                            "The Header should be 'Welcome to Wonga'");
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), MultipleAsserts, Pending("Test in development"), Owner(Owner.PavithranVangiti)]
        public void HomePageLinksTest()
        {
            var homePage = Client.Home();
            //Ensure 'Code of practice' link has correct URL
            Assert.AreEqual("node/495", homePage.GetHomePageCodeOfPracticeLink());
            Console.WriteLine(homePage.GetHomePageCodeOfPracticeLink());

            //Ensure 'Trust rating' link has correct URL
            Assert.AreEqual("/trust-rating", homePage.GetHomePageTrustRatingLink());
            Console.WriteLine(homePage.GetHomePageTrustRatingLink());

            //Ensure 'APR' link has correct URL
            Assert.AreEqual("/apr", homePage.GetHomePageAPRLink());
            Console.WriteLine(homePage.GetHomePageAPRLink());

            //Ensure 'Contact us' link has correct URL
            Assert.AreEqual("/contact", homePage.GetHomePageContactUsLink());
            Console.WriteLine(homePage.GetHomePageContactUsLink());

            //Ensure 'Privacy policy' link has correct URL
            Assert.AreEqual("/privacy-policy", homePage.GetHomePagePrivacyPolicyLink());
            Console.WriteLine(homePage.GetHomePagePrivacyPolicyLink());

            //Ensure VeriSign image link has correct URL
            Assert.AreEqual("https://sealinfo.verisign.com/splash?form_file=fdf/splash.fdf&dn=WWW.WONGA.COM&lang=en", homePage.GetHomePageVeriSignLink());
            Console.WriteLine(homePage.GetHomePageVeriSignLink());

            //Ensure FLA image link has correct URL
            Assert.AreEqual("http://www.fla.org.uk/fla/", homePage.GetHomePageFLALink());
            Console.WriteLine(homePage.GetHomePageFLALink());

            //Ensure KIVA image link has correct URL
            Assert.AreEqual("http://www.wonga.com/money/kiva/", homePage.GetHomePageKIVALink());
            Console.WriteLine(homePage.GetHomePageKIVALink());

            //Ensure Media Guardian Awards image link has correct URL
            Assert.AreEqual("http://www.guardian.co.uk/megas/winners-2011", homePage.GetHomePageMediaGuardianAwardsLink());
            Console.WriteLine(homePage.GetHomePageMediaGuardianAwardsLink());

            //Ensure Webby Awards image link has correct URL
            Assert.AreEqual("http://www.webbyawards.com", homePage.GetHomePageWebbyAwardsLink());
            Console.WriteLine(homePage.GetHomePageWebbyAwardsLink());

            //Ensure Consumer Credit Awards image link has correct URL
            Assert.AreEqual("http://www.ccr-interactive.co.uk/index.php?option=com_content&task=view&id=1401&Itemid=111", homePage.GetHomePageCCRCreditAwardsLink());
            Console.WriteLine(homePage.GetHomePageCCRCreditAwardsLink());

            //Ensure Tech Track 100 image link has correct URL
            Assert.AreEqual("http://www.fasttrack.co.uk/fasttrack/leagues/dbDetails.asp?siteID=3&compID=3164&yr=2011", homePage.GetHomePageTechTrack100Link());
            Console.WriteLine(homePage.GetHomePageTechTrack100Link());

            //Ensure Payday Loans Alternative link has correct URL
            Assert.AreEqual("http://www.wonga.com/money/payday-loans-alternative/", homePage.GetHomePagePaydayLoansLink());
            Console.WriteLine(homePage.GetHomePagePaydayLoansLink());

            //Ensure Quick Loan link has correct URL
            Assert.AreEqual("http://www.wonga.com/money/wonga-quick-loan/", homePage.GetHomePageQuickLoanLink());
            Console.WriteLine(homePage.GetHomePageQuickLoanLink());

            //Ensure Cash Loan link has correct URL
            Assert.AreEqual("http://www.wonga.com/money/wonga-cash-loan/", homePage.GetHomePageCashLoanLink());
            Console.WriteLine(homePage.GetHomePageCashLoanLink());
        }
        [Test, AUT(AUT.Uk), JIRA("UKWEB-344", "UKWEB-345"), MultipleAsserts, Owner(Owner.OrizuNwokeji), Pending("Test in development. Code in development.")]
        public void HomePageRefactorTest()
        {
            var homePage = Client.Home();

            Assert.IsTrue(homePage.IsNewBodyFrameworkExists());
            var awards = homePage.GetAwardsList();
            var seoLinks = homePage.GetSeoLinks();
            var promoBoxes = homePage.GetPromoBoxes();

            Assert.IsTrue(homePage.Source.Contains(promoBoxes), "Promo Boxes not found");
            Assert.IsTrue(homePage.Source.Contains(awards), "Awards not found");
            Assert.IsTrue(homePage.Source.Contains(seoLinks), "SEO Links not found");
        }
        
        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development. Code in development."), DependsOn("HomePagePersonalisedNewUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0HomePagePersonalisedLoggedInUserTest()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var homePage = Client.Home();
            // user has logged in
            Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + 
                "? click here) We can deposit up to £400 in your bank account by " + DateTime.Now.AddMinutes(23).ToShortTimeString()
                + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText()); 
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development. Code in development."), DependsOn("HomePagePersonalisedLoggedInUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0HomePagePersonalisedCookiedUserTest()
        {
            var homePage = Client.Home();
            Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here)", homePage.GetWelcomeHeaderMessageText()); // user has being cookied
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development, and waiting for functionality"), DependsOn("HomePagePersonalisedNewUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnHomePagePersonalisedLoggedInUserTest()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
               .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
               .WithEmail(_email);

            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var homePage = Client.Home();
            Console.WriteLine(_email);
            Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here) We can deposit up to " +  " in your bank account by " + DateTime.Now.AddMinutes(23).ToShortTimeString() + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText()); // user has logged in
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development, and waiting for functionality"), DependsOn("HomePagePersonalisedLoggedInUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnHomePagePersonalisedCookiedUserTest()
        {
            var homePage = Client.Home();
            //Assert.AreEqual("Welcome back " + _firsName + "! (not " + _firsName + "? click here)", homePage.Headers[1]); // user has being cookied
        }

        /*
            _time = DateTime.Now.AddMinutes(24).ToShortTimeString();
            Console.WriteLine("time={0}", _time);
            //Console.WriteLine("email={0}", _email);

            //var page = homePage.Login.LoginAs(_email, Get.GetPassword());
            //Console.WriteLine("Trust Rating={0:0.00}", trustRating);
         */
    }
}

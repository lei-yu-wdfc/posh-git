using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.Descendants), JIRA("UKWEB-343", "UKWEB-373")]
    public class HomePageTests: UiTest
    {
        private string _email;
        private string _fullFirstName;
        private string _url;
        private string _truncatedFirstName;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _email = Get.RandomEmail();
            _fullFirstName = Get.RandomString(20);
            _truncatedFirstName = _fullFirstName.Remove(15);
            Console.WriteLine("Email={0}, First Name={1}, Trancated First Name={2}", _email, _fullFirstName, _truncatedFirstName);

            Customer customer = CustomerBuilder.New().WithEmailAddress(_email).WithForename(_fullFirstName).Build();
            ApplicationBuilder.New(customer).Build();
        }

        #region L0_HomePage_Personalised

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void HomePagePersonalisedNewUserTest()
        {
            var homePage = Client.Home();

            // User hasn't logged in before. Verify the Welcome message
            Assert.AreEqual("Welcome to Wonga. We can deposit up to £400 in your bank account by " + DateTime.Now.AddMinutes(24).ToShortTimeString() + homePage.GetWelcomeMessageDay(),
                            homePage.GetWelcomeHeaderMessageText(), "The Wellcome text is wrong");

            // Verify Sub message
            Assert.AreEqual("Existing customers may be able to borrow up to £1,000, depending on your current trust rating.", homePage.GetWelcomeSubMessageText(),
                            "The Header should be 'Welcome to Wonga'");

            Assert.IsNotNull(homePage);

            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            // TODO: UKWEB-371: Check Navigation Header (international-trigger, help-trigger, social-trigger, login-trigger)
            // UKWEB-371: Navigation Header is partially checked when object homePage is created.
            // TODO: UKWEB-371: Apart from checking that the navigation header on the Home page is displayed, also check the elements’ (International, Social, Help, Login) behaviour and content.
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development. Code in development."), DependsOn("HomePagePersonalisedNewUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0HomePagePersonalisedLoggedInUserTest()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var homePage = Client.Home();

            // user has logged in
            Assert.AreEqual("Welcome back " + _truncatedFirstName + "...! (not " + _truncatedFirstName +
                            "...? click here) We can deposit up to £400 in your bank account by " + DateTime.Now.AddMinutes(23).ToShortTimeString()
                            + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText());

            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            // TODO: 1. Log out and check the home page opens 2. Log in again to create cookies for L0HomePagePersonalisedCookiedUserTest
            // TODO: Change AddMinutes(23) to AddMinutes(24) when the code is fixed
            // TODO: UKWEB-1072: When users clicks "click here" in the Welcome message on the Homepage, as a recognised or logged in user, the Homepage should open.
            // TODO: UKWEB-371: Check "Welcome <15-symbolsTrancatedFirstName> Logout" in the Navigation Header
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development. Code in development."), DependsOn("HomePagePersonalisedLoggedInUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void L0HomePagePersonalisedCookiedUserTest()
        {
            var homePage = Client.Home();
            Assert.AreEqual("Welcome back " + _truncatedFirstName + "...! (not " + _truncatedFirstName + "...? click here)", homePage.GetWelcomeHeaderMessageText()); // user has being cookied

            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            // TODO: Check "If you're not <15-symbolsTrancatedFirstName>, click here."
            // TODO: 1. Click on "click here" in the Navigation Header. 2. Check that the Login page opens and the user is logged out.
            // TODO: UKWEB-1072: When users clicks "click here" in the Welcome message on the Homepage, as a recognised or logged in user, the Homepage should open.
            // TODO: UKWEB-371: Check Navigation Header (international-trigger, help-trigger, social-trigger, login-trigger, click-here)
            // TODO: UKWEB-371: Apart from checking that the navigation header on the Home page is displayed, also check the elements’ (International, Social, Help, Login) behaviour and content.
        }

        #endregion

        #region Ln_HomePage_Personalised

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development. Code in development."), DependsOn("HomePagePersonalisedNewUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnHomePagePersonalisedLoggedInUserTest()
        {
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
               .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
               .WithEmail(_email);

            var loginPage = Client.Login();
            loginPage.LoginAs(_email);
            var homePage = Client.Home();
            Console.WriteLine(_email);
            Assert.AreEqual("Welcome back " + _truncatedFirstName + "...! (not " + _truncatedFirstName + "...? click here) We can deposit up to " + " in your bank account by " + DateTime.Now.AddMinutes(23).ToShortTimeString() + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText()); // user has logged in

            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            // TODO: Change AddMinutes(23) to AddMinutes(24) when the code is fixed
            // TODO: UKWEB-371: Check Navigation Header (international-trigger, help-trigger, social-trigger, login-trigger, #logout)
            // Check "Welcome <15-symbolsTrancatedFirstName> Logout"
            // TODO: UKWEB-371: Apart from checking that the navigation header on the Home page is displayed, also check the elements’ (International, Social, Help, Login) behaviour and content.
            // TODO: UKWEB-1072: When users clicks "click here" in the Welcome message on the Homepage, as a recognised or logged in user, the Homepage should open.
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), Pending("Test in development. Code in development."), DependsOn("HomePagePersonalisedLoggedInUserTest"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void LnHomePagePersonalisedCookiedUserTest()
        {
            var homePage = Client.Home();
            //Assert.AreEqual("Welcome back " + _truncatedFirstName + "...! (not " + _truncatedFirstName + "...? click here)", homePage.Headers[1]); // user has being cookied

            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            // TODO: UKWEB-371: Check Navigation Header (international-trigger, help-trigger, social-trigger, login-trigger, click-here)
            // Check "If you're not <15-symbolsTrancatedFirstName>, click here."
            // TODO: UKWEB-371: Apart from checking that the navigation header on the Home page is displayed, also check the elements’ (International, Social, Help, Login) behaviour and content.
            // TODO: UKWEB-1072: When users clicks "click here" in the Welcome message on the Homepage, as a recognised or logged in user, the Homepage should open.
        }

        #endregion

        #region Miscellaneous_Homepage_Tests

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), MultipleAsserts, Pending("Test in development"), Owner(Owner.PavithranVangiti)]
        public void HomePageLinksTest()
        {
            var homePage = Client.Home();
            _url = Client.Home().Url;

            //Ensure 'Code of practice' link has correct URL
            Assert.AreEqual(_url + "node/495", homePage.GetHomePageCodeOfPracticeLink());
            Console.WriteLine(homePage.GetHomePageCodeOfPracticeLink());

            //Ensure 'Trust rating' link has correct URL
            Assert.AreEqual(_url + "trust-rating", homePage.GetHomePageTrustRatingLink());
            Console.WriteLine(homePage.GetHomePageTrustRatingLink());

            //Ensure 'APR' link has correct URL
            Assert.AreEqual(_url + "apr", homePage.GetHomePageAPRLink());
            Console.WriteLine(homePage.GetHomePageAPRLink());

            //Ensure 'Contact us' link has correct URL
            Assert.AreEqual(_url + "contact", homePage.GetHomePageContactUsLink());
            Console.WriteLine(homePage.GetHomePageContactUsLink());

            //Ensure 'Privacy policy' link has correct URL
            Assert.AreEqual(_url + "privacy-policy", homePage.GetHomePagePrivacyPolicyLink());
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
            Assert.AreEqual("http://www.webbyawards.com/", homePage.GetHomePageWebbyAwardsLink());
            Console.WriteLine(homePage.GetHomePageWebbyAwardsLink());

            //Ensure Consumer Credit Awards image link has correct URL
            Assert.AreEqual("http://www.ccr-interactive.co.uk/index.php?option=com_content&task=view&id=1401&Itemid=111", homePage.GetHomePageCCRCreditAwardsLink());
            Console.WriteLine(homePage.GetHomePageCCRCreditAwardsLink());

            //Ensure Tech Track 100 image link has correct URL
            Assert.AreEqual("http://www.fasttrack.co.uk/fasttrack/leagues/dbDetails.asp?siteID=3&compID=3164&yr=2011", homePage.GetHomePageTechTrack100Link());
            Console.WriteLine(homePage.GetHomePageTechTrack100Link());

            //Ensure Payday Loans Alternative link has correct URL
            Assert.AreEqual(_url + "content/payday-loans-alternative", homePage.GetHomePagePaydayLoansLink());
            Console.WriteLine(homePage.GetHomePagePaydayLoansLink());

            //Ensure Quick Loan link has correct URL
            Assert.AreEqual(_url + "content/quick-loan", homePage.GetHomePageQuickLoanLink());
            Console.WriteLine(homePage.GetHomePageQuickLoanLink());

            //Ensure Cash Loan link has correct URL
            Assert.AreEqual(_url + "content/cash-loan", homePage.GetHomePageCashLoanLink());
            Console.WriteLine(homePage.GetHomePageCashLoanLink());
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-373"), MultipleAsserts, Pending("Test in development.  Code in development."), Owner(Owner.PavithranVangiti)]
        public void HomePageRepresentativeExampleTest()
        {
            var homePage = Client.Home();

            //Ensure 'Representative example' tool tip has the correct text
            Assert.AreEqual("Representative example:" +
                            "Amount of credit £207 for 20 days" +
                            "Total amount payable £254.42" +
                            "Interest £41.92" +
                            "Interest rate 360% pa (fixed)" +
                            "Transmission fee £5.50" +
                            "Representative APR 4212%", homePage.GetRepresentativeExampleText());
            Console.WriteLine("Representative Example text is: " + homePage.GetRepresentativeExampleText());
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-229"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void HomePageContentSlotsTest()
        {
            var homePage = Client.Home();
            _url = Client.Home().Url;

            //Ensure correct title is present in first content slot in home page
            Assert.AreEqual("Betty, Earl and Joyce show how wonga.com's short term cash loans put you in control", homePage.GetContentSlot1Title());
            Console.WriteLine("Content Slot1 title is: " + homePage.GetContentSlot1Title());

            //Ensure correct text is displayed in 'Wonga customers' box in home page
            Assert.AreEqual("Populus survey of 2012 with over 25,000 respondents", homePage.GetWongaCustomersBoxText());
            Console.WriteLine("Text in Wonga customers box: " + homePage.GetWongaCustomersBoxText());

            //Ensure correct text is displayed in 'Responsible lending' box in home page
            Assert.AreEqual("Responsible lending", homePage.GetResponsibleLendingBoxText());
            Console.WriteLine("Responsible lending box heading: " + homePage.GetResponsibleLendingBoxText());

            // TODO: UKWEB-229: Check the content slots post login
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

        #endregion

    }
}

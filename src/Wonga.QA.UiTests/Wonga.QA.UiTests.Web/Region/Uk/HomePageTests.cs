using System;
using MbUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Testing.Attributes;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk
{
    [Parallelizable(TestScope.Descendants), JIRA("UKWEB-343", "UKWEB-373")]
    // TODO: UKWEB-371: Apart from checking that the navigation header on the Home page is displayed, also check the elements’ (International, Social, Help, Login) behaviour and content.

    #region L0_HomePage_Personalised

    public class HomePageL0Tests: UiTest
    {
        private string _emailL0;
        private string _fullFirstNameL0;
        private string _url;
        private string _truncatedFirstNameL0;
        private Cookie _userCookie;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            _emailL0 = Get.RandomEmail();
            _fullFirstNameL0 = Get.RandomString(20);
            _truncatedFirstNameL0 = _fullFirstNameL0.Remove(15);
            Console.WriteLine("Email={0}, First Name={1}, Trancated First Name={2}", _emailL0, _fullFirstNameL0, _truncatedFirstNameL0);

            Customer customerL0 = CustomerBuilder.New().WithEmailAddress(_emailL0).WithForename(_fullFirstNameL0).Build();
            ApplicationBuilder.New(customerL0).WithLoanAmount(100).Build();
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        //[Pending("Test in development. Code in development.")]
        public void HomePage_NewUser()
        {
            var homePage = Client.Home();

            //Ensure 'Max available credit' value is correct
            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            _url = Client.Home().Url;

            // User hasn't logged in before. Verify the Welcome message
            Assert.AreEqual("Welcome to Wonga. We can deposit up to £400 in your bank account by " + DateTime.Now.AddMinutes(23).ToShortTimeString() + homePage.GetWelcomeMessageDay(),
                            homePage.GetWelcomeHeaderMessageText(), "The Welcome text is wrong");

            // Verify Sub message
            Assert.AreEqual("Existing customers may be able to borrow up to £1,000, depending on your current trust rating.", homePage.GetWelcomeSubMessageText(),
                            "The Header should be 'Welcome to Wonga'");

            //Ensure 'Existing Customers' link has correct URL
            Assert.AreEqual(_url + "login", homePage.GetExistingCustomersLink());

            //Ensure 'Trust rating' link above sliders has correct URL
            Assert.AreEqual(_url + "money/about-trust", homePage.GetAboveSlidersTrustRatingLink());

            // TODO: Change AddMinutes(23) to AddMinutes(24) when the code is fixed
            // TODO: UKWEB-371: Check Navigation Header (international-trigger, help-trigger, social-trigger, login-trigger)
            // UKWEB-371: Navigation Header is partially checked when object homePage is created.
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370", "UKWEB-371"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        [IgnorePageErrors]//[Pending("Test in development. Code in development.")]
        public void L0_HomePage_LoggedInUser()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_emailL0);

            // Save user's cookie
            _userCookie = Client.Driver.Manage().Cookies.GetCookieNamed("wonga_visitor");

            var homePage = Client.Home();

            var expectedWelcomeMessage = "Welcome back " + _truncatedFirstNameL0 + "...! (not " + _truncatedFirstNameL0 +
                                        "...?) click hereWe can deposit up to £300 in your bank account by " + DateTime.Now.AddMinutes(24).ToShortTimeString()
                                        + homePage.GetWelcomeMessageDay();
            // user has logged in
            Assert.AreEqual(expectedWelcomeMessage, homePage.GetWelcomeHeaderMessageText());
            Assert.AreEqual("300", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong."); //Ensure maximum available credit is displayed correctly
            Assert.AreEqual("Welcome " + _truncatedFirstNameL0 + "... Logout", homePage.GetHeaderBarText(), "Header bar text is wrong."); //Check "Welcome <15-symbolsTruncatedFirstName> Logout" in the Navigation Header

            //Esnure 'click here' link in welcome message takes User back to home page.
            homePage.ClickWelcomeMessageClickHereLink();
            homePage = new HomePage(this.Client);
            Assert.IsFalse(homePage.IsHeaderBarTextVisible());
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), DependsOn("L0_HomePage_LoggedInUser"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        [IgnorePageErrors]//[Pending("Test in development. Code in development.")]
        public void L0_HomePage_CookiedUser()
        {
            var homePage = Client.Home();

            // Add user's cookie to see the personalised Navigation Header
            var userCookie = new Cookie(_userCookie.Name, _userCookie.Value, _userCookie.Domain, _userCookie.Path, _userCookie.Expiry);
            Client.Driver.Manage().Cookies.AddCookie(userCookie);

            Client.Driver.Navigate().Refresh();
            homePage = new HomePage(Client);

            Assert.AreEqual("If you're not " + _truncatedFirstNameL0 + "..., click here", homePage.GetHeaderBarText()); //Verify header bar text

            //Verify Welcome message text
            Assert.AreEqual("Welcome back " + _truncatedFirstNameL0 + "...! (not " + _truncatedFirstNameL0 +
                                        "...?) click hereWe can deposit up to £300 in your bank account by " + DateTime.Now.AddMinutes(24).ToShortTimeString()
                                        + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText());

            Assert.AreEqual("300", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            //Esnure 'click here' link in welcome message takes User back to home page.
            homePage.ClickWelcomeMessageClickHereLink();
            homePage = new HomePage(Client);
            Assert.IsFalse(homePage.IsHeaderBarTextVisible());
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), DependsOn("L0_HomePage_LoggedInUser"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        [IgnorePageErrors]//[Pending("Test in development. Code in development.")]
        public void L0_HomePage_CookiedUser_ClickHereInNavigationHeader()
        {
            Assert.IsNotNull(_userCookie, "Cookie is not created. Ensure the test is run after test that creates the cookie.");

            // Check cookied user
            //-----------------------
            var homePage = Client.Home();

            // Add user's cookie to see the personalised Navigation Header
            var userCookie = new Cookie(_userCookie.Name, _userCookie.Value, _userCookie.Domain, _userCookie.Path, _userCookie.Expiry);
            Client.Driver.Manage().Cookies.AddCookie(userCookie);

            Client.Driver.Navigate().Refresh();
            homePage = new HomePage(Client);

            //Ensure header bar text is displayed correctly
            Assert.AreEqual("If you're not " + _truncatedFirstNameL0 + "..., click here", homePage.GetHeaderBarText());

            //Esnure 'click here' link in welcome message takes User back to home page.
            homePage.ClickHeaderBarClickHereLink();
            homePage = new HomePage(Client);
            Assert.IsFalse(homePage.IsHeaderBarTextVisible());
        }
    }

    #endregion

    #region Ln_HomePage_Personalised

    public class HomePageLnTests : UiTest
    {
        private string _emailLn;
        private string _fullFirstNameLn;
        //private string _url;
        private string _truncatedFirstNameLn;
        private Cookie _userCookie;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            // Create Ln user
            _emailLn = Get.RandomEmail();
            _fullFirstNameLn = Get.RandomString(20);
            _truncatedFirstNameLn = _fullFirstNameLn.Remove(15);
            Console.WriteLine("Email={0}, First Name={1}, Trancated First Name={2}", _emailLn, _fullFirstNameLn, _truncatedFirstNameLn);

            Customer customerLn = CustomerBuilder.New().WithEmailAddress(_emailLn).WithForename(_fullFirstNameLn).Build();
            var appicationLn = ApplicationBuilder.New(customerLn).WithLoanAmount(150).Build();
            appicationLn.RepayOnDueDate();
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370")] //Pending("Test in development. Code in development."), 
        [MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void Ln_HomePage_LoggedInUser()
        {
            var loginPage = Client.Login();
            loginPage.LoginAs(_emailLn);
            var homePage = Client.Home();

            // Save cookie
            _userCookie = Client.Driver.Manage().Cookies.GetCookieNamed("wonga_visitor");

            Console.WriteLine(_emailLn);
            Assert.AreEqual("Welcome back " + _truncatedFirstNameLn + "...! (not " + _truncatedFirstNameLn + "...?) click hereWe can deposit up to £400 in your bank account by " + DateTime.Now.AddMinutes(24).ToShortTimeString() + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText()); // user has logged in
            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            // TODO: Change Expected text in welcome message when the code is fixed
            // TODO: UKWEB-371: Check Navigation Header (international-trigger, help-trigger, social-trigger, login-trigger, #logout)
            // Check "Welcome <15-symbolsTrancatedFirstName> Logout"

            //Esnure 'click here' link in welcome message takes User back to home page.
            homePage.ClickWelcomeMessageClickHereLink();
            homePage = new HomePage(Client);
            Assert.IsFalse(homePage.IsHeaderBarTextVisible());
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370")]
        //, Pending("Test in development. Code in development."), 
        [DependsOn("Ln_HomePage_LoggedInUser"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        [IgnorePageErrors]
        public void Ln_HomePage_CookiedUser_ClickHereInWelcomeMessage()
        {
            Assert.IsNotNull(_userCookie, "Cookie is not created. Ensure the test is run after test that creates the cookie.");

            // Check cookied user
            //-----------------------
            var homePage = Client.Home();

            // Add user's cookie to see the personalised Navigation Header
            var userCookie = new Cookie(_userCookie.Name, _userCookie.Value, _userCookie.Domain, _userCookie.Path, _userCookie.Expiry);
            Client.Driver.Manage().Cookies.AddCookie(userCookie);

            Client.Driver.Navigate().Refresh();
            homePage = new HomePage(Client);

            //Ensure header bar text is displayed correctly
            Assert.AreEqual("If you're not " + _truncatedFirstNameLn + "..., click here", homePage.GetHeaderBarText());

            //Ensure welcome message text is displayed correctly
            Assert.AreEqual("Welcome back " + _truncatedFirstNameLn + "...! (not "
                           + _truncatedFirstNameLn + "...?) click hereWe can deposit up to £400 in your bank account by "
                           + DateTime.Now.AddMinutes(24).ToShortTimeString() + homePage.GetWelcomeMessageDay(), homePage.GetWelcomeHeaderMessageText());

            //Ensure maximum available credit value is displayed correctly
            Assert.AreEqual("400", homePage.Sliders.MaxAvailableCredit(), "Max Available Credit in sliders is wrong.");

            //Esnure 'click here' link in welcome message takes User back to home page.
            homePage.ClickWelcomeMessageClickHereLink();
            homePage = new HomePage(Client);
            Assert.IsFalse(homePage.IsHeaderBarTextVisible());
            Console.WriteLine("Test is run");
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-370"), IgnorePageErrors, Pending("Test in development. Code in development.")] 
        [DependsOn ("Ln_HomePage_LoggedInUser"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void Ln_HomePage_CookiedUser_ClickHereInNavigationHeader()
        {
            Assert.IsNotNull(_userCookie, "Cookie is not created. Ensure the test is run after test that creates the cookie.");

            // Check cookied user
            var homePage = Client.Home();

            // Add user's cookie to see the personalised Navigation Header
            var userCookie = new Cookie(_userCookie.Name, _userCookie.Value, _userCookie.Domain, _userCookie.Path, _userCookie.Expiry);
            Client.Driver.Manage().Cookies.AddCookie(userCookie);

            Client.Driver.Navigate().Refresh();
            homePage = new HomePage(Client);

            //Ensure header bar text is displayed correctly
            Assert.AreEqual("If you're not " + _truncatedFirstNameLn + "..., click here", homePage.GetHeaderBarText());

            //Esnure 'click here' link in welcome message takes User back to home page.
            homePage.ClickHeaderBarClickHereLink();
            homePage = new HomePage(Client);
            Assert.IsFalse(homePage.IsHeaderBarTextVisible());
            Console.WriteLine("Test is run");
        }
    }

    #endregion

    #region Miscellaneous_Homepage_Tests

    public class HomePageMiscellaneousTests : UiTest
    {
        //private string _emailL0;
        //private string _fullFirstNameL0;
        private string _url;
        //private string _truncatedFirstNameL0;

        private void CheckContentSlots()
        {
            //Ensure correct title is present in first content slot in home page
            Assert.AreEqual("Betty, Earl and Joyce show how wonga.com's short term cash loans put you in control", Client.Home().GetContentSlot1Title());
            Console.WriteLine("Content Slot1 title is: " + Client.Home().GetContentSlot1Title());

            //Ensure correct text is displayed in 'Wonga customers' box in home page
            Assert.AreEqual("Populus survey of 2012 with over 25,000 respondents", Client.Home().GetWongaCustomersBoxText());
            Console.WriteLine("Text in Wonga customers box: " + Client.Home().GetWongaCustomersBoxText());

            //Ensure correct text is displayed in 'Responsible lending' box in home page
            Assert.AreEqual("Responsible lending", Client.Home().GetResponsibleLendingBoxText());
            Console.WriteLine("Responsible lending box heading: " + Client.Home().GetResponsibleLendingBoxText());
        }

        //[FixtureSetUp]
        //public void FixtureSetup()
        //{
        //    _emailL0 = Get.RandomEmail();
        //    _fullFirstNameL0 = Get.RandomString(20);
        //    _truncatedFirstNameL0 = _fullFirstNameL0.Remove(15);
        //    Console.WriteLine("Email={0}, First Name={1}, Trancated First Name={2}", _emailL0, _fullFirstNameL0, _truncatedFirstNameL0);

        //    Customer customerL0 = CustomerBuilder.New().WithEmailAddress(_emailL0).WithForename(_fullFirstNameL0).Build();
        //    ApplicationBuilder.New(customerL0).WithLoanAmount(100).Build();
        //}

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

            // TODO: 1. Click on the APR Example link. 2. Check the current page is Homepage.
            // TODO: Check the page APR Example does not exist
        }

        [Test, AUT(AUT.Uk), JIRA("UKWEB-229"), MultipleAsserts, Owner(Owner.PavithranVangiti)]
        public void HomePageContentSlotsTest()
        {
            var homePage = Client.Home();

            //Check Content slots pre-login
            CheckContentSlots();

            //Check Content slots post-login
            //var loginPage = Client.Login();
            //loginPage.LoginAs(_emailL0);
            //homePage.Client.Home();
            //CheckContentSlots();
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
    }

    #endregion
}

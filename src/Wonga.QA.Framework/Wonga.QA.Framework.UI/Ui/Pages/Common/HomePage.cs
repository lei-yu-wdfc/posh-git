using System;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Elements;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;

namespace Wonga.QA.Framework.UI.UiElements.Pages
{
    public class HomePage : BasePage, IApplyPage
    {
        //public IWebElement MenuContent;
        public SlidersElement Sliders { get; set; }
        public HelpElement Help { get; set; }
        public InternationalElement InternationalElements { get; set; }
        public LoginElement Login { get; set; }
        public SurveyElement Survey { get; set; }
        public ContactElement Contact { get; set; }
        private IWebElement _headerMessage;
        private IWebElement _subMessage;
        private IWebElement _promoBoxes;
        private IWebElement _awards;
        private IWebElement _seoLinks;
        private IWebElement _codeOfPracticeLink;
        private IWebElement _trustRatingLink;
        private IWebElement _aprLink;
        private IWebElement _contactUsLink;
        private IWebElement _privacyPolicyLink;
        private IWebElement _veriSignLink;
        private IWebElement _flaLink;
        private IWebElement _kivaLink;
        private IWebElement _mediaGuardianAwardsLink;
        private IWebElement _webbyAwardsLink;
        private IWebElement _ccrCreditAwardsLink;
        private IWebElement _techTrack100Link;
        private IWebElement _paydayLoansLink;
        private IWebElement _quickLoanLink;
        private IWebElement _cashLoanLink;
        private IWebElement _contentSlot1;
        private IWebElement _wongaCustomersBox;
        private IWebElement _responsibleLendingBox;
        private IWebElement _representativeExample;
        //private IWebElement _fastCashLink;
        //private IWebElement _cashAdvanceLink;
        //private IWebElement _quickQuidLink;
        //private IWebElement _borrowMoneyLink;
        //private IWebElement _loansOnlineLink;
        private SocialElement Social;


        public string ending;

        public TabsElement Tabs { get; set; }

        public HomePage(UiClient client)
            : base(client)
        {
            Sliders = new SlidersElement(this);
            switch (Config.AUT)
            {
                case (AUT.Ca):
                    Contact = new ContactElement(this);
                    Help = new HelpElement(this);
                    InternationalElements = new InternationalElement(this);
                    Survey = new SurveyElement(this);
                    Login = new LoginElement(this);
                    Tabs = new TabsElement(this);
                    break;
                case (AUT.Za):
                case (AUT.Pl):
                    Contact = new ContactElement(this);
                    Help = new HelpElement(this);
                    InternationalElements = new InternationalElement(this);
                    Login = new LoginElement(this);
                    Tabs = new TabsElement(this);
                    break;
                case (AUT.Wb):
                    Help = new HelpElement(this);
                    Tabs= new TabsElement(this);
                    Login = new LoginElement(this);
                    Contact = new ContactElement(this);
                    break;
                case (AUT.Uk):
                    InternationalElements = new InternationalElement(this);
                    Help = new HelpElement(this);
                    Social = new SocialElement(this);
                    Login = new LoginElement(this);
                    break;
            }
            //Tabs = new TabsElement(this);

        }

        public string PopupSetProvince
        {
            set
            {
                Do.With.Interval(1).While(LookForProvicePopup);
                Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.YourProvince)).SelectOption(value);
            }
        }

        public IApplyPage PopupClickThisIsMyProvince()
        {
            Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ThisIsMyProvince)).Click();
            return new ApplyPage(Client);
        }
        private bool LookForProvicePopup()
        {
            var popupTitle = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ProvincePopupTitle));
            return !popupTitle.Displayed;
        }

        public void AssertThatIsWbHomePage()
        {
            Assert.That(Headers, Has.Item(UiMap.Get.HomePage.BusinessTitleText));

        }

        public void CloseWbWelcomePopup()
        {
            try
            {
                var wbWelcomePopUp = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePageWelcomePopup.Frame));
                if(wbWelcomePopUp.Displayed)
                    Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePageWelcomePopup.Close)).Click(); 

            }
            catch (NoSuchElementException)
            {}
        }
       
        public bool IsNewBodyFrameworkExists()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.BodyFramework)));
            var bodyFramework = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.BodyFramework));
            return bodyFramework.Displayed;
        }

        public string GetPromoBoxes()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.PromoBoxes)));
            _promoBoxes = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.PromoBoxes));
            var promoBoxesText = _promoBoxes.Text;
            return promoBoxesText;
        }

        public string GetAwardsList()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.AwardsList)));
            _awards = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.AwardsList));
            var awardsText = _awards.Text;
            return awardsText;
        }

        public string GetSeoLinks()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.SeoLinks)));
            _seoLinks = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.SeoLinks));
            var seoLinksText = _seoLinks.Text;
            return seoLinksText;
        }

        public String GetWelcomeHeaderMessageText()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WelcomeHeaderMessage)));
            _headerMessage = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WelcomeHeaderMessage));
            var headerMessageText = _headerMessage.Text.Replace("\r\n", " ");
            return headerMessageText;
        }

        public String GetWelcomeSubMessageText()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WelcomeSubMessage)));
            _subMessage = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WelcomeSubMessage));
            var subMessageText = _subMessage.Text;
            return subMessageText;
        }

        public String GetWelcomeMessageDay()
        {
            ending = " today.";
            if ((DateTime.Now.AddMinutes(24).Hour.ToString("#") == "00") && (DateTime.Now.Hour.ToString("#") != "00"))
            ending = " tomorrow.";
            return ending;
        }

        public String GetHomePageCodeOfPracticeLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.CodeOfPracticeLink)));
            _codeOfPracticeLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.CodeOfPracticeLink));
            var codeOfPracticeLink = _codeOfPracticeLink.GetAttribute("href");
            return codeOfPracticeLink;
        }

        public String GetHomePageTrustRatingLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.TrustRatingLink)));
            _trustRatingLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.TrustRatingLink));
            var trustRatingLink = _trustRatingLink.GetAttribute("href");
            return trustRatingLink;
        }

        public String GetHomePageAPRLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.AprLink)));
            _aprLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.AprLink));
            var aprLink = _aprLink.GetAttribute("href");
            return aprLink;
        }

        public String GetHomePageContactUsLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ContactUsLink)));
            _contactUsLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ContactUsLink));
            var contactUsLink = _contactUsLink.GetAttribute("href");
            return contactUsLink;
        }

        public String GetHomePagePrivacyPolicyLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.PrivacyPolicyLink)));
            _privacyPolicyLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.PrivacyPolicyLink));
            var privacyPolicyLink = _privacyPolicyLink.GetAttribute("href");
            return privacyPolicyLink;
        }

        public String GetHomePageVeriSignLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.VeriSignLink)));
            _veriSignLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.VeriSignLink));
            var veriSignLink = _veriSignLink.GetAttribute("href");
            return veriSignLink;
        }

        public String GetHomePageFLALink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.FlaLink)));
            _flaLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.FlaLink));
            var flaLink = _flaLink.GetAttribute("href");
            return flaLink;
        }

        public String GetHomePageKIVALink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.KivaLink)));
            _kivaLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.KivaLink));
            var kivaLink = _kivaLink.GetAttribute("href");
            return kivaLink;
        }

        public String GetHomePageMediaGuardianAwardsLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.MediaGuardianAwardsLink)));
            _mediaGuardianAwardsLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.MediaGuardianAwardsLink));
            var mediaGuardianAwardsLink = _mediaGuardianAwardsLink.GetAttribute("href");
            return mediaGuardianAwardsLink;
        }

        public String GetHomePageWebbyAwardsLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WebbyAwardsLink)));
            _webbyAwardsLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WebbyAwardsLink));
            var webbyAwardsLink = _webbyAwardsLink.GetAttribute("href");
            return webbyAwardsLink;
        }

        public String GetHomePageCCRCreditAwardsLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.CcrCreditAwardLink)));
            _ccrCreditAwardsLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.CcrCreditAwardLink));
            var ccrCreditAwardsLink = _ccrCreditAwardsLink.GetAttribute("href");
            return ccrCreditAwardsLink;
        }

        public String GetHomePageTechTrack100Link()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.TechTrack100Link)));
            _techTrack100Link = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.TechTrack100Link));
            var techTrack100Link = _techTrack100Link.GetAttribute("href");
            return techTrack100Link;
        }

        public String GetHomePagePaydayLoansLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.PaydayLoansLink)));
            _paydayLoansLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.PaydayLoansLink));
            var paydayLoansLink = _paydayLoansLink.GetAttribute("href");
            return paydayLoansLink;
        }

        public String GetHomePageCashLoanLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.CashLoanLink)));
            _cashLoanLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.CashLoanLink));
            var cashLoanLink = _cashLoanLink.GetAttribute("href");
            return cashLoanLink;
        }

        public String GetHomePageQuickLoanLink()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.QuickLoanLink)));
            _quickLoanLink = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.QuickLoanLink));
            var quickLoanLink = _quickLoanLink.GetAttribute("href");
            return quickLoanLink;
        }

        public String GetContentSlot1Title()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ContentSlot1Title)));
            _contentSlot1 = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ContentSlot1Title));
            var contenSlot1 = _contentSlot1.GetAttribute("title");
            return contenSlot1;
        }

        public String GetWongaCustomersBoxText()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WongaCustomersBoxText)));
            _wongaCustomersBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.WongaCustomersBoxText));
            var wongaCustomersBox = _wongaCustomersBox.Text;
            return wongaCustomersBox;
        }

        public String GetResponsibleLendingBoxText()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ResponsibleLendingBoxText)));
            _responsibleLendingBox = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.ResponsibleLendingBoxText));
            var responsibleLendingBox = _responsibleLendingBox.Text;
            return responsibleLendingBox;
        }

        public String GetRepresentativeExampleText()
        {
            Do.With.Timeout(new TimeSpan(0, 0, 5)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.RepresentativeExampleTrigger)));

            IWebElement AprExampleLink = Content.FindElement(By.CssSelector(UiMap.Get.HomePage.RepresentativeExampleTrigger));

            Actions builder = new Actions(Client.Driver);
            builder.MoveToElement(AprExampleLink);

            Do.With.Timeout(new TimeSpan(0, 0, 10)).Until(() => Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.RepresentativeExampleText)));
            _representativeExample = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.RepresentativeExampleText));
            var representativeExampleText = _representativeExample.Text;
            return representativeExampleText;
        }

        public bool IsMocked()
        {
            try
            {
                var mocked = Client.Driver.FindElement(By.CssSelector(UiMap.Get.HomePage.Mocked));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

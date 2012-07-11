using System;
using MbUnit.Framework;
using NHamcrest.Core;
using OpenQA.Selenium;
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

    }
}

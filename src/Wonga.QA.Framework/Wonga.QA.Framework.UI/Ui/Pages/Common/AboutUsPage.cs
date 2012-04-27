using System;
using System.Threading;
using OpenQA.Selenium;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Interfaces;
using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.UiElements.Pages.Common
{
    public class AboutUsPage : BasePage
    {
        private readonly IWebElement _news;
        private readonly IWebElement _ourCustomers;
        private readonly IWebElement _responsibleLending;
        private readonly IWebElement _whyUseUs;
        private IWebElement _wereDifferent;
        private IWebElement _wereFast;
        private IWebElement _wereResponsible;
        private IWebElement _wongaMoments;
        private IWebElement _tabTitle;

        public AboutUsPage(UiClient client)
            : base(client)
        {
            switch (Config.AUT)
            {
                case (AUT.Ca):
                case (AUT.Za):
                    _news = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.News));
                    _ourCustomers = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.OurCustomers));
                    _responsibleLending = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.ResponsibleLending));
                    _whyUseUs = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WhyUseUs));
                    break;
            }
        }

        public BlogPage NewsClick()
        {
            _news.Click();
            return new BlogPage(Client);
        }

        public OurCustomersPage OurCustomersClick()
        {
            _ourCustomers.Click();
            return new OurCustomersPage(Client);
        }

        public ResponsibleLendingPage ResponsibleLendingClick()
        {
            _responsibleLending.Click();
            return new ResponsibleLendingPage(Client);
        }

        public WhyUseUsPage WhyUseUsClick()
        {
            _whyUseUs.Click();
            return new WhyUseUsPage(Client);
        }

        public bool WereDifferentClickAndCheck()
        {
            _wereDifferent = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereDifferent));
            _wereDifferent.Click();
            _tabTitle = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereDifferentTitle));
            if (_tabTitle.Text == "We're different")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WereFastClickAndCheck()
        {
            _wereFast = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereFast));
            _wereFast.Click();
            _tabTitle = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereFastTitle));
            if (_tabTitle.Text == "We're fast")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WereResponsibleClickAndCheck()
        {
            _wereResponsible = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereResponsible));
            _wereResponsible.Click();
            _tabTitle = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereResponsibleTitle));
            if (_tabTitle.Text == "We're responsible")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool WongaMomentsClickAndCheck()
        {
            _wongaMoments = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WongaMoments));
            _wongaMoments.Click();
            _tabTitle = Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WongaMomentsTitle));
            if (_tabTitle.Text == "Wonga moments")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetTextFromPage
        {
            get { return Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.AboutUsText)).Text; }
        }

        public string GetWereDifferentText
        {
            get
            {
                switch (Config.AUT)
                {
                        case AUT.Wb:
                        Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereDifferent)).Click();
                        break;
                }
                return Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereDifferentText)).Text;
            }
        }

        public string GetWereFastText
        {
            get
            {
                switch (Config.AUT)
                {
                    case AUT.Wb:
                        Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereFast)).Click();
                        break;
                }
                return Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereFastText)).Text;
            }
        }

        public string GetWereResponsibleText
        {
            get
            {
                switch (Config.AUT)
                {
                    case AUT.Wb:
                        Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereResponsible)).Click();
                        break;
                }
                return Client.Driver.FindElement(By.CssSelector(UiMap.Get.AboutUsPage.WereResponsibleText)).Text;
            }
        }
    }
}

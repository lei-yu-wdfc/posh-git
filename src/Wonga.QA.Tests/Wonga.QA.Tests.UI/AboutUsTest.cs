using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    class AboutUsTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-169")]
        public void CustomerOnAboutUsPageShouldBeAbleChooseEveryLink()
        {
            string news = "wonga.com/blog";
            string ourCustomers = "wonga.com/our-customers";
            string responsibleLending = "wonga.com/our-commitment-responsible-lending";
            string whyUseUs = "wonga.com/why-use-us";
            const string ca = "ca.";
            const string za = "za.";

            switch (Config.AUT)
            {
                case (AUT.Ca):
                    news = ca + news;
                    ourCustomers = ca + ourCustomers;
                    responsibleLending = ca + responsibleLending;
                    whyUseUs = ca + whyUseUs;
                    break;
                case (AUT.Za):
                    news = za + news;
                    ourCustomers = za + ourCustomers;
                    responsibleLending = za + responsibleLending;
                    whyUseUs = za + whyUseUs;
                    break;
            }


            var aboutpage = Client.About();
            Assert.IsTrue(aboutpage.WereFastClickAndCheck());
            Assert.IsTrue(aboutpage.WereDifferentClickAndCheck());
            Assert.IsTrue(aboutpage.WereResponsibleClickAndCheck());
            Assert.IsTrue(aboutpage.WongaMomentsClickAndCheck()); 

            aboutpage.NewsClick();
            Assert.IsTrue(aboutpage.Url.Contains(news));
            aboutpage = Client.About();
            aboutpage.OurCustomersClick();
            Assert.IsTrue(aboutpage.Url.Contains(ourCustomers));
            aboutpage = Client.About();
            aboutpage.ResponsibleLendingClick();
            Assert.IsTrue(aboutpage.Url.Contains(responsibleLending));
            aboutpage = Client.About();
            aboutpage.WhyUseUsClick();
            Assert.IsTrue(aboutpage.Url.Contains(whyUseUs));
        }
    }
}

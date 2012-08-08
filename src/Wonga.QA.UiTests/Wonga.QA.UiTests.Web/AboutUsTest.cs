using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [Parallelizable(TestScope.All)]
    class AboutUsTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-169"), Category(TestCategories.SmokeTest)]
        public void CustomerOnAboutUsPageShouldBeAbleChooseEveryLink()
{
            string news = "blog";
            string ourCustomers = "our-customers";
            string responsibleLending = "our-commitment-responsible-lending";
            string whyUseUs = "why-use-us";
          
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

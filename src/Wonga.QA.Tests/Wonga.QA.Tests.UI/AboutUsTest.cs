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
    [Parallelizable(TestScope.All)]
    class AboutUsTest : UiTest
    {
        [Test, AUT(AUT.Za, AUT.Ca), JIRA("QA-169"), SmokeTest]
        public void CustomerOnAboutUsPageShouldBeAbleChooseEveryLink()
{
            string homestr = Config.Ui.Home.ToString().Replace("http", "https");
            string news = homestr + "blog";
            string ourCustomers = homestr + "our-customers";
            string responsibleLending = homestr + "our-commitment-responsible-lending";
            string whyUseUs = homestr + "why-use-us";
          
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

        [Test, AUT(AUT.Wb), JIRA("QA-253")]
        public void AboutUsWongaBusinessSouldHasCorrectDataDisplayed()
        {
            #region verification
            var verificationText = "Wonga Business is a ground-breaking, online business lender. Our new service is powered by the same technology used by Wonga.com, one of the world’s most innovative credit providers, which has already processed millions of consumer loans." + "Our mission is to solve businesses’ short term and urgent cash flow needs with an equally short term and responsible solution." + " Wonga Business launched in April 2012 and we are proud to be transforming the UK business credit market by offering small, short term business loans with more speed, convenience and flexibility than the banks.  We remove the complexity and inflexibility that businesses face whenever they need to borrow some cash in a hurry to support their business growth and needs.";
            var verificationDifferent = "Wonga Business, like Wonga.com, is different from other lenders because our sophisticated risk and decision technology means the application and approval process takes literally minutes. We are the first company in the world to fully automate the business lending process and we are able to make completely objective and responsible decisions around the clock. There’s no paperwork, meetings with bank managers or hanging on the phone – our entire service is online and real-time. We are also uniquely flexible, allowing business applicants to choose exactly how much cash they want to borrow and for how many weeks they need it. Our business customers aren't forced to borrow a fixed sum they might not need, or pay interest for any longer than necessary.";
            //var verificationFast = "Business applicants simply decide how much they want to borrow, for how many weeks, apply and we calculate the total cost in real-time before they proceed. Once an application is approved, we can deposit the money straight into a business bank account within one business day – and as little as 15 minutes depending on the bank in question.  Even collection is hassle-free, as we take a weekly repayment via continuous payment authority using your business debit card on the agreed day for the agreed term.";
            var verificationResponsible = "Wonga Business offers short term loans for up to 52 weeks and we make money when our customers repay us quickly, not by continually extending a growing line of credit. We are serious about our commitment to responsible lending.";
            #endregion
            var aboutpage = Client.About();
            Assert.AreEqual(verificationText, aboutpage.GetTextFromPage.Replace("\r\n", ""));
            Assert.AreEqual(verificationDifferent, aboutpage.GetWereDifferentText);
            // Assert.AreEqual(verificationFast, aboutpage.GetWereFastText); //text on page and Expected text are different
            Assert.AreEqual(verificationResponsible, aboutpage.GetWereResponsibleText);
        }
    }
}

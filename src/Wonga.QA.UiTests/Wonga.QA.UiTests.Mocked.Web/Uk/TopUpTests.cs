using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [TestFixture]
    internal class TopUpTests: UiTest
    {
        [Test, JIRA("UK-826"), MultipleAsserts]
        [Owner(Owner.StanDesyatnikov)]
        [Pending("UKWEB-928: Top Up throws an exception on the Accept page")]
        public void TopUpHappyPath()
        {
            const string topupAmount = "10";
           // const string newTotalToRepay = "172.37";
            const string totalRepayable = "504.23";
            //var interestAndFees = "";

            string email = Get.RandomEmail();
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.TopupSliders.HowMuch = topupAmount;

            var requestPage = mySummaryPage.TopupSliders.Apply();

            requestPage.SubmitButtonClick();

            var processPage = new TopupProcessingPage(this.Client);
            var agreementPage = processPage.WaitForAgreementPage(Client);

            Assert.IsFalse(agreementPage.IsTopupAgreementPageDateNotPresent(), "Date on Agreement page is not displayed");
            Assert.IsTrue(agreementPage.IsTopupAgreementPageLegalInfoDisplayed(), "Legal Info on Agreement page is not displayed");
            Assert.IsFalse(agreementPage.IsTopupAgreementPageTopupAmountNotPresent(), "Topup Amount on Agreement page is not displayed");
            Assert.IsFalse(agreementPage.IsTopupTotalAmountTokenBeingReplaced(), "Amount Token Agreement page is not replaced with value");

            var dealDonePage = agreementPage.Accept();
            Assert.IsFalse(dealDonePage.IsDealDonePageDateNotPresent(), "Date on Deal Done page is not displayed");
            Assert.IsFalse(dealDonePage.IsDealDonePageJiffyNotPresent(), "Jiffy on Deal Done page is not displayed");
            Assert.IsFalse(dealDonePage.IsDealDonePageTopupAmountNotPresent(), "Topup Amount on Deal Done page is not displayed");
            Assert.Contains(dealDonePage.SucessMessage, totalRepayable, "Success Message on Deal Done page does not contain Total Repayable");
            Assert.Contains(dealDonePage.SucessMessage, topupAmount, "Success Message on Deal Done page does not contain Total Amount");

            dealDonePage.ContinueToMyAccount();

            //Test my account summary page
            Assert.IsTrue(this.Client.Driver.Url.Contains("my-account"), "My Account page was not open");
        }
    }
}

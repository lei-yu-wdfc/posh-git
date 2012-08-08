using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.UiTests.Mocked.Web.Uk
{
    [Parallelizable(TestScope.All), SUT(SUT.WIP), AUT(AUT.Uk)]
    internal class RepayTests: UiTest
    {
        [Test, JIRA("UKWEB-247"), MultipleAsserts]
        public void RepayEarlyFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            Assert.IsNotEmpty(requestPage.RepayCard);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayEarlyFullpaySuccessPage>() as RepayEarlyFullpaySuccessPage;

            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent());
            Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageDateTokenNotPresent());

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            var testTitle = "Success! Your balance has been settled in full";
            var testMessage = "Thanks and nice work for repaying early";

            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testTitle), "Title is missing on Payment Taken page");
            Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testMessage), "Success Message is missing on Payment Taken page");
        }
    }
}

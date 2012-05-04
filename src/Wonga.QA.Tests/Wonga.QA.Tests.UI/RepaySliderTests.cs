using System.Globalization;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.UI;
using Wonga.QA.Tests.Core;
using System.Linq;
using System;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Tests.Ui
{
    /// <summary>
    /// TopupSlider tests for UK
    /// </summary>
    /// 
    class RepaySliderTests : UiTest
    {
        private int _amountMax;
        private int _amountMin;
        //private string _repaymentDate;
        private ApiResponse _response;
        //private DateTime _actualDate;
       
        
        [Test, AUT(AUT.Uk), JIRA("UK-1827")]
        public void MovingRepaySliderRemainingAmountShouldBeCorrect()
        {
            //build L0 loan
            string email = Get.RandomEmail();
            DateTime todayDate = DateTime.Now;

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(email);
            var mySummaryPage = myAccountPage.Navigation.MySummaryButtonClick();

            mySummaryPage.RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Runs assertions internally
            requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.Id.ToString());

            //Branch point - Add Cv2 for each path and proceed
            //requestPage.setSecurityCode("888");
            //requestPage.SubmitButtonClick();

            //var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

            //var declinedPage = extensionProcessingPage.WaitFor<ExtensionPaymentFailedPage>() as ExtensionPaymentFailedPage;

            //Assert.IsFalse(declinedPage.IsPaymentFailedAmountNotPresent());
            //Assert.IsFalse(declinedPage.IsPaymentFailedDateNotPresent());
        }


    }
}
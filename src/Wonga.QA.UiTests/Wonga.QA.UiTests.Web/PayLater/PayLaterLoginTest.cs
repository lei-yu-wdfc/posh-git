using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.PayLater
{
    public class PayLaterLoginTest : UiTest
    {
        [Test, AUT(AUT.Uk), JIRA("QA-77, 79, 80, 81"), Pending("Fails trying to open http://dev.paylater.com/")]
        public void InspectElementsHomePage()
        {
            var payLaterStartPage = Client.PayLaterStart();
            var payLaterHome = payLaterStartPage.InspectElemrnts();

            Assert.AreEqual("Representative APR 28.7%", payLaterHome["Apr"]);
            Assert.AreEqual("Sign Up", payLaterHome["SingUp"]);
            Assert.AreEqual("£11.68", payLaterHome["Fee"]);
            Assert.AreEqual("Forgotten your password", payLaterHome["ForgottenPassword"]);
            Assert.AreEqual("£57.43", payLaterHome["PaymentAmount"]);
        }


        [Test, AUT(AUT.Uk), JIRA("QA-78"), Pending("Fails trying to open http://dev.paylater.com/")]
        public void LoginPagePayments()
        {
            const string user = "qa.wongatest+2012425163553@gmail.com";
            const string pass = "Passw0rd";

            var payLaterStartPage = Client.PayLaterStart();
            payLaterStartPage.LoginAs(user, pass);

            Thread.Sleep(2000);

            var payLaterSubmited = Client.PayLaterSubmition();
            var elementsSubmition = payLaterSubmited.InspectElementsSubmition();

            Assert.AreEqual("£1000", elementsSubmition["AvailabelCreditCookie"]);
            
            payLaterSubmited.RedirectToThanks();

            Thread.Sleep(2000);
        }

        [Test, AUT(AUT.Uk), JIRA("QA-82, 75, 84, 86, 89"), Pending("Fails trying to open http://dev.paylater.com/")]
        public void InspectElementsSubmitionPage()
        {
            const string footerIn = @"Paylater is a trading name of Wonga.com Ltd Copyright © 2008 - 2012 Wonga.com. Registered in England and Wales. 
                                    Registered Number: 6374235. Registered Address: 88 Crawford Street, London, W1H 2EJ. Wonga.com’s consumer credit activities 
                                    are licensed by the OFT, licence number 611974. Paylater is only available to over 18s and are subject to status.";

            const string user = "qa.wongatest+2012425163553@gmail.com";
            const string pass = "Passw0rd";

            var payLaterStartPage = Client.PayLaterStart();

            payLaterStartPage.LoginAs(user, pass);

            Thread.Sleep(2000);

            var payLaterSubmited = Client.PayLaterSubmition();

            var elementsSubmition = payLaterSubmited.InspectElementsSubmition();

            Assert.AreEqual(footerIn, elementsSubmition["FooterInfo"]);
            Assert.AreEqual("£1000", elementsSubmition["AvailabelCreditCookie"]);
            Assert.AreEqual("?", elementsSubmition["InfoCirculButton"]);
            Assert.AreEqual("You've been approved, please accept the paylater Agreement", elementsSubmition["TextApproved"]);
            Assert.AreEqual("You'll pay £11.68 today and £57.43 on 27th May and £57.43 on 27th June and £57.43 on 27th July ?", elementsSubmition["RepaymentDetails"]);
        }

        [Test, AUT(AUT.Uk), JIRA("QA-92"), Pending("Fails trying to open http://dev.paylater.com/")]
        public void InspectElementsThanksPage()
        {
            const string user = "qa.wongatest+2012425163553@gmail.com";
            const string pass = "Passw0rd";

            var payLaterStartPage = Client.PayLaterStart();
            payLaterStartPage.LoginAs(user, pass);

            Thread.Sleep(2000);

            var payLaterSubmited = Client.PayLaterSubmition();
            payLaterSubmited.InspectElementsSubmition();
            var payLaterThank = payLaterSubmited.RedirectToThanks();
            var elemntThank = payLaterThank.InspectElementsThanks();

            Assert.AreEqual("Thanks for using paylater You still have £1000 available credit", elemntThank["ThanksText"]);
        }
    }
}

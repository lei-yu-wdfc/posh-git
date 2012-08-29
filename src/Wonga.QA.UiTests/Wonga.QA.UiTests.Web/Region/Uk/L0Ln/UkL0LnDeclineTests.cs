using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Region.Uk.L0Ln
{
    [AUT(AUT.Uk)]
    class UkL0LnDeclineTests : UiTest
    {
        private Dictionary<string, string> _outputData = new Dictionary<string, string> { };

        struct Cust
        {
            public String email;
            public String FirstName;
            public String LastName;
        }

        private static string _creditAgenciesContactDetails = " Please find the contact details below to obtain a free copy of your credit report.\r\n\r\n" +
                                                     "Experian\r\nAutomated advice line: 0844 481 8000\r\nconsumer.helpservice@uk.experian.com\r\n" +
                                                     "Customer Support Centre\r\nPO Box 1136\r\nWarrington\r\nWA4 9GQ\r\n\r\n" +
                                                     "Callcredit\r\nGeneral questions: care@callcreditcheck.com\r\nCustomer Care\r\nCallcredit check\r\nPO Box 734\r\nLeeds\r\nLS1 9GX";

        /*private static string _experianContactDetails = " Please find the contact details below to obtain a free copy of your credit report.\r\n\r\n" +
                                                     "Experian\r\nAutomated advice line: 0844 481 8000\r\nconsumer.helpservice@uk.experian.com\r\n" +
                                                     "Customer Support Centre\r\nPO Box 1136\r\nWarrington\r\nWA4 9GQ";

        private static string _callCreditContactDetails = " Please find the contact details below to obtain a free copy of your credit report.\r\n\r\n" +
                                                     "Callcredit\r\nGeneral questions: care@callcreditcheck.com\r\nCustomer Care\r\nCallcredit check\r\nPO Box 734\r\nLeeds\r\nLS1 9GX";*/

        public enum RiskMaskForDeclinedLoan
        {
            //TESTBankAccountMatchedToApplicant,
            TESTBlacklist,
            TESTCreditBureauDataIsAvailable
            //TESTExperianCreditBureauDataIsAvailable,
            //TESTDateOfBirth, //-- does not work
            //TESTCustomerDateOfBirthIsCorrect, -- does not work
            //TESTPaymentCardIsValid, -- does not work
            //TESTCardMask, -- does not work
            //TESTIsAlive, -- does not work
            //TESTIsSolvent, -- does not work
            //TESTMonthlyIncome, -- does not work
            //TESTCreditBureauDataIsAvailable, -- works fine
            //TESTCustomerIsSolvent, -- does not work
            //TESTCustomerDateOfBirthIsCorrectSME, -- does not work
            //TESTMonthlyIncomeEnoughForRepayment, -- does not work
            //TESTRepaymentPredictionPositive, -- does not work
            //TESTCallValidatePaymentCardIsValid, -- does not work
            //TESTExperianPaymentCardIsValid, -- does not work
            //TESTRiskPaymentCardIsValid, -- does not work          
            //TESTExperianCustomerIsSolvent, -- does not work
            //TESTExperianCustomerDateOfBirthIsCorrect, -- does not work
            //TESTExperianCustomerDateOfBirthIsCorrectSME, -- does not work
        }

        [FixtureSetUp]
        public void MySetUp()
        {

        }

        [FixtureTearDown]
        private void MyTearDown()
        {
            // Output the emails of the created users
            Debug.WriteLine("**************");
            foreach (var outputData in _outputData)
            {
                Debug.WriteLine("{0},  {1}", outputData.Key, outputData.Value);
            }
            Debug.WriteLine("**************");
        }

        [Test, JIRA("UKWEB-253"), Owner(Owner.StanDesyatnikov, Owner.PavithranVangiti)]
        [Pending ("Test in development")]
        //[Row(RiskMaskForDeclinedLoan.TESTBankAccountMatchedToApplicant)]
        //[Row(RiskMaskForDeclinedLoan.TESTDateOfBirth)]
        //[Row(RiskMaskForDeclinedLoan.TESTExperianCreditBureauDataIsAvailable)]
        public void L0DeclinedWithVariousAdvices(RiskMaskForDeclinedLoan riskMask)
        //public void L0DeclinedWithVariousAdvices([EnumData(typeof(RiskMaskForDeclinedLoan))]RiskMaskForDeclinedLoan riskMask)
        {

            var customer = PrepareConditions(riskMask);
            var declinePage = RunL0Journey(customer, riskMask);
            Assert.IsTrue(declinePage.DeclineAdviceExists());

/*            var email = Get.RandomEmail();
            _outputData.Add(email, Get.EnumToString(riskMask));

            Client.Driver.Manage().Window.Maximize();
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmail(email)
                .WithEmployerName(Get.EnumToString(riskMask))
                .WithDeclineDecision();
                //.WithEmployerName(RiskMask.TESTBlacklist.ToString());
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;

            Assert.IsTrue(declinedPage.DeclineAdviceExists());*/
            //Assert.AreEqual(DeclineAdvices[riskMask], declinedPage.DeclineAdvice());
            //Console.WriteLine("L0 Decline Advice: {0}", declinedPage.DeclineAdvice());

            // TODO: check that "here" link in Decline Advice leads to correct page. Now it leads to Wonga.com/my-account, which does not exists.

            // TODO: Ln fails with Nearly There page instead of Decline Page
            // log in
            //var loginPage = Client.Login();
            //var mySummaryPage = loginPage.LoginAs(email);

            //var journeyLn = JourneyFactory.GetLnJourney(Client.Home());
            //var processingPage = journeyLn.Teleport<ProcessingPage>() as ProcessingPage;

            //Assert.IsTrue(declinedPage.DeclineAdviceExists());
            //Console.WriteLine("Ln Decline Advice: {0}", declinedPage.DeclineAdvice());
        }

        [Test, JIRA("UKWEB-253"), Owner(Owner.StanDesyatnikov, Owner.PavithranVangiti)]
        [Pending ("Test in development")]
        [Row(RiskMaskForDeclinedLoan.TESTBlacklist)]
        [Row(RiskMaskForDeclinedLoan.TESTCreditBureauDataIsAvailable)]
        public void L0Declined_WithGeneralAdvices(RiskMaskForDeclinedLoan riskMask)
        {
            var customer = PrepareConditions(riskMask);
            var declinePage = RunL0Journey(customer, riskMask);
            Assert.IsTrue(declinePage.DeclineAdviceExists());
            VerifyDeclineAdviceText(riskMask, declinePage);
        }

        #region helpers

        private Cust PrepareConditions(RiskMaskForDeclinedLoan riskMask)
        {
            //String email = Get.RandomEmail();
            //String FirstName = Get.RandomString(6);
            //String LastName = Get.RandomString(6);
            var cust = new Cust();
            cust.email = Get.RandomEmail();
            cust.FirstName = Get.RandomString(6);
            cust.LastName = Get.RandomString(6);
            _outputData.Add(cust.email, Get.EnumToString(riskMask));

            switch(riskMask)
            {
                case RiskMaskForDeclinedLoan.TESTBlacklist:
                    // create blacklisted user
                    var blackListTable = Drive.Data.Blacklist.Db.BlackList;
                    dynamic blackListEntity = new ExpandoObject();
                    blackListEntity.FirstName = cust.FirstName;
                    blackListEntity.LastName = cust.LastName;
                    blackListTable.Insert(blackListEntity);
                    break;

                case RiskMaskForDeclinedLoan.TESTCreditBureauDataIsAvailable:
                    //To do
                    break;

                /*case RiskMaskForDeclinedLoan.TESTDateOfBirth:
                    To do
                    break;

                case RiskMaskForDeclinedLoan.TESTPaymentCardIsValid:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTCreditBureauDataIsAvailable:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTMonthlyIncome:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTIsSolvent:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTIsAlive:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTCustomerHistoryIsAcceptable:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTDirectFraud:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTApplicationDeviceNotOnBlacklist:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTNoSuspiciousApplicationActivity:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTEmployedMask:
                    //To do
                    break;

                case RiskMaskForDeclinedLoan.TESTApplicationElementNotCIFASFlagged:
                    //To do
                    break;*/

            }
            // TODO: add data preparation for other risk masks

            return cust;
        }

        private DeclinedPage RunL0Journey(Cust customer, RiskMaskForDeclinedLoan riskMask)
        {
            Client.Driver.Manage().Window.Maximize();
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmail(customer.email)
                .WithFirstName(customer.FirstName)
                .WithLastName(customer.LastName)
                .WithEmployerName(Get.EnumToString(riskMask))
                .WithDeclineDecision();
            var declinedPage = journeyL0.Teleport<DeclinedPage>() as DeclinedPage;
            return declinedPage;

        }

        private void VerifyDeclineAdviceText(RiskMaskForDeclinedLoan riskMask, DeclinedPage declinedPage)
        {
            switch (riskMask)
            {
                case RiskMaskForDeclinedLoan.TESTBlacklist:
                    Assert.AreEqual(@ContentMap.Get.L0DeclinedPage.ApplicationBlacklistCheck.Replace("\'", "'").Replace("\\r\\n", "\r\n"), declinedPage.DeclineAdvice());
                    break;

                case RiskMaskForDeclinedLoan.TESTCreditBureauDataIsAvailable:
                    string expMessage = (@ContentMap.Get.L0DeclinedPage.CreditBureauDataIsAvailable.Replace("\'","'").Replace("\\r\\n", "\r\n"));
                    Assert.AreEqual(expMessage + _creditAgenciesContactDetails, declinedPage.DeclineAdvice().Replace("\'t", "'t"));
                    break;
            }
        }

        #endregion

    }
}

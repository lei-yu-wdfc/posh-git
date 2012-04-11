﻿using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;


namespace Wonga.QA.Tests.Ui
{
    public class LoanStatusMessageTests : UiTest
    {

        Dictionary<int, string> loanStatusMessages = new Dictionary<int, string> 
	    {
	    {2, @"If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do that below. Please bear in mind that you will need to pay any interest and fees up to that point, in order for a new date to be approved."},
        {3, @"If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {4, @"Please also remember you have promised to repay £{total to repay 300.00} on {promise date}, when you simply need to ensure the funds are available in the bank account linked to your primary debit card. Changing your promise date isn't possible at this point, so we look forward to collecting your payment and then being of service again in the future."},
        {5, @"If you would like to change your repayment date it's too early to do it just yet, but you can request a new one from {date extensions available}. You can set a handy reminder to do it below. Please bear in mind that you will need to pay any interest and fees up to that point, in order to have your request for a change of promise date approved."},
        {6, @"If you would like to change your promised repayment date you can do so here. Please note you can only extend your promise date a maximum of three times and will need to pay any interest and fees up to that point each time you extend, in order to have any request approved."},
        {7, @"Changing your promise date isn't possible at this point, so we look forward to collecting your full payment and then being of service again in the future. Thanks for using Wonga!"},
        {9, @"We understand genuine mistakes happen so we hope you can make this payment today and save yourself further costs. If the balance isn't cleared by 5pm today, however, you will incur a missed payment fee of £20, which is the last thing we want to happen! Please click Repay now to settle your balance. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {10, @"You have unfortunately incurred a missed payment fee of £20 and interest continues to accrue. Please click Repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. If you are unable to pay in full today, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {11, @"Please act now to avoid incurring further interest, which continues to accrue. Please click Repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of four months. If this doesn't work for you, you should call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {12, @"Please act now to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Please click Repay now to settle your balance and bring your account back into line. You can add a new debit card if you need to. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday."},
        {13, @"Please take action today to avoid incurring further interest, which continues to accrue, and potential negative entries on your credit file. Click Repay now to settle your balance and bring your account back into line. Alternatively, we will freeze your balance today if you set up an acceptable repayment plan. Please use the self-service function below to repay over a maximum of six months. If this doesn't work for you, please call our friendly collections team straight away on 0844 842 9109. If you choose not to deal with this matter immediately, we may need to take more formal steps to recover the balance owed."},
        {15, @"Please make this payment online right away, or call our automated payment line on 0207 183 0063 to resolve this situation quickly. If you don't make this payment by {deadline for grace period} your plan will be cancelled automatically and interest will start to accrue again, which is the last thing we want to happen! Please act today to avoid further costs."},
        {16, @"Your account is seriously in arrears and interest is again accruing . Your current balance is £{total balance today £230.45}. Please click Repay now to settle in full and rectify this situation. If you are unable to pay in full today, please call our collections team straight away on 0844 842 9109. We're here between 9am and 10pm, Monday to Friday. If you choose not to deal with this matter immediately, we may need to take more formal steps to recover the balance owed."},
        {17, @"Your application is in the final stages of our approval process. We hate to keep you waiting, but, on this rare occasion, we need to check a few more details. There's no need to contact us or do anything and you should hear back from us {within the next 6 hours}. You can also check for updates about your application by logging into your account. As soon as we complete our checks, we will email you and send you a text message, so thanks for your patience in the meantime.\nIf approved you will just need to come back to the site and click the ‘I accept’ button on your agreement and we will then send the money to your bank within 15 minutes."},
        {19, @"You informed us that you wanted to cancel your credit agreement please contact us on {CS tel. No} to complete this process by making the required repayment."},
        {21, @"One last step to receive your cash.\n\nYour application has been approved! Now you just need to read and accept your new agreement and the loan conditions by clicking the ‘I accept’ button in the agreement below. You will then receive {£loan amount} in your account.\n\nWe’ll then collect {£xx.xx total repayable on due date} from your debit card on {repayment date in format 15th March 2011.}\n\nThanks for using Wonga!"},
	    };



        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario2() { LoanStatusMessage(2, 1); } 

        [Test, AUT(AUT.Uk), JIRA("UK-795")] 
        public void LoanStatusMessageScenario3() { LoanStatusMessage(3, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario4() { LoanStatusMessage(4, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")] 
        public void LoanStatusMessageScenario5() { LoanStatusMessage(5, 2); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario6() { LoanStatusMessage(6, 3); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario7() { LoanStatusMessage(7, 10); }

        //[Test, AUT(AUT.Uk), JIRA("UK-795")]
        //public void LoanStatusMessageScenario9() { LoanStatusMessage(9, 3); } // not ready

        //[Test, AUT(AUT.Uk), JIRA("UK-795")]
        //public void LoanStatusMessageScenario10() { LoanStatusMessage(10, 11); } // not ready

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario11() { LoanStatusMessage(11, 14); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario12() { LoanStatusMessage(12, 44); }

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario13() { LoanStatusMessage(13, 64); }

        //[Test, AUT(AUT.Uk), JIRA("UK-795")]
        //public void LoanStatusMessageScenario15() { LoanStatusMessage(15, 11); } // not ready

        //[Test, AUT(AUT.Uk), JIRA("UK-795")]
        //public void LoanStatusMessageScenario16() { LoanStatusMessage(16, 11); } // not ready

        [Test, AUT(AUT.Uk), JIRA("UK-795")]
        public void LoanStatusMessageScenario17() { LoanStatusMessage(17, 0); } // hangs?

        //[Test, AUT(AUT.Uk), JIRA("UK-795")]
        //public void LoanStatusMessageScenario19() { LoanStatusMessage(19, 0); } // not ready

        //[Test, AUT(AUT.Uk), JIRA("UK-795")]
        //public void LoanStatusMessageScenario21() { LoanStatusMessage(21, 0); } // not ready


        private void LoanStatusMessage(int scenarioId, int daysShift)
        {
            // Create a customer
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            Customer customer;
            if (scenarioId == 20)
            {
                customer = CustomerBuilder.New().WithEmailAddress(email).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithMiddleName(RiskMask.TESTEmployedMask).Build();
            }
            else
            {
                customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            }

            // Create a loan
            Application application;
            if (scenarioId < 5) application = ApplicationBuilder.New(customer).Build();
            else if ((scenarioId >= 5) && (scenarioId <= 7))
                application = ApplicationBuilder.New(customer).WithLoanAmount(400).Build();
            else if (scenarioId == 17)
                application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).Build(); //hangs
            else if (scenarioId == 20)
                application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build(); //hangs
            else
            {
                application = ApplicationBuilder.New(customer).Build();
            }

            // Rewind application dates
            ApplicationEntity applicationEntity =
                Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication =
                Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);

            //application.PutApplicationIntoArrears(daysShift); // throws exception; alternative to the code above?

            // Repay a loan
            if (scenarioId == 8) application = application.RepayOnDueDate();

            // Create repayment plan
            if ((scenarioId >= 14) && (scenarioId <= 16))
            {
                Drive.Msmq.Payments.Send(new CreateExtendedRepaymentArrangementCommand
                {
                    AccountId = customer.Id,
                    ApplicationId = application.Id,
                    EffectiveBalance = application.GetBalance(),
                    RepaymentAmount = application.GetBalance(),
                    RepaymentDetails = new[]
						{
							new ArrangementDetail{Amount = application.GetBalance(), Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today.AddDays(7)}
						}
                });

                /* THIS IS ALTERNATIVE TO CODE ABOVE BUT THIS THROWS EXCEPTION
                 * 
                 * var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
                 * Thread.Sleep(10000);
                 * var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
                 * Assert.AreEqual(1, repaymentArrangement.RepaymentArrangementDetails.Count);
                 * 
                 */
                

            }



            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);


            // Check Tag Cloud is displayed/correct
            if ((scenarioId == 8) || (scenarioId == 20))
            {
                Assert.IsFalse(mySummaryPage.IsLoanStatusMessageAvailable());
                return;
            }

            string expectedLoanMessageText = loanStatusMessages[scenarioId];
            if ((scenarioId == 2) || (scenarioId == 5))
            {
                var extensionStartDate = applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value.AddDays(-7);
                if (extensionStartDate >= applicationEntity.SignedOn.Value)
                {
                    expectedLoanMessageText = expectedLoanMessageText.Replace("{date extensions available}", extensionStartDate.ToString("dd-MMM-yyyy"));    
                }
                else // if loan period is less than 7 days
                {
                    expectedLoanMessageText = loanStatusMessages[3];  // can extend right now  
                }
            }
            string actualLoanMessageText = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanMessageText, actualLoanMessageText);
        }

        private static void RewindApplicationDates(ApplicationEntity application, RiskApplicationEntity riskApp, TimeSpan span)
        {
            application.ApplicationDate -= span;
            application.SignedOn -= span;
            application.CreatedOn -= span;
            application.AcceptedOn -= span;
            application.FixedTermLoanApplicationEntity.PromiseDate -= span;
            application.FixedTermLoanApplicationEntity.NextDueDate -= span;
            application.Transactions.ForEach(t => t.PostedOn -= span);
            if (application.ClosedOn != null)
                application.ClosedOn -= span;
            application.Submit(true);

            riskApp.ApplicationDate -= span;
            riskApp.PromiseDate -= span;
            if (riskApp.ClosedOn != null)
                riskApp.ClosedOn -= span;
            riskApp.Submit(true);

        }

        //Needed for serialization in CreateExtendedRepaymentArrangementCommand
        private class ArrangementDetail
        {
            public decimal Amount { get; set; }
            public CurrencyCodeIso4217Enum Currency { get; set; }
            public DateTime DueDate { get; set; }
        }

        /*private void LoanStatusMessage_old(string message, int days) // outdated
        {
            string email = Get.RandomEmail();

            Console.WriteLine("email:{0}", email);

            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).Build();

            // Hack DB to change "NextDueDate" related fields
            ApplicationEntity applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            TimeSpan span = TimeSpan.FromDays(days);
            application.MoveDueDates(applicationEntity, span);
            
            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check Loan Status message
            string expectedLoanStatusMessage = message;
            //string actualLoanStatusMessage = mySummaryPage.WarningBox.GetLoanStatusMessage; // outdated
            string actualLoanStatusMessage = mySummaryPage.GetLoanStatusMessage;
            Assert.AreEqual(expectedLoanStatusMessage, actualLoanStatusMessage);
        }*/
    }
}
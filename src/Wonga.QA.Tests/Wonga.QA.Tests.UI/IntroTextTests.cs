using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Tests.Ui
{
    [Parallelizable]
    public class IntroTextTests: UiTest
    {
        Dictionary<int, string> introTexts = new Dictionary<int, string> 
	    {
	    {1, "Hi {first name}. Your current trust rating means you can apply for up to £{500} below."},
        {2, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at anytime."},
        {3, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at anytime."},
        {4, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at anytime."},
        {5, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {6, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {7, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {8, "Hi {first name}. We collected your full payment of £{300} today, as promised.Many thanks for repaying our trust in you.You can now request your available credit up to your current Wonga trust rating of £{500}, at anytime. Thanks for using Wonga and we hope we can help again in the future."},
        {9, "Hi {first name}. Your promised repayment of £{456.34}, due first thing today, was declined by your bank."},
        {10, "Hi {first name}. Your repayment of £{456.34}, promised on {date}, was declined by your bank and is now overdue."},
        {11, "Hi {first name}. Your account is now {26} days in arrears."},
        {12, "Hi {first name}. We are dissapointed that your account remains overdue and you are now {46} days in arrears."},
        {13, "Hi {first name}. We are dissapointed that your account remains overdue and you are now {61} days in arrears."},
        {14, "Hi {first name}. We are dissapointed that your account remains overdue and you are now {61} days in arrears."},
        {15, "Hi {first name}. We were unable to collect a scheduled repayment towards your current repayment plan."},
        {16, "Hi {first name}. You have missed a scheduled payment against your agreed repayment plan, which has now been cancelled."},
        {17, "Hi {first name}."},
        {19, "Hi {first name}."},
        {20, "Hi {first name}. As a new customer you can apply for up to £{400} below."},
        {21, "Hi {first name}."}
	    };

        /// <summary>
        /// Tests
        /// </summary>
        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario1A()
        {
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home());          
            var aPage = journey.ApplyForLoan(loanAmount, days)
                .FillPersonalDetailsWithEmail(Get.EnumToString(RiskMask.TESTEmployedMask), email)
                .FillAddressDetails()
                .FillAccountDetails();
 
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
                
            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedlntroText = introTexts[1];
            expectedlntroText = expectedlntroText.Replace("{first name}", journey.FirstName).Replace("{500}", "400"); 
            Assert.AreEqual(expectedlntroText, actuallntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario1B()
        {

            IntroText(1, 0);
        }

        /// <summary>
        /// Main method that create a scenario and checks the Introduction message.
        /// Used by tests.
        /// </summary>
        private void IntroText (int scenarioId, params int[] days)
        {
            int daysShift = days[0];
            int loanTerm = 10;  // Loan Term by default
            string expectedlntroText = introTexts[scenarioId]; 
            if (days.Length == 2) loanTerm = days[1]; // initialize the explicitely defined Loan Term
            ApplicationEntity applicationEntity = null;
            Application application = null;

            // Create a customer
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            Customer customer;
            if (scenarioId == 20)
                customer = CustomerBuilder.New().WithEmailAddress(email).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithMiddleName(RiskMask.TESTEmployedMask).Build();
           else
                customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            expectedlntroText = expectedlntroText.Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]); // replace first name in the expected text
            

            // Create a loan
            
            if (scenarioId < 5)
                application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();
            else if ((scenarioId >= 5) && (scenarioId <= 7))
                application = ApplicationBuilder.New(customer).WithLoanAmount(400).WithLoanTerm(loanTerm).Build();
            else if (scenarioId == 17)
                application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Pending).WithLoanTerm(loanTerm).Build();
            else if (scenarioId == 20)
                application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).WithLoanTerm(loanTerm).Build();
            else
                application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            // Rewind application dates
            applicationEntity = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            RiskApplicationEntity riskApplication = Drive.Db.Risk.RiskApplications.Single(r => r.ApplicationId == application.Id);
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);

            RewindApplicationDates(applicationEntity, riskApplication, daysShiftSpan);               
            

            // Repay a loan
            if ((scenarioId == 8) || (scenarioId == 1))  if (application != null) application = application.RepayOnDueDate();

            // Create repayment plan
            /* Does it work?
            * if ((scenarioId >= 14) && (scenarioId <= 16))
            {
                Drive.Msmq.Payments.Send(new CreateExtendedRepaymentArrangementCommand
                {
                    AccountId = customer.Id,
                    ApplicationId = application.Id,
                    EffectiveBalance = application.GetBalance(),
                    RepaymentAmount = application.GetBalance(),
                    RepaymentDetails = new[]
						{
							new LoanStatusMessageTests.ArrangementDetail{Amount = application.GetBalance(), Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today.AddDays(7)}
						}
                });
            }*/

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            if ((scenarioId == 2) || (scenarioId == 5))
                if (application.LoanTerm > 7)
                {
                    var extensionStartDate = applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value.AddDays(-7);
                    expectedlntroText = expectedlntroText.Replace("{date extensions available}", getOrdinalDate(extensionStartDate));
                }
                else // if loan period is less than 7 days
                {
                    expectedlntroText = introTexts[3];  // can extend right now  
                }
            

            if (scenarioId == 4)
            {
                //{total to repay 300.00} on {promise date}
                expectedlntroText = expectedlntroText.Replace("{total to repay 300.00}", application.GetDueDateBalance().ToString("#.##")).Replace("{promise date}", getOrdinalDate(applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value));
            }

            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            Assert.AreEqual(expectedlntroText, actuallntroText);
        }

        private static void RewindApplicationDates(Framework.Db.Payments.ApplicationEntity application, RiskApplicationEntity riskApp, TimeSpan span)
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

        public string getOrdinalDate(DateTime date)
        {
            //Returns date as string in the format "Wed 18th Apr 2012"
            var cDate = date.Day.ToString("d")[date.Day.ToString("d").Length - 1];
            string suffix;
            switch (cDate)
            {
                case '1':
                    suffix = "st";
                    break;
                case '2':
                    suffix = "nd";
                    break;
                case '3':
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }

            return date.ToString("ddd d MMM yyyy").Replace(date.Day.ToString("d"), date.Day.ToString("d") + suffix);
        }
    }
}


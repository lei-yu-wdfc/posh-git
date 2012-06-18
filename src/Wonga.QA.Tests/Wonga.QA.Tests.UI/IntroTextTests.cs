using System;
using System.Collections.Generic;
using System.Globalization;
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
using Wonga.QA.Tests.Payments.Helpers;
using EmploymentStatusEnum = Wonga.QA.Framework.Msmq.EmploymentStatusEnum;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Db.Extensions;
using CreateScheduledPaymentRequestCommand = Wonga.QA.Framework.Msmq.CreateScheduledPaymentRequestCommand;


namespace Wonga.QA.Tests.Ui
{
    [Parallelizable(TestScope.All)]
    public class IntroTextTests: UiTest
    {
        Dictionary<int, string> introTexts = new Dictionary<int, string> 
	    {
	    {1, "Hi {first name}. Your current trust rating means you can apply for up to £{500} below."},
        {2, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at any time."},
        {3, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at any time."},
        {4, "Hi {first name}. You currently have up to £{245.00} of available credit which you can request at any time."},
        {5, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {6, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {7, "Hi {first name}. You don't currently have any available credit and you promised to repay £{425.00} on {Friday 13 Feb 2011}."},
        {8, "Hi {first name}. We collected your full payment of £{300} today, as promised.Many thanks for repaying our trust in you.You can now request your available credit up to your current Wonga trust rating of £{500}, at anytime. Thanks for using Wonga and we hope we can help again in the future."},
        {9, "Hi {first name}. Your promised repayment of £{456.34}, due first thing today, was declined by your bank."},
        {10, "Hi {first name}. Your repayment of £{456.34}, promised on {date}, was declined by your bank and is now overdue."},
        {11, "Hi {first name}. Your account is now {26} days in arrears."},
        {12, "Hi {first name}. We are disappointed that your account remains overdue and you are now {46} days in arrears."},
        {13, "Hi {first name}. We are disappointed that your account remains overdue and you are now {61} days in arrears."},
        {14, "Hi {first name}. You have commited to a repayment plan with us, which will bring your account back into line. Maintaining your promised payments is essential, details of which are below. If you know you will be unable to honour a future payment, please email collections@wonga.com at least two days before your next instalment is due."},
        {15, "Hi {first name}. We were unable to collect a scheduled repayment towards your current repayment plan."},
        {16, "Hi {first name}. You have missed a scheduled payment against your agreed repayment plan, which has now been cancelled."},
        {17, "Hi {first name}."},
        {19, "Hi {first name}."},
        {20, "Hi {first name}. As a new customer you can apply for up to £{400} below."},
        {21, "Hi {first name}."}
	    };

        [Test, AUT(AUT.Uk), JIRA("UK-788", "UK-1614"), Pending("The variant of scenario 1 is being implemented.")]
        public void IntroTextScenario1A()
        {
            string firstName = Get.RandomString(3, 10);
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithFirstName(firstName)
                .WithEmail(email)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.ApplyForLoan()
                .FillPersonalDetails()
                .FillAddressDetails()
                .FillAccountDetails();
 
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);
                
            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[1];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName).Replace("{500}", "400.00");
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(1));
            Assert.AreEqual(expectedIntroText, actuallntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario1B()
        // Ln journey
        {
            string email = Get.RandomEmail();
            const decimal trustRating = 400.00M;
            var applicationId = Guid.NewGuid();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var accountId = customer.Id;

            var setupData = new AccountSummarySetupFunctions();

            setupData.Scenario01Setup(accountId, applicationId, trustRating);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
                Drive.Api.Queries.Post(new GetAccountOptionsUkQuery {AccountId = accountId, TrustRating = trustRating});
            Assert.AreEqual(1, int.Parse(response.Values["ScenarioId"].Single()));

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[1];
            expectedIntroText = expectedIntroText.Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]).Replace("{500}", "400.00");
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(1));
            Assert.AreEqual(expectedIntroText, actuallntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        [Row(2,0)]
        [Row(2,1)]
        [Row(2,2)]
        public void IntroTextScenario2(int scenarioId, int dayShift) { IntroText(scenarioId, dayShift); }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        [Row(3, 3, 10)]
        [Row(3, 7, 10)]
        [Row(3, 8, 10)]
        [Row(3, 0, 7)] //0 days passed in 7-day loan
        public void IntroTextScenario3(int scenarioId, int dasyShift, int loanTerm)
        {
            IntroText(scenarioId, dasyShift, loanTerm);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788"), Pending("Check that after 3 extensions we get scenario 4. Passed manually.")]
        // [Row(4, 2, 7)]
        public void IntroTextScenario4(int scenarioId, int dasyShift, int loanTerm)
        {
            IntroText(scenarioId, dasyShift, loanTerm);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788","UK-1909")]
        [Row(5, 2)]    
        public void IntroTextScenario5(int scenarioId, int dasyShift)
        {
            IntroText(scenarioId, dasyShift);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        [Row(6, 0, 1)] //0 days passed in 1-day loan
        [Row(6, 3, 10)] //3 days passed in 10-day loan
        [Row(6, 6, 7)]  //6 days passed in 7-day loan
        public void IntroTextScenario6(int scenarioId, int dasyShift, int loanTerm)
        {
            IntroText(scenarioId, dasyShift, loanTerm);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        [Row(7, 10)]
        public void IntroTextScenario7(int scenarioId, int dasyShift)
        {
            IntroText(scenarioId, dasyShift);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        [Row(8, 10), Pending(" UK-1913 Waiting for implementation of GetRepayLoanStatus")]
        public void IntroTextScenario8(int scenarioId, int dasyShift)
        {
            IntroText(scenarioId, dasyShift);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario9()
        {
            const int scenarioId = 9;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = customer.BankAccountId;
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario09Setup(requestId2, requestId1, accountId, paymentCardId, appId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
                Drive.Api.Queries.Post(new GetAccountOptionsUkQuery {AccountId = accountId, TrustRating = trustRating});
            Assert.AreEqual(9, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);


            response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = appId });
            var expectedNextDueDateRepay = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());

            var expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            expectedIntroText = expectedIntroText.Replace("Your promised repayment of £{456.34}", "Your promised repayment of £" + expectedNextDueDateRepay.ToString("#.00"));
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actualIntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario10()
        {
            const int scenarioId = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario10Setup(requestId1, requestId2, appId, bankAccountId, accountId, paymentCardId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
                Drive.Api.Queries.Post(new GetAccountOptionsUkQuery {AccountId = accountId, TrustRating = trustRating});
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);


            response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = appId });
            var expectedBalanceToday = String.Format("{0:0.00}", Convert.ToDecimal(response.Values["BalanceToday"].Single()));
            var expectedNextDueDate = Date.GetOrdinalDate(Convert.ToDateTime(response.Values["NextDueDate"].Single()), "ddd d MMM yyyy");

            //Your repayment of £{456.34}, promised on {date}
            var expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            expectedIntroText = expectedIntroText.Replace("Your repayment of £{456.34}", "Your repayment of £" + expectedBalanceToday);
            expectedIntroText = expectedIntroText.Replace("promised on {date}", "promised on " + expectedNextDueDate);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actualIntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        [Row(11, 13)]
        [Row(11, 14)]
        [Row(11, 40)]
        public void IntroTextScenario11(int scenarioId, int dasyShift)
        {
            IntroText(scenarioId, dasyShift);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788", "UK-1954")]
        [Row(12, 41)]
        [Row(12, 42)]
        [Row(12, 70)]
        public void IntroTextScenario12(int scenarioId, int dasyShift)
        {
            IntroText(scenarioId, dasyShift);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788", "UK-1966")]
        [Row(13, 71)]
        [Row(13, 72)]
        [Row(13, 100)]
        [Row(13, 1000)]
        public void IntroTextScenario13(int scenarioId, int dasyShift)
        {
            IntroText(scenarioId, dasyShift);
        }


        [Test, AUT(AUT.Uk), JIRA("UK-788"), Pending("Awating Repayment Arrangment Functionality.")]
        public void IntroTextScenario14()
        {
            const int scenarioId = 14;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario14Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
                Drive.Api.Queries.Post(new GetAccountOptionsUkQuery {AccountId = accountId, TrustRating = trustRating});
            Assert.AreEqual(scenarioId, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            
            string expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actualIntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788"), Pending("Awating Repayment Arrangment Functionality.")]
        public void IntroTextScenario15()
        {
            const int scenarioId = 15;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario15Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
                Drive.Api.Queries.Post(new GetAccountOptionsUkQuery {AccountId = accountId, TrustRating = trustRating});
            Assert.AreEqual(15, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actualIntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788"), Pending("Awating Repayment Arrangment Functionality.")]
        public void IntroTextScenario16()
        {
            const int scenarioId = 16;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);
            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var accountId = customer.Id;
            var bankAccountId = Guid.NewGuid();
            var paymentCardId = Guid.NewGuid();
            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            var appId = Guid.NewGuid();
            const int applicationId = 0;
            const decimal trustRating = 400.00M;

            var setupData = new AccountSummarySetupFunctions();
            setupData.Scenario16Setup(requestId1, requestId2, applicationId, accountId, appId, paymentCardId, bankAccountId);

            var response = Drive.Api.Queries.Post(new GetAccountOptionsUkQuery { AccountId = accountId, TrustRating = trustRating });
            Assert.AreEqual(16, int.Parse(response.Values["ScenarioId"].Single()), "Incorrect ScenarioId");

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            string expectedIntroText = introTexts[scenarioId].Replace("{first name}", customer.GetCustomerFullName().Split(' ')[0]);
            string actualIntroText = mySummaryPage.GetIntroText;
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actualIntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario17()
        {
            string firstName = Get.RandomString(3, 10);
            var scenarioId = 17;
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithFirstName(firstName).WithEmail(email)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.ApplyForLoan()
                .FillPersonalDetails()
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[17];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName);
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actuallntroText);
        }

        [Test, AUT(AUT.Uk), JIRA("UK-788"), Pending("Waiting for implementation of agreement cancellation process.")]
        public void IntroTextScenario19() { }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario20() { IntroText(20, 10); }

        [Test, AUT(AUT.Uk), JIRA("UK-788")]
        public void IntroTextScenario21()
        {
            string firstName = Get.RandomString(3, 10);
            var scenarioId = 21;
            const int loanAmount = 100;
            const int days = 10;
            string email = Get.RandomEmail();
            Console.WriteLine("email:{0}", email);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithFirstName(firstName).WithEmail(email)
                .WithAmount(loanAmount).WithDuration(days);
            var aPage = journey.ApplyForLoan()
                .FillPersonalDetails()
                .FillAddressDetails()
                .FillAccountDetails()
                .FillBankDetails()
                .FillCardDetails();

            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email);

            // Check the actual text
            string actuallntroText = mySummaryPage.GetIntroText;
            string expectedIntroText = introTexts[21];
            expectedIntroText = expectedIntroText.Replace("{first name}", firstName);
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actuallntroText);
        }

        private void IntroText(int scenarioId, params int[] days)
        {
            int daysShift = days[0];
            int loanTerm = 10;
            if (days.Length == 2) loanTerm = days[1];

            // Create a customer
            string email = Get.RandomEmail();
            Customer customer;
            if (scenarioId == 20) customer = CustomerBuilder.New().WithEmailAddress(email).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithMiddleName(RiskMask.TESTEmployedMask).Build();
            else customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            // Create a loan
            Application application;
            if (scenarioId < 5) application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();
            else if ((scenarioId >= 5) && (scenarioId <= 7)) application = ApplicationBuilder.New(customer).WithLoanAmount(400).WithLoanTerm(loanTerm).Build();
            else if (scenarioId == 20) application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).WithLoanTerm(loanTerm).Build();
            else application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).Build();

            // Rewind application dates
            if (daysShift != 0)
            { 
                TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);

                ApplicationOperations.RewindApplicationDates(application,daysShiftSpan);
            }

            var expectedDueDateBalance = 0.00M;
            var expectedAmountMax = "0";
            if (scenarioId == 8)
            {
                expectedDueDateBalance = application.GetDueDateBalance();
                //TBD: get expectedAmountMax 
                //GetFixedTermLoanOfferResponse
                var PromoCodeId = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id }).Values["PromoCodeId"].Single();
                expectedAmountMax = Drive.Api.Queries.Post(new GetFixedTermLoanOfferUkQuery { AccountId = customer.Id, PromoCodeId = PromoCodeId }).Values["AmountMax"].Single();
                expectedAmountMax =
                    Drive.Api.Queries.Post(new GetFixedTermLoanOfferUkQuery
                                               {AccountId = customer.Id, PromoCodeId = PromoCodeId}).Values["AmountMax"]
                        .Single();
                application = application.RepayOnDueDate(); // Repay a loan
            }

            if (scenarioId == 20)
            {
                expectedAmountMax = Drive.Api.Queries.Post(new GetAccountSummaryQuery{AccountId = customer.Id}).Values["AvailableCredit"].Single();
                    Drive.Api.Queries.Post(new GetAccountSummaryQuery {AccountId = customer.Id}).Values[
                        "AvailableCredit"].Single();
                expectedAmountMax = String.Format("{0:0.00}", Convert.ToDecimal(expectedAmountMax));
            }

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            if (scenarioId == 11 || scenarioId == 12 || scenarioId == 13)
            {
                // Send command to create scheduled payment request
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
                Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequestCommand() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });
            }

            // Login and open my summary page
            var loginPage = Client.Login();
            var mySummaryPage = loginPage.LoginAs(email); // My Summary page opens

            
            // Check the scenario returned by Back-End is correct
            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));

            // Check the actual text
            string expectedIntroText = introTexts[scenarioId].Replace("{first name}",
                                                                      customer.GetCustomerFullName().Split(' ')[0]);

            var response = Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.Id });
            var expectedAvailableCredit = Convert.ToDecimal(response.Values["AvailableCredit"].Single());
            expectedIntroText = expectedIntroText.Replace("You currently have up to £{245.00}", "You currently have up to £" + String.Format("{0:0.00}", expectedAvailableCredit));
            

            var expectedNextDueDateRepay = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());
            var dExpectedNextDueDate = Convert.ToDateTime(response.Values["NextDueDate"].Single());
            var expectedNextDueDate = Date.GetOrdinalDate(dExpectedNextDueDate, "ddd d MMM yyyy");
            TimeSpan daysInArrears = DateTime.Today - dExpectedNextDueDate;


            expectedIntroText = expectedIntroText.Replace("you promised to repay £{425.00}", "you promised to repay £" + String.Format("{0:0.00}", expectedNextDueDateRepay));
            expectedIntroText = expectedIntroText.Replace("on {Friday 13 Feb 2011}", "on " + expectedNextDueDate);
            expectedIntroText = expectedIntroText.Replace("We collected your full payment of £{300} today", "We collected your full payment of £" + expectedDueDateBalance + " today");
            expectedIntroText = expectedIntroText.Replace("your current Wonga trust rating of £{500}", "your current Wonga trust rating of £" + expectedAmountMax);
            expectedIntroText = expectedIntroText.Replace("Your account is now {26}", "Your account is now " + daysInArrears.Days.ToString("#"));
            expectedIntroText = expectedIntroText.Replace("you are now {46}", "you are now " + daysInArrears.Days.ToString("#"));
            expectedIntroText = expectedIntroText.Replace("you are now {61}", "you are now " + daysInArrears.Days.ToString("#"));
            expectedIntroText = expectedIntroText.Replace("you can apply for up to £{400}", "you can apply for up to £" + expectedAmountMax);
             
            string actuallntroText = mySummaryPage.GetIntroText;

            Assert.IsTrue(mySummaryPage.IsBackEndScenarioCorrect(scenarioId));
            Assert.AreEqual(expectedIntroText, actuallntroText);
        }
    }
}
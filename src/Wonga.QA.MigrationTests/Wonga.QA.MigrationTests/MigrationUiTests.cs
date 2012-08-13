using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.MigrationTests.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;


namespace Wonga.QA.MigrationTests
{
    [TestFixture]
    //[Parallelizable(TestScope.All)]
    public class MigrationUiTests : UiTest
    {
        private readonly MigrationHelper _migHelper = new MigrationHelper();
        private readonly AcceptanceTestResults _acceptanceTestResults = new AcceptanceTestResults();

        private string GetFunctionName()
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();

            return methodBase.Name;
        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            if (_migHelper.IsControlTableEmpty())
            {
                _migHelper.FillAcceptanceTestControlTable();
            }
            _acceptanceTestResults.TestStartDate = DateTime.Now;
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-243"), /*Parallelizable,*/ Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanLogin()
        {

            while (_migHelper.KeepRunningTests())
            {
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        var loginPage = Client.Login();

                        loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                          _acceptanceTestResults.MigratedUser.Password);

                        //set test result as 1 to show test passed.
                        _acceptanceTestResults.TestResult = 1;
                    }
                    catch (Exception)
                    {
                        _acceptanceTestResults.TestResult = 0;
                    }
                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }
                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-226"), Owner(Owner.MuhammadQureshi), Timeout(9999)]
        public void TestMigratedUserCanTakeANewLoan()
        {
            while (_migHelper.KeepRunningTests())
            {
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        var loginPage = Client.Login();

                        #region uk wip login

                        //var email = CustomerBuilder.RandomLnCustomerEmail();
                        //loginPage.LoginAs(email, "Passw0rd");

                        #endregion

                        loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                          _acceptanceTestResults.MigratedUser.Password);
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(10).WithDuration(1);
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;

                        //set test result as 1 to show test passed.
                        _acceptanceTestResults.TestResult = 1;
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }
                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-229"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanExtendLoan()
        {
            while (_migHelper.KeepRunningTests())
            {
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(10).WithDuration(1);

                        //go to my summary page and click the change promise button
                        mySummaryPage.ChangePromiseDateButtonClick();
                        var requestPage = new ExtensionRequestPage(this.Client);

                        var application =
                            Drive.Db.Payments.Applications.Single(
                                a => a.AccountId == _acceptanceTestResults.MigratedUser.AccountId);

                        //Runs assertions internally
                        requestPage.IsExtensionRequestPageSliderReturningCorrectValuesOnChange(
                            application.ExternalId.ToString());

                        //Branch point - Add Cv2 for each path and proceed
                        requestPage.setSecurityCode("123");
                        requestPage.SubmitButtonClick();

                        var extensionProcessingPage = new ExtensionProcessingPage(this.Client);

                        var agreementPage =
                            extensionProcessingPage.WaitFor<ExtensionAgreementPage>() as ExtensionAgreementPage;
                        agreementPage.Accept();

                        var dealDonePage = new ExtensionDealDonePage(this.Client);
                        Assert.IsFalse(dealDonePage.IsDealDonePageExtensionAmountNotPresent());
                        Assert.IsFalse(dealDonePage.IsDealDonePageDateTokenPresent());

                        //set test result as 1 to show test passed.
                        _acceptanceTestResults.TestResult = 1;
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }

                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-229"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanTopUp()
        {
            while (_migHelper.KeepRunningTests())
            {
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(150).WithDuration(7);
                        //this completes the Ln journey
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;

                        _acceptanceTestResults.TestParameters.ParametersUsed =
                            "Loan Amount = £150, Loan Duration = 7 Days";

                        var responseLimit =
                            Drive.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery { AccountId = _acceptanceTestResults.MigratedUser.AccountId });
                        int _amountMax =
                            (int)
                            Decimal.Parse(responseLimit.Values["AmountMax"].Single(), CultureInfo.InvariantCulture);
                        int _amountMin = 1;

                        int randomAmount = _amountMin + (new Random()).Next(_amountMax - _amountMin);

                        /* not sure if this should be removed or left in 
                        var loginPage = Client.Login();
                        var mySummaryPage = loginPage.LoginAs(email);
                        */

                        decimal topupAmountDec = (decimal)randomAmount;
                        var topupAmount = randomAmount.ToString();

                        mySummaryPage.TopupSliders.HowMuch = topupAmount;

                        //get application id from payments table (called external id in that table)
                        var application =
                            Drive.Db.Payments.Applications.Single(
                                a => a.AccountId == _acceptanceTestResults.MigratedUser.AccountId);

                        ApiResponse _response =
                            Drive.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery { ApplicationId = application.ExternalId, TopupAmount = topupAmountDec });
                        var totalRepayable = _response.Values["TotalRepayable"].Single();
                        var interestAndFees = _response.Values["InterestAndFeesAmount"].Single();

                        Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalToRepay.Remove(0, 1), totalRepayable,
                                        "Total to Repay on Sliders on My Summary page is wrong");
                        Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalAmount.Remove(0, 1), topupAmount,
                                        "Total Amount on Sliders on My Summary page is wrong");
                        Assert.AreEqual(mySummaryPage.TopupSliders.GetTotalFees.Remove(0, 1), interestAndFees,
                                        "Interest Fees on Sliders on My Summary page is wrong");

                        var requestPage =
                            mySummaryPage.TopupSliders.Apply();

                        //Runs assertions internally
                        requestPage.IsTopupRequestPageSliderReturningCorrrectValuesOnChange(application.ExternalId.ToString());

                        requestPage.SubmitButtonClick();

                        var processPage = new TopupProcessingPage(this.Client);
                        var agreementPage = processPage.WaitForAgreementPage(Client);

                        Assert.IsFalse(agreementPage.IsTopupAgreementPageDateNotPresent(),
                                       "Date on Agreement page is not displayed");
                        Assert.IsTrue(agreementPage.IsTopupAgreementPageLegalInfoDisplayed(),
                                      "Legal Info on Agreement page is not displayed");
                        Assert.IsFalse(agreementPage.IsTopupAgreementPageTopupAmountNotPresent(),
                                       "Topup Amount on Agreement page is not displayed");
                        Assert.IsFalse(agreementPage.IsTopupTotalAmountTokenBeingReplaced(),
                                       "Amount Token Agreement page is not replaced with value");

                        var dealDonePage = agreementPage.Accept();
                        Assert.IsFalse(dealDonePage.IsDealDonePageDateNotPresent(),
                                       "Date on Deal Done page is not displayed");
                        Assert.IsFalse(dealDonePage.IsDealDonePageJiffyNotPresent(),
                                       "Jiffy on Deal Done page is not displayed");
                        Assert.IsFalse(dealDonePage.IsDealDonePageTopupAmountNotPresent(),
                                       "Topup Amount on Deal Done page is not displayed");
                        Assert.Contains(dealDonePage.SucessMessage, totalRepayable,
                                        "Success Message on Deal Done page does not contain Total Repayable");
                        Assert.Contains(dealDonePage.SucessMessage, topupAmount,
                                        "Success Message on Deal Done page does not contain Total Amount");

                        dealDonePage.ContinueToMyAccount();

                        //Test my account summary page
                        Assert.IsTrue(this.Client.Driver.Url.Contains("my-account"), "My Account page was not open");
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }
                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-230"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanChangePromiseDate()
        {
            while (_migHelper.KeepRunningTests())
            {
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(10).WithDuration(1);

                        //go to my summary page and click the change promise button
                        mySummaryPage.ChangePromiseDateButtonClick();
                        var requestPage = new ExtensionRequestPage(this.Client);

                        //set test result as 1 to show test passed.
                        _acceptanceTestResults.TestResult = 1;
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }

                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-231"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanRepayEarlyFull()
        {
            while (_migHelper.KeepRunningTests())
            {
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(150).WithDuration(7);
                        //this completes the Ln journey
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;

                        _acceptanceTestResults.TestParameters.ParametersUsed =
                            "Loan Amount = £150, Loan Duration = 7 Days";


                        var requestPage = new RepayRequestPage(this.Client);

                        Assert.IsNotEmpty(requestPage.RepayCard);

                        //Branch point - Add Cv2 for each path and proceed
                        requestPage.setSecurityCode("123");
                        requestPage.SubmitButtonClick();

                        var repayProcessingPage = new RepayProcessingPage(this.Client);

                        var paymentTakenPage =
                            repayProcessingPage.WaitFor<RepayEarlyFullpaySuccessPage>() as RepayEarlyFullpaySuccessPage;

                        Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageAmountTokenNotPresent());
                        Assert.IsFalse(paymentTakenPage.IsRepayEarlyFullpaySuccessPageDateTokenNotPresent());

                        // Get the content from the Payment Taken Page
                        string paymentTakenText = paymentTakenPage.ContentArea();

                        var testTitle = "Success! Your balance has been settled in full";
                        var testMessage = "Thanks and nice work for repaying early";

                        Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testTitle),
                                      "Title is missing on Payment Taken page");
                        Assert.IsTrue(paymentTakenPage.Content.Text.Contains(testMessage),
                                      "Success Message is missing on Payment Taken page");
                        //set test result as 1 to show test passed.
                        _acceptanceTestResults.TestResult = 1;
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }

                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-231"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanRepayEarlyPart()
        {
            while (_migHelper.KeepRunningTests())
            {
                const decimal repayAmount = 100.00m;
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(150).WithDuration(7);
                        //this completes the Ln journey
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;

                        _acceptanceTestResults.TestParameters.ParametersUsed =
                            "Loan Amount = £150, Loan Duration = 7 Days, Repay Amount = 100";

                        //get application id from payments table (called external id in that table)
                        var application =
                            Drive.Db.Payments.Applications.Single(
                                a => a.AccountId == _acceptanceTestResults.MigratedUser.AccountId);

                        ApiResponse response =
                            Drive.Api.Queries.Post(new GetFixedTermLoanApplicationQuery { ApplicationId = application.ExternalId });
                        var dueDate = Convert.ToDateTime(response.Values["NextDueDate"].Single());
                        var sDueDate = Date.GetOrdinalDate(dueDate, "d MMM yyyy");
                        var oldDueDateBalance = Convert.ToDecimal(response.Values["BalanceNextDueDate"].Single());

                        Client.Login().LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                               _acceptanceTestResults.MigratedUser.Password).RepayButtonClick();
                        var requestPage = new RepayRequestPage(this.Client);

                        // Set partial payment amount, test for correct values at same time
                        requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.ExternalId.ToString(),
                                                                                           repayAmount.ToString("#"));

                        // Branch point - Add Cv2 for each path and proceed
                        requestPage.setSecurityCode("123");
                        requestPage.SubmitButtonClick();

                        var repayProcessingPage = new RepayProcessingPage(this.Client);

                        var paymentTakenPage =
                            repayProcessingPage.WaitFor<RepayEarlyPartpaySuccessPage>() as RepayEarlyPartpaySuccessPage;

                        Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageAmountTokenNotPresent());
                        Assert.IsFalse(paymentTakenPage.IsRepayEarlyPartpaySuccessPageDateTokenNotPresent());

                        // Post payment values
                        ApiResponse summaryResponse =
                            Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = _acceptanceTestResults.MigratedUser.AccountId });
                        var newDueDateAmount = summaryResponse.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();
                        var newDueDateAmountDecimal = decimal.Parse(newDueDateAmount);
                        var interestSaved = oldDueDateBalance - newDueDateAmountDecimal - repayAmount;
                        string sInterestSaved = String.Format("{0:0.00}", interestSaved);
                        string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

                        // Get the content from the Payment Taken Page
                        string paymentTakenText = paymentTakenPage.ContentArea();

                        Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Due Date Amount is wrong.");
                        Assert.IsTrue(paymentTakenText.Contains(sInterestSaved), "Interest Saved is wrong.");
                        Assert.IsTrue(paymentTakenText.Contains(sDueDate), "Due Date is wrong.");
                        Assert.IsTrue(paymentTakenText.Contains(String.Format("{0:0.00}", repayAmount)),
                                      "Repay Amount is wrong.");
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }
                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-231"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanRepayDueFull()
        {
            while (_migHelper.KeepRunningTests())
            {
                const decimal repayAmount = 100.00m;
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(150).WithDuration(7);
                        //this completes the Ln journey
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;

                        _acceptanceTestResults.TestParameters.ParametersUsed =
                            "Loan Amount = £150, Loan Duration = 7 Days, Repay Amount = 100";

                        //get application id from payments table (called external id in that table)
                        var application =
                            Drive.Db.Payments.Applications.Single(
                                a => a.AccountId == _acceptanceTestResults.MigratedUser.AccountId);
                        var daysShift = 7;

                        //time-shift loan so it's due today
                        TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
                        ApplicationOperations.RewindApplicationDates((dynamic)application, daysShiftSpan);

                        /*Not sure if this is needed or not
                        var loginPage = Client.Login();
                        var mySummaryPage = loginPage.LoginAs(email);
                        */

                        mySummaryPage.RepayButtonClick();
                        var requestPage = new RepayRequestPage(this.Client);

                        //Branch point - Add Cv2 for each path and proceed
                        requestPage.setSecurityCode("123");
                        requestPage.SubmitButtonClick();

                        var repayProcessingPage = new RepayProcessingPage(this.Client);
                        var paymentTakenPage =
                            repayProcessingPage.WaitFor<RepayDueFullpaySuccessPage>() as RepayDueFullpaySuccessPage;

                        // Get the content from the Payment Taken Page
                        string paymentTakenText = paymentTakenPage.ContentArea();

                        var testTitle = "Success! Your balance has been settled in full";
                        var testMessage =
                            "Thanks for keeping your promise. We value your custom and hope we can help again in the future.";

                        Assert.IsTrue(paymentTakenPage.Headers.Contains(testTitle), "Header is incorrect.");
                        Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }
                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-231"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanRepayDuePart()
        {
            while (_migHelper.KeepRunningTests())
            {
                const decimal repayAmount = 100.00m;
                _acceptanceTestResults.TestName = GetFunctionName();
                _acceptanceTestResults.TestStartDate = DateTime.Now;
                _acceptanceTestResults.MigratedUser = new MigratedUser();
                _acceptanceTestResults.MigratedUser = _migHelper.GetMigratedAccountLogin();
                _acceptanceTestResults.TestResult = 0;

                using (var Client = new UiClient())
                {
                    try
                    {
                        //make page object
                        var loginPage = Client.Login();

                        //login as migrated user
                        var mySummaryPage = loginPage.LoginAs(_acceptanceTestResults.MigratedUser.Login,
                                                              _acceptanceTestResults.MigratedUser.Password);

                        //Take an Ln loan
                        var journey = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(150).WithDuration(7);
                        //this completes the Ln journey
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;

                        _acceptanceTestResults.TestParameters.ParametersUsed =
                            "Loan Amount = £150, Loan Duration = 7 Days, Repay Amount = 100";

                        //get application id from payments table (called external id in that table)
                        var application =
                            Drive.Db.Payments.Applications.Single(
                                a => a.AccountId == _acceptanceTestResults.MigratedUser.AccountId);
                        var daysShift = 7;

                        //time-shift loan so it's due today
                        TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
                        ApplicationOperations.RewindApplicationDates((dynamic)application, daysShiftSpan);
                        
                        /*Not sure if this is needed or not
                        var loginPage = Client.Login();
                        var mySummaryPage = loginPage.LoginAs(email);
                        */

                        mySummaryPage.RepayButtonClick();
                        var requestPage = new RepayRequestPage(this.Client);

                        //Set partial payment amount, test for correct values at same time
                        requestPage.IsRepayRequestPageSliderReturningCorrectValuesOnChange(application.ExternalId.ToString(), "100");

                        //Branch point - Add Cv2 for each path and proceed
                        requestPage.setSecurityCode("123");
                        requestPage.SubmitButtonClick();

                        var repayProcessingPage = new RepayProcessingPage(this.Client);

                        var paymentTakenPage = repayProcessingPage.WaitFor<RepayDuePartpaySuccessPage>() as RepayDuePartpaySuccessPage;
                        Assert.IsFalse(paymentTakenPage.IsRepayDuePartpaySuccessPageAmountTokenNotPresent());

                        // Post payment values
                        ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = _acceptanceTestResults.MigratedUser.AccountId });
                        var newDueDateAmount = summaryResponse.Values["CurrentLoanRepaymentAmountOnDueDate"].Single();
                        string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

                        // Get the content from the Payment Taken Page
                        string paymentTakenText = paymentTakenPage.ContentArea();
                        var testTitle = "Success! Your partial payment has gone through";
                        var testMessage = "Please repay this remaining balance without delay to avoid incurring further costs, which is the last thing we want to happen";

                        Assert.IsTrue(paymentTakenPage.Headers.Contains(testTitle), "Message title is incorrect.");
                        Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
                        Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Total Repayable is incorrect.");
                    }
                    catch (Exception ex)
                    {
                        _acceptanceTestResults.TestParameters.Exception = ex.Message;
                    }

                    finally
                    {
                        _acceptanceTestResults.TestEndDate = DateTime.Now;
                        _migHelper.StoreTestResults(_acceptanceTestResults);
                    }
                }
            }
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-231"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanRepayOverdueFull()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();
            //Thank you for resolving  this situation. We can’t promise your Wonga trust rating won’t have been affected, but we may consider helping you again in the future.
            const string testTitle = "Success! Your balance has been settled in full";
            const string testMessage = "Thank you for resolving this situation. We can’t promise your Wonga trust rating won’t have been affected, but we may consider helping you again in the future.";

            Assert.IsTrue(paymentTakenPage.Headers.Contains(testTitle), "Header is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
        }

        [Test, AUT(AUT.Uk), JIRA("UKMIG-231"), Owner(Owner.MuhammadQureshi)]
        public void TestMigratedUserCanRepayOverduePart()
        {
            //build L0 loan
            string email = Get.RandomEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            var application = ApplicationBuilder.New(customer).WithLoanAmount(150).WithLoanTerm(7).Build();
            var daysShift = 15;

            //time-shift loan so it's in arrears
            TimeSpan daysShiftSpan = TimeSpan.FromDays(daysShift);
            ApplicationOperations.RewindApplicationDates(application, daysShiftSpan);

            var requestId1 = Guid.NewGuid();
            var requestId2 = Guid.NewGuid();
            // Send command to create scheduled payment request
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId1, });
            Drive.Msmq.Payments.Send(new CreateScheduledPaymentRequest() { ApplicationId = application.Id, RepaymentRequestId = requestId2, });

            Client.Login().LoginAs(email).RepayButtonClick();
            var requestPage = new RepayRequestPage(this.Client);

            //Set partial payment amount, test for correct values at same time
            requestPage.IsRepayRequestPageSliderReturningCorrectOverDueValuesOnChange(application.Id.ToString(), "100");

            //Branch point - Add Cv2 for each path and proceed
            requestPage.setSecurityCode("123");
            requestPage.SubmitButtonClick();

            var repayProcessingPage = new RepayProcessingPage(this.Client);

            var paymentTakenPage = repayProcessingPage.WaitFor<RepayOverduePartpaySuccessPage>() as RepayOverduePartpaySuccessPage;
            Assert.IsFalse(paymentTakenPage.IsRepayOverduePartpaySuccessPageAmountTokenNotPresent());

            // Post payment values
            ApiResponse summaryResponse = Drive.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = customer.Id });
            var newDueDateAmount = summaryResponse.Values["CurrentLoanAmountDueToday"].Single();
            string sNewDueDateAmount = String.Format("{0:0.00}", newDueDateAmount);

            // Get the content from the Payment Taken Page
            string paymentTakenText = paymentTakenPage.ContentArea();

            var testTitle = "Success! Your partial payment has gone through";
            var testMessage = "Thanks for making a part-payment on your overdue balance. It’s a step in the right direction.";

            Assert.IsTrue(paymentTakenPage.Headers[0].Contains(testTitle), "Header is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(testMessage), "Content area text is incorrect.");
            Assert.IsTrue(paymentTakenText.Contains(sNewDueDateAmount), "New Total Repayable is wrong.");
        }

        #region test user creation in local build
        [Test]
        public void CreateUser()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();//"testMigratedUser@gmail.com";

            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();

            Application application = ApplicationBuilder
                .New(customer)
                .Build();

            loginPage.LoginAs(email);

        }

        [Test]
        public void CreateUserFromWeb()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }
        #endregion
    }
}

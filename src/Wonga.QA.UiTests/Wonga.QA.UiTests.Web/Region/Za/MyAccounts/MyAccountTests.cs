using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI;

namespace Wonga.QA.UiTests.Web.Region.Za.MyAccounts
{
    [Parallelizable(TestScope.All), AUT(AUT.Za)]
    class ZaMyAccountTests : UiTest
    {
        [Test, JIRA("QA-203"), Category(TestCategories.SmokeTest)]
        public void L0JourneyInvalidAccountNumberShouldCauseWarningMessageOnNextPage()
        {
            var journey1 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAccountNumber("7434567")
                .FillAndStop();
            var bankDetailsPage1 = journey1.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage1.Next(); });

            var journey2 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask))
                .WithAccountNumber("7534567")
                .FillAndStop();
            var bankDetailsPage2 = journey2.Teleport<PersonalBankAccountPage>() as PersonalBankAccountPage;
            Assert.Throws<AssertionFailureException>(() => { var processingPage = bankDetailsPage2.Next(); });
        }

        [Test, JIRA("QA-202"), Category(TestCategories.SmokeTest)]
        public void LNJourneyInvalidAccountNumberShouldCauseWarningMessageOnNextPage()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();  // to take LN status

            var page = loginPage.LoginAs(email);
            var payment1 = Client.Payments();

            if (payment1.IsAddBankAccountButtonExists())
            {
                payment1.AddBankAccountButtonClick();


                payment1.AddBankAccount("Capitec", "Current", "7434567", "2 to 3 years");
                Do.With.Timeout(2).Until(payment1.IfHasAnExeption);
                Assert.IsTrue(payment1.IfHasAnExeption());
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
            var home = Client.Home();
            var payment2 = Client.Payments();

            if (payment2.IsAddBankAccountButtonExists())
            {
                payment2.AddBankAccountButtonClick();

                Thread.Sleep(2000); // Wait some time to load popup

                payment1.AddBankAccount("Capitec", "Current", "7534567", "2 to 3 years");
                Do.With.Timeout(2).Until(payment2.IfHasAnExeption);
                Assert.IsTrue(payment2.IfHasAnExeption());
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }

        [Test, JIRA("QA-201"), Category(TestCategories.SmokeTest), Pending("ZA-2777")]
        public void WhenLoggedCustomerWithoutLiveLoanAddsNewBankAccountItShouldBecomePrimary()
        {
            string accountNumber = "1234567";
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();

            var page = loginPage.LoginAs(email);
            var payment = Client.Payments();

            if (payment.IsAddBankAccountButtonExists())
            {
                payment.AddBankAccountButtonClick();
                payment.AddBankAccount("Capitec", "Current", accountNumber, "2 to 3 years");
                payment.WaitBankAccountPopupClose();
                payment.ClickCloseButton();

                payment = Client.Payments();
                int whileCount = 0;
                while (accountNumber.Remove(0, 3) != payment.DefaultAccountNumber && whileCount < 50)
                {
                    whileCount++;
                    payment = Client.Payments();
                }
                Console.WriteLine(whileCount);
                Assert.AreEqual(accountNumber.Remove(0, 3), payment.DefaultAccountNumber);
            }
            else
            {
                throw new NullReferenceException("Add bank account button not found");
            }
        }

        [Test, JIRA("QA-214"), Category(TestCategories.SmokeTest)]
        public void CustomerOnMyPersonalDetailsShouldBeAbleToChangeCommunicationPrefs()
        {


            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);

            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            myPersonalDetailsPage.CommunicationClick();

            switch (myPersonalDetailsPage.GetCommunicationText)
            {


                case ("You are not happy to receive updates and other communications from Wonga via email and SMS."):
                    {
                        myPersonalDetailsPage.SetCommunicationPrefs =
                            "I am happy to receive updates and other communications from Wonga via email and SMS.";

                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);
                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);

                        var happy = Drive.Data.Comms.Db.ContactPreferences.FindAllBy(AccountId: customer.Id).FirstOrDefault().AcceptMarketingContact;
                        Assert.IsTrue(happy);
                        Assert.AreEqual(
                            "You are happy to receive updates and other communications from Wonga via email and SMS.",
                            myPersonalDetailsPage.GetCommunicationText);
                        break;
                    }
                case ("You are happy to receive updates and other communications from Wonga via email and SMS."):
                    {
                        myPersonalDetailsPage.SetCommunicationPrefs =
                            "I am not happy to receive updates and other communications from Wonga via email and SMS.";

                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);
                        myPersonalDetailsPage.Submit();
                        Thread.Sleep(10000);

                        var happy = Drive.Data.Comms.Db.ContactPreferences.FindAllBy(AccountId: customer.Id).FirstOrDefault().AcceptMarketingContact;
                        Assert.IsFalse(happy);
                        Assert.AreEqual(
                            "You are not happy to receive updates and other communications from Wonga via email and SMS.",
                            myPersonalDetailsPage.GetCommunicationText);
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }

        }

        [Test, JIRA("QA-216")]
        public void CustomerShouldBeAbleToChangePassword()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PasswordClick();
            myPersonalDetailsPage.ChangePassword("Passw0rd", "Passw0rd", "Passw0rd");
            myPersonalDetailsPage.Submit();

            Assert.IsTrue(myPersonalDetailsPage.IsPasswordPopupHasErrorMessage());
            Thread.Sleep(1000);
            myPersonalDetailsPage.ChangePassword("Passw0rd", "Pass", "Pass");
            Assert.IsTrue(myPersonalDetailsPage.IsPasswordWarningMessageOccurs());
            Thread.Sleep(1000);
            myPersonalDetailsPage.PassPopupLostFocus();
            myPersonalDetailsPage.ChangePassword("Passw0rd", "QWEasd12", "QWEasd12");
            myPersonalDetailsPage.Submit();
            Thread.Sleep(10000);

            var homePage = myPersonalDetailsPage.Login.Logout();
            var mySummary = homePage.Login.LoginAs(email, "QWEasd12");
        }

        [Test, JIRA("QA-209")]
        public void CustomerShouldBeAbleToChangePhoneNumber()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();

            Do.With.Message("Problem with changing phone number").Until(() => myPersonalDetailsPage.ChangePhone("0123000000", "0212571908", "0000"));

            myPersonalDetailsPage.Submit();

            Do.With.Message("There is no home phone in db").Until(() => Drive.Db.Comms.CustomerDetails.Single(c => c.Email == email).HomePhone != "0210000000");
            var homePhone = Drive.Db.Comms.CustomerDetails.FirstOrDefault(c => c.Email == email).HomePhone;

            Assert.AreEqual("0123000000", myPersonalDetailsPage.GetHomePhone);
            Assert.AreEqual("0123000000", homePhone);
            //TODO check SF
        }

        [Test, JIRA("QA-212"), Category(TestCategories.SmokeTest)]
        public void CustomerShouldBeAbleToChangeMobileNumber()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();

            Do.With.Message("Problem with changing phone number").Until(() => myPersonalDetailsPage.ChangePhone("0210000000", "0213456789", "0000"));

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            Do.With.Message("There is no phone in DB").Until(() => Drive.Db.Comms.CustomerDetails.Single(c => c.Email == email).MobilePhone == "0213456789");
            var mobilePhone = Drive.Db.Comms.CustomerDetails.FirstOrDefault(c => c.Email == email).MobilePhone;

            Assert.AreEqual("0213456789", myPersonalDetailsPage.GetMobilePhone);
            Assert.AreEqual("0213456789", mobilePhone);
            //TODO check SF
        }

        [Test, JIRA("QA-211"), Category(TestCategories.SmokeTest)]
        public void ChangingPhoneNumberWithWrongPinShouldCauseWarningMessage()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();

            Do.With.Message("Problem with changing phone number").Until(() => myPersonalDetailsPage.ChangePhone("0210000000", "0211234567", "1111"));
            myPersonalDetailsPage.Submit();
            Assert.IsTrue(myPersonalDetailsPage.GetPopupErrorMessage.Equals("The SMS PIN you entered was incorrect."));
        }

        [Test, JIRA("QA-208"), Category(TestCategories.SmokeTest)]
        public void LoanOlderThanThreeDaysThenViewLoanDetailsLinkShouldBeDisplayedAndCorrect()
        {

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.DaysFromStart(5);
            var mySummaryPage = loginPage.LoginAs(email);

            mySummaryPage.ClickViewLoanDetailsButton();
            mySummaryPage.WaitForMySummaryPopup();
            Assert.IsTrue(mySummaryPage.IsPopupContainsSummaryDetailsTable());
        }

        [Test, JIRA("QA-213"), Category(TestCategories.SmokeTest)]
        public void CustomerUpdatesPhoneNumbersAndDoesntMakeChangesShouldSeeMessageOnTopWindow()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetails = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();
            myPersonalDetails.PhoneClick();
            Assert.IsTrue(myPersonalDetails.DontChangePhone());
        }

        [Test, JIRA("QA-217"), Category(TestCategories.SmokeTest)]
        public void CustomerChangesAddressWithNotValidDataThenWarningMessageShouldOccur()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            var mySummaryPage = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummaryPage.Navigation.MyPersonalDetailsButtonClick();

            var oldTown = myPersonalDetailsPage.GetTown; //to check if changes in db occured

            myPersonalDetailsPage.AddressClick();
            Do.With.Message("Change MyAddress title didn't displayed").Until(myPersonalDetailsPage.ChangeMyAddressElement.IsChangeMyAddressTitleDisplayed);

            var newFlat = Get.RandomInt(100).ToString();
            var newStreet = Get.RandomString(3, 10);
            var newHouseNumber = Get.RandomString(3, 10);
            var newDistrict = Get.RandomString(3, 10);
            var newTown = Get.RandomString(3, 10);
            var newPostcode = Get.GetPostcode();

            myPersonalDetailsPage.ChangeMyAddressElement.Postcode = "123";
            myPersonalDetailsPage.ChangeMyAddressElement.Flat = newFlat;
            Assert.IsTrue(myPersonalDetailsPage.ChangeMyAddressElement.IsPostcodeWarningOccurred());

            myPersonalDetailsPage.ChangeMyAddressElement.Postcode = newPostcode;
            myPersonalDetailsPage.ChangeMyAddressElement.Street = newStreet;
            myPersonalDetailsPage.ChangeMyAddressElement.HouseNumber = newHouseNumber;
            myPersonalDetailsPage.ChangeMyAddressElement.District = newDistrict;
            myPersonalDetailsPage.ChangeMyAddressElement.Town = newTown;
            myPersonalDetailsPage.ChangeMyAddressElement.AddressPeriod = "2 to 3 years";
            Thread.Sleep(2000);

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            var addresses = Drive.Data.Comms.Db.Addresses;

            Do.With.Message("There is no sought-for addres in DB").Until(() => addresses.FindByAccountId(customer.Id).Town != oldTown);
            var currentAddress = addresses.FindByAccountId(customer.Id);
            //Check changes in DB
            Assert.AreEqual(currentAddress.Flat, newFlat);
            Assert.AreEqual(currentAddress.Street, newStreet);
            Assert.AreEqual(currentAddress.HouseNumber, newHouseNumber);
            Assert.AreEqual(currentAddress.District, newDistrict);
            Assert.AreEqual(currentAddress.Town, newTown);
            Assert.AreEqual(currentAddress.PostCode, newPostcode);

            //Check changes in UI
            Assert.AreEqual(myPersonalDetailsPage.GetHouseNumberAndStreet, newHouseNumber + " " + newStreet);
            Assert.AreEqual(myPersonalDetailsPage.GetTown, newTown);
            Assert.AreEqual(myPersonalDetailsPage.GetPostcode, newPostcode);


        }

        [Test, JIRA("QA-219"), Category(TestCategories.SmokeTest), Pending("ZA-2777")]
        public void CustomerShouldBeAbleToAddBankAccount()
        {
            var accountPreferences = Drive.Data.Payments.Db.AccountPreferences;
            var bankAccountsBase = Drive.Data.Payments.Db.BankAccountsBase;
            string accountNumber = "1234567";

            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();

            loginPage.LoginAs(email);
            var firstMyPaymentsPage = Client.Payments();

            Do.With.Message("Add Bank Account button is not exists").Until(firstMyPaymentsPage.IsAddBankAccountButtonExists);
            firstMyPaymentsPage.AddBankAccountButtonClick();
            firstMyPaymentsPage.AddBankAccount("Capitec", "Current", accountNumber, "2 to 3 years");
            firstMyPaymentsPage.WaitBankAccountPopupClose();
            firstMyPaymentsPage.ClickCloseButton();
            var newMyPaymentsPage = Client.Payments();

            Assert.AreEqual(accountNumber.Remove(0, 3), newMyPaymentsPage.DefaultAccountNumber);

            var primaryBankAccountId = accountPreferences.FindByAccountId(customer.Id).PrimaryBankAccountId;
            var bankAccount = bankAccountsBase.FindByBankAccountId(primaryBankAccountId);

            Assert.AreEqual("Capitec", bankAccount.BankName);
            Assert.AreEqual("Current", bankAccount.AccountType);
            Assert.AreEqual(accountNumber, bankAccount.AccountNumber);

            var journey = JourneyFactory.GetLnJourney(Client.Home());

            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;

            Assert.AreEqual(accountNumber.Remove(0, 3), applyPage.GetCurrentBankAccount);

        }

        [Test, Category(TestCategories.SmokeTest), JIRA("QA-279")]
        public void LNCustomerChangesMobilePhoneNumberToTheSameOneButUsingSeparators()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string phone = "0211000000";
            List<string> invalidPhones = new List<string> { "021-1000000", "021.1000000", "021,1000000", "021/1000000" };
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).WithMobileNumber(phone).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummary = loginPage.LoginAs(email);
            var myPersonals = mySummary.Navigation.MyPersonalDetailsButtonClick();
            foreach (var invaliPhone in invalidPhones)
            {
                myPersonals.PhoneClick();
                myPersonals.ChangeMobilePhone(invaliPhone, "0000");
            }
        }

        [Test, JIRA("QA-210"), Category(TestCategories.SmokeTest)]
        public void CustomerChangeTelephonFieldsCheckHomePhone()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer)
                .Build();
            application.RepayOnDueDate();

            var mySummary = loginPage.LoginAs(email);
            var myPersonalDetailsPage = mySummary.Navigation.MyPersonalDetailsButtonClick();

            myPersonalDetailsPage.PhoneClick();
            myPersonalDetailsPage.ChangeHomePhone("");

            myPersonalDetailsPage.Submit();
            myPersonalDetailsPage.WaitForSuccessPopup();
            myPersonalDetailsPage.Submit();

            var homePhoneUI = myPersonalDetailsPage.GetHomePhone;
            Console.WriteLine(customer.Id.ToString());
            var homePhoneDB = Do.With.Message("There is no sought-for phone number in DB").Until(() => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(customer.Id).HomePhone);

            Assert.AreEqual("", homePhoneUI);
            Assert.AreEqual("", homePhoneDB);
        }

        [Test, JIRA("QA-304")]
        public void CustomerClicksOnRepayButtonBeforeDueDate()
        {
            var _applications = Drive.Data.Payments.Db.Applications;
            var _scheduledPayments = Drive.Data.Payments.Db.ScheduledPayments;

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();

            var appId = _applications.FindByExternalId(application.Id).ApplicationId;
            var naedo = _scheduledPayments.FindByApplicationId(appId);
            Assert.IsNull(naedo);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(customer.Email);

            var repaymentOptionsPage = myAccountPage.RepayClick();
        }

        [Test, JIRA("QA-306")]
        public void ThereIsNoRepayButtonWhenNAEDOTrackingInPlace()
        {
            var _applications = Drive.Data.Payments.Db.Applications;
            var _scheduledPayments = Drive.Data.Payments.Db.ScheduledPayments;

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            application.PutIntoArrears(2);

            var appId = _applications.FindByExternalId(application.Id).ApplicationId;
            var naedo = _scheduledPayments.FindByApplicationId(appId);
            Assert.IsNotNull(naedo);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(customer.Email);

            var tagCloud = myAccountPage.GetTagCloud;
            Assert.AreEqual("", tagCloud);
        }

        [Test, JIRA("QA-307")]
        public void CustomerSetsUpDebitOrder()
        {
            var _applications = Drive.Data.Payments.Db.Applications;
            var _scheduledPayments = Drive.Data.Payments.Db.ScheduledPayments;

            var customer = CustomerBuilder.New().Build();
            var application = ApplicationBuilder.New(customer).Build();
            
            var appId = _applications.FindByExternalId(application.Id).ApplicationId;
            var naedo = _scheduledPayments.FindByApplicationId(appId);
            Assert.IsNull(naedo);

            var loginPage = Client.Login();
            var myAccountPage = loginPage.LoginAs(customer.Email);

            var repaymentOptionsPage = myAccountPage.RepayClick();
            var debitOrderPage = repaymentOptionsPage.DebitOrderButtonClick();
            var debitOrderSuccessPage = debitOrderPage.Submit();
            var updatedMyAccountPage = debitOrderSuccessPage.BackToYourAccountButtonClick();
            string successText = ContentMap.Get.MySummaryPage.DebitOrderSuccesText.Insert(13, "R" + application.LoanAmount.ToString() + ".00");
            Assert.IsTrue(updatedMyAccountPage.GetStatusText.Contains(successText));
        }
    }
}

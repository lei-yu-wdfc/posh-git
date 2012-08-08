﻿using System;
using System.Linq;
using System.Threading;
using Gallio.Framework.Assertions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web
{
    [TestFixture, Parallelizable(TestScope.All)]
    internal class LnJourneyTests : UiTest
    {
        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("QA-199")]
        public void LoggedCustomerWithoutLoanAppliesNewLoanChangesMobilePhoneAndClicksResendPinItShouldBeResent()
        {
            string email = Get.RandomEmail();
            string name = Get.RandomString(3, 10);
            string surname = Get.RandomString(3, 10);
            string oldphone = Get.GetMobilePhone() /*"077009" + Get.RandomLong(1000, 9999).ToString()*/;
            string phone = Get.GetMobilePhone() /*"077009" + Get.RandomLong(1000, 9999).ToString()*/;
            Customer customer;
            Application application;
            LoginPage loginPage;
            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Za:
                default:
                    customer = CustomerBuilder
                        .New()
                        .WithForename(name)
                        .WithSurname(surname)
                        .WithEmailAddress(email)
                        .WithMobileNumber(oldphone)
                        .Build();
                    application = ApplicationBuilder
                       .New(customer)
                       .Build();
                    application.RepayOnDueDate();
                    loginPage = Client.Login();
                    loginPage.LoginAs(email);
                    break;
                case AUT.Uk:
                    customer = CustomerBuilder
                        .New()
                        .WithForename(name)
                        .WithSurname(surname)
                        .WithEmailAddress(email)
                        .Build();
                    application = ApplicationBuilder
                       .New(customer)
                       .Build();
                    application.RepayOnDueDate();
                    loginPage = Client.Login();
                    loginPage.LoginAs(email);
                    break;
            }
            switch (Config.AUT)
            {
                #region Za
                case AUT.Za:
                    var journeyZa = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(200).WithDuration(20);
                    var pageZA = journeyZa.Teleport<ApplyPage>() as ApplyPage;
                    pageZA.SetNewMobilePhone = phone;
                    pageZA.ResendPinClick();

                    var phoneInDbFormat = phone.Replace("077", "2777");

                    var smsZa = Do.With.Message("There is no sought-for sms in DB").Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(phoneInDbFormat));
                    Do.With.Message("There is one sms in DB instead of two").Until(() => smsZa.Count() == 2);
                    Assert.AreEqual(2, smsZa.Count());
                    Console.WriteLine(smsZa.Count());
                    foreach (var sms in smsZa)
                    {
                        Console.WriteLine(sms.MessageText + " / " + sms.CreatedOn);
                        Assert.IsTrue(
                            sms.MessageText.Contains("You will need it to complete your application back at Wonga.com."));
                    }

                    Console.WriteLine(smsZa.Count());
                    break;
                #endregion
                #region Ca
                case AUT.Ca:
                    var journeyCa = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(200).WithDuration(25);
                    var pageCa = journeyCa.Teleport<ApplyPage>() as ApplyPage;
                    pageCa.SetNewMobilePhone = phone;
                    pageCa.ResendPinClick();
                    var smsCa = Do.With.Message("There is no sought-for sms in DB").Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(phone.Replace("077", "177")));
                    foreach (var sms in smsCa)
                    {
                        Console.WriteLine(sms.MessageText + " / " + sms.CreatedOn);
                        Assert.IsTrue(sms.MessageText.Contains("You will need it to complete your application back at Wonga.ca."));
                    }
                    Assert.AreEqual(1, smsCa.Count());
                    break;
                #endregion
                #region Uk
                case AUT.Uk:
                    var journeyUk = JourneyFactory.GetLnJourney(Client.Home()).WithAmount(200).WithDuration(20).WithNewMobilePhone(oldphone);
                    var pageUk = journeyUk.Teleport<ApplyPage>() as ApplyPage;
                    pageUk.SetNewMobilePhone = phone;
                    pageUk.ResendPinClick();

                    var phoneInDbFormatUk = phone.Replace("077", "447");

                    //------------- commented while frontend on Uk won't be repaired ---------------
                    //var smsUk = Do.With.Message("There is no sought-for sms in DB").Until(() => Drive.Data.Sms.Db.SmsMessages.FindAllByMobilePhoneNumber(phoneInDbFormatUk));
                    //Do.With.Message("There is one sms in DB instead of two").Until(() => smsUk.Count() == 2);
                    //Assert.AreEqual(2, smsUk.Count());
                    //Console.WriteLine(smsUk.Count());
                    //foreach (var sms in smsUk)
                    //{
                    //    Console.WriteLine(sms.MessageText + " / " + sms.CreatedOn);
                    //    Assert.IsTrue(
                    //        sms.MessageText.Contains("You will need it to complete your application back at Wonga.com."));
                    //}

                    //Console.WriteLine(smsUk.Count());
                    break;
                #endregion
            }
        }

        [Test, AUT(AUT.Ca, AUT.Za, AUT.Uk), JIRA("QA-302, QA-335"), Pending("Uses sleep()!")]
        public void LoggedCustomerWithoutLoanAppliesNewLoanChangesMobilePhoneAndClicksResendPinAndGoFarther()
        {
            string email = Get.RandomEmail();
            string name = Get.RandomString(3, 10);
            string surname = Get.RandomString(3, 10);
            string oldphone = Get.GetMobilePhone() /*"077009" + Get.RandomLong(1000, 9999).ToString()*/;
            Customer customer;
            Application application;
            LoginPage loginPage;
            BaseLnJourney journey;

            switch (Config.AUT)
            {
                case AUT.Ca:
                case AUT.Za:
                default:
                    customer = CustomerBuilder
                        .New()
                        .WithForename(name)
                        .WithSurname(surname)
                        .WithEmailAddress(email)
                        .WithMobileNumber(oldphone)
                        .Build();
                    application = ApplicationBuilder
                       .New(customer)
                       .Build();
                    application.RepayOnDueDate();
                    loginPage = Client.Login();
                    loginPage.LoginAs(email);
                    journey = JourneyFactory.GetLnJourney(Client.Home());

                    break;
                case AUT.Uk:
                    customer = CustomerBuilder
                        .New()
                        .WithForename(name)
                        .WithSurname(surname)
                        .WithEmailAddress(email)
                        .Build();
                    application = ApplicationBuilder
                       .New(customer)
                       .Build();
                    application.RepayOnDueDate();
                    loginPage = Client.Login();
                    loginPage.LoginAs(email);
                    journey = JourneyFactory.GetLnJourney(Client.Home()).WithNewMobilePhone(oldphone);
                    break;
            }

            string phone = Get.GetMobilePhone();
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;
            applyPage.SetNewMobilePhone = phone;
            applyPage.ResendPinClick();
            Thread.Sleep(2000);
            applyPage.CloseResendPinPopup();
            applyPage.ApplicationSection.SetPin = "0000";
            journey.CurrentPage = applyPage.Submit();
            var mySummary = journey.Teleport<AcceptedPage>() as AcceptedPage;


        }

        [Test, AUT(AUT.Za, AUT.Ca, AUT.Uk), Category(TestCategories.SmokeTest), Pending("Fail")]
        public void LnVerifyUrlsAreCorrect()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            string name = Get.GetName();
            string surname = Get.RandomString(10);
            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .WithForename(name)
                .WithSurname(surname)
                .Build();
            Application application = ApplicationBuilder
                .New(customer)
                .Build();
            application.RepayOnDueDate();
            var mySummaryPageAfterLogin = loginPage.LoginAs(email);
            var homePage = Client.Home();
            var journey = JourneyFactory.GetLnJourney(homePage);
            var applyPage = journey.Teleport<ApplyPage>() as ApplyPage;
            //var journey = JourneyFactory.GetLnJourney(homePage).WithFirstName(name).WithLastName(surname);

            // Check the URL here is /apply-member
            Assert.Contains(Client.Driver.Url, "/apply-member?",
                            "The apply page URL does not contain '/apply-member?'");
            journey.CurrentPage = applyPage.Submit() as ProcessingPage;
            // Check the URL here is /processing-member
            Assert.EndsWith(Client.Driver.Url, "/processing-member",
                            "The processing page URL is not /processing-member.");
            var acceptedPageZaCa = journey.Teleport<AcceptedPage>() as AcceptedPage;
            // Check the URL here is /apply-accept-member
            Assert.EndsWith(Client.Driver.Url, "/apply-accept-member",
                            "The accept page URL is not /apply-accept-member.");
            var dealDonePageZaCa = journey.Teleport<DealDonePage>() as DealDonePage;
            // Check the URL here is /deal-done-member
            Assert.EndsWith(Client.Driver.Url, "/deal-done-member",
                            "The deal done page URL is not /deal-done-member.");


        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.UiTests.Web.Prepaid
{
    class PrepaidAccountTests : UiTest
    {
        private Customer _eligibleCustomer = null;

        private static readonly String PROMOTION_CARD_TEXT = "Promotional text";
        private static readonly String FOOTER_CARD_TEXT = "Footext text";
        public static readonly String CUSTOMER_FULL_NAME = "FULL_NAME";
        public static readonly String CUSTOMER_FULL_ADDRESS = "FULL_ADDRESS"; 

 
        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id, true);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-1"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void DisplayPrepaidCardSubnavForEligibleCustomer()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            summaryPage.ShowPrepaidCardButton();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-3"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void DisplayLastRegisteredDetailsForEligibleCustomer()
        {
            Customer cutomerWithNocards = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(cutomerWithNocards.Id,true);
            CustomerOperations.MakeZeroCardsForCustomer(cutomerWithNocards.Id);

            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(cutomerWithNocards.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage.ApplyPrepaidCardButtonClick();

            var dictionary = GetFullCustomerInfo(cutomerWithNocards.Id);
            var pageSource = prepaidPage.Client.Source();
               
            Assert.IsTrue(pageSource.Contains(dictionary[CUSTOMER_FULL_NAME]));
            Assert.IsTrue(pageSource.Contains(dictionary[CUSTOMER_FULL_ADDRESS]));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-2"), Pending("Fails")]
        public void DisplayPrepaidCardBannerForEligibleCustomer()
        {
            var loginPage = Client.Login();

            Application application = ApplicationBuilder.New(_eligibleCustomer).Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(_eligibleCustomer.GetEmail());

            var journey = JourneyFactory.GetLnJourney(Client.Home()).WithNewMobilePhone();
            var page = journey.Teleport<DealDonePage>() as DealDonePage;

            Assert.IsTrue(Client.Driver.PageSource.Contains(PROMOTION_CARD_TEXT));
            Assert.IsTrue(Client.Driver.PageSource.Contains(FOOTER_CARD_TEXT));

        }

        [Test, AUT(AUT.Uk), JIRA("PP-2"), Pending("Fails")]
        public void NoBannerShouldBeDisplayForNonEligibleCustomer()
        {
            var loginPage = Client.Login();
            
            CustomerOperations.ChangeMarketingEligibility(_eligibleCustomer.Id,true);
            Application application = ApplicationBuilder.New(_eligibleCustomer).Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(_eligibleCustomer.GetEmail());

            var journey = JourneyFactory.GetLnJourney(Client.Home()).WithNewMobilePhone();
            var page = journey.Teleport<DealDonePage>() as DealDonePage;

            Assert.IsFalse(Client.Driver.PageSource.Contains(PROMOTION_CARD_TEXT));
            Assert.IsFalse(Client.Driver.PageSource.Contains(FOOTER_CARD_TEXT));
        }

        [Test, AUT(AUT.Uk), JIRA("PP-16"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void CustomerWithPremiumCardShouldSeeMenuNav()
        {
            CustomerOperations.UpdateCustomerPrepaidCard(_eligibleCustomer.Id,true);
            
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage.ShowMenuElementsForPremiumCard();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-16"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void CustomerWithStandardCardShouldSeeMenuNav()
        {
            CustomerOperations.UpdateCustomerPrepaidCard(_eligibleCustomer.Id,false);
            
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage.ShowMenuElementsForStandardCard();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-148"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void LinksPresentOnStandardCardLandingPageForCustomerWithoutCards()
        {
            Customer cutomerWithNocards = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(cutomerWithNocards.Id, true);
            CustomerOperations.MakeZeroCardsForCustomer(cutomerWithNocards.Id);

            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(cutomerWithNocards.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            
            prepaidPage.FindFAQAndTSLinks();
            prepaidPage.FindTSInFeesLink();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-148"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void LinksPresentOnStandardCardLandingPageForCustomerWithStandartCard()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();

            prepaidPage.FindFAQAndTSLinks();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-148"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void LinksPresentOnStandardCardLandingPageForCustomerWithPremiumCard()
        {
            CustomerOperations.UpdateCustomerPrepaidCard(_eligibleCustomer.Id, true);

            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();

            prepaidPage.FindFAQAndTSLinks();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-148"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void LinksPresentOnPremiumCardLandingPageForCustomerWithoutCards()
        {
            Customer cutomerWithNocards = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(cutomerWithNocards.Id, true);
            CustomerOperations.MakeZeroCardsForCustomer(cutomerWithNocards.Id);

            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(cutomerWithNocards.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage = prepaidPage.PremiumCardButtonClick();

            prepaidPage.FindFAQAndTSLinks();
            prepaidPage.FindPremiumRewardsLink();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-78"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void LostOrStolenOrForgottenPinTest()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage = prepaidPage.LostOrStolenOrForgottenButtonClick();
            prepaidPage = prepaidPage.GetResetCodeButtonClick();

            var resetCodeTextField = prepaidPage.GetResetCodeTextField();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-18"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void HighlightedOffersBlockPresentOnSummaryPage()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage = prepaidPage.SummaryMenuChoose();
            prepaidPage.FindHighlightedOffersBlock();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-101"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void HighlightedOffersBlockPresentOnRewardsPage()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage = prepaidPage.RewardsAndOffersMenuChoose();
            prepaidPage.FindHighlightedOffersBlock();
        }

		//[Test, AUT(AUT.Uk), JIRA("PP-203"), Pending("Fails as cannot open database PrepaidCard")]
		//public void ShowAvailableCustomerBalanceOnSummaryPageTest()
		//{
		//    var customer = CustomerBuilder.New().WithEmailAddress(Get.GetEmail(50)).Build();
		//    CustomerOperations.CreateMarketingEligibility(customer.Id, true);
		//    CustomerOperations.CreatePrepaidCardForCustomer(customer.Id, false);

		//    var loginPage = Client.Login();
		//    var summaryPage = loginPage.LoginAs(customer.Email);
		//    var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
		//    String availableBalance = prepaidPage.GetAvailableBalanceValue();

		//    var query = new GetPrepaidAvailableAccountBalanceQuery();
		//    query.CustomerExternalId = customer.Id;
		//    var response = Drive.Api.Queries.Post(query);
		//    String expectedAvailableBalance = String.Format("Current balance : £{0}", response.Values["Balance"].Single());
		//    Assert.AreEqual(availableBalance, expectedAvailableBalance);
		//}
		
        [Test,AUT(AUT.Uk),JIRA("PP-33"), Pending("Fails when can't find css selector on MyPrepaidButtonClick")]
        public void CustomerShouldSeeLoadChoicesWithLoan()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidCardPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidCardPage.ShowMenuElementsForStandardCard();
            prepaidCardPage.TopUpClick();
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomer.Id);
        }

        
        private static  String GetFullCustomerName(Guid customerId)
        {
            StringBuilder builder = new StringBuilder();

            var request = new GetCustomerDetailsQuery();
            request.AccountId = customerId;

            var response = Drive.Api.Queries.Post(request);
            builder.Append(response.Values["Forename"].First());
            builder.Append(" ");
            builder.Append(response.Values["MiddleName"].First());
            builder.Append(" ");
            builder.Append(response.Values["Surname"].First());

            return builder.ToString();
        }

        private static String GetFullCustomerAddress(Guid customerId)
        {
            StringBuilder builder = new StringBuilder();

            var request = new GetCurrentAddressQuery();
            request.AccountId = customerId;

            var response = Drive.Api.Queries.Post(request);
            builder.Append(response.Values["HouseName"].First());
            builder.Append(response.Values["HouseNumber"].First());
            builder.Append(" ");
            builder.Append(response.Values["Street"].First());
            builder.Append("<br />");
            builder.Append(response.Values["Town"].First());
            builder.Append(" ");
            builder.Append(response.Values["Postcode"].First());

            return builder.ToString();
        }

        public static Dictionary<String, String> GetFullCustomerInfo(Guid customerId)
        {
            Dictionary<String, String> result = new Dictionary<string, string>();
            result.Add(CUSTOMER_FULL_NAME, GetFullCustomerName(customerId));
            result.Add(CUSTOMER_FULL_ADDRESS, GetFullCustomerAddress(customerId));
            return result;
        }


    }
}

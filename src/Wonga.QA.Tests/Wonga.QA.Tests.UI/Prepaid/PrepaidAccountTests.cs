using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Prepaid
{
    class PrepaidAccountTests : UiTest
    {
        private Customer _eligibleCustomer = null;

        private static readonly String PROMOTION_CARD_TEXT = "Promotional text";
        private static readonly String FOOTER_CARD_TEXT = "Footext text";

 
        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id, true);
        }

        [Test, AUT(AUT.Uk), JIRA("PP-1")]
        public void DisplayPrepaidCardSubnavForEligibleCustomer()
        {
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            summaryPage.ShowPrepaidCardButton();
        }

        [Test, AUT(AUT.Uk), JIRA("PP-3")]
        public void DisplayLastRegisteredDetailsForEligibleCustomer()
        {
            Customer cutomerWithNocards = CustomerBuilder.New().Build();
            CustomerOperations.CreateMarketingEligibility(cutomerWithNocards.Id,true);
            CustomerOperations.MakeZeroCardsForCustomer(cutomerWithNocards.Id);

            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(cutomerWithNocards.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
            prepaidPage.ApplyPrepaidCardButtonClick();

            var dictionary = CustomerOperations.GetFullCustomerInfo(cutomerWithNocards.Id);
            var pageSource = prepaidPage.Client.Source();
               
            Assert.IsTrue(pageSource.Contains(dictionary[CustomerOperations.CUSTOMER_FULL_NAME]));
            Assert.IsTrue(pageSource.Contains(dictionary[CustomerOperations.CUSTOMER_FULL_ADDRESS]));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-2")]
        public void DisplayPrepaidCardBannerForEligibleCustomer()
        {
            var loginPage = Client.Login();

            Application application = ApplicationBuilder.New(_eligibleCustomer).Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(_eligibleCustomer.GetEmail());

            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = ((UkLnJourney)journey.ApplyForLoan(200, 10))
                            .FillApplicationDetailsWithNewMobilePhone()
                            .WaitForAcceptedPage()
                            .FillAcceptedPage()
                           .CurrentPage as DealDonePage;

            Assert.IsTrue(Client.Driver.PageSource.Contains(PROMOTION_CARD_TEXT));
            Assert.IsTrue(Client.Driver.PageSource.Contains(FOOTER_CARD_TEXT));

        }

        [Test, AUT(AUT.Uk), JIRA("PP-2")]
        public void NoBannerShouldBeDisplayForNonEligibleCustomer()
        {
            var loginPage = Client.Login();
            
            CustomerOperations.ChangeMarketingEligibility(_eligibleCustomer.Id,true);
            Application application = ApplicationBuilder.New(_eligibleCustomer).Build();
            application.RepayOnDueDate();
            loginPage.LoginAs(_eligibleCustomer.GetEmail());

            var journey = JourneyFactory.GetLnJourney(Client.Home());
            var page = ((UkLnJourney)journey.ApplyForLoan(200, 10))
                            .FillApplicationDetailsWithNewMobilePhone()
                            .WaitForAcceptedPage()
                            .FillAcceptedPage()
                           .CurrentPage as DealDonePage;

            Assert.IsFalse(Client.Driver.PageSource.Contains(PROMOTION_CARD_TEXT));
            Assert.IsFalse(Client.Driver.PageSource.Contains(FOOTER_CARD_TEXT));
        }

        [Test,AUT(AUT.Uk),JIRA("PP-16")]
        public void CustomerWithPremiumCardShouldSeeMenuNav()
        {
            CustomerOperations.UpdateCustomerPrepaidCard(_eligibleCustomer.Id,true);
            
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();

        }

        [Test,AUT(AUT.Uk),JIRA("PP-16")]
        public void CustomerWithStandardCardShouldSeeMenuNav()
        {
            CustomerOperations.UpdateCustomerPrepaidCard(_eligibleCustomer.Id,false);
            
            var loginPage = Client.Login();
            var summaryPage = loginPage.LoginAs(_eligibleCustomer.GetEmail());
            var prepaidPage = summaryPage.Navigation.MyPrepaidCardButtonClick();
        }


        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomer.Id);
        }
    }
}

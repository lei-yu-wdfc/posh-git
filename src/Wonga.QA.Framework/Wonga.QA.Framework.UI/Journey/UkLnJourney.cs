using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class UkLnJourney : ILnConsumerJourney
    {
         public string FirstName { get; set; }
        public string LastName { get; set; }
        public BasePage CurrentPage { get; set; }

        public UkLnJourney(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
        }

        public ILnConsumerJourney SetName(string forename, string surname)
        {
            FirstName = forename;
            LastName = surname;
            return this;
        }

        public ILnConsumerJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.ApplyLn() as ApplyPage;
            return this;
        }

        public ILnConsumerJourney FillApplicationDetails()
        {
            var applyPage = CurrentPage as ApplyPage;
            applyPage.SetNewMobilePhone = Get.GetMobilePhone();
            applyPage.ApplicationSection.SetPin = "0000";
            applyPage.ApplicationSection.SetSecurityCode = "000";
            applyPage.ApplicationSection.SetMinCash = "100";
            CurrentPage = applyPage.Submit() as ProcessingPage;
            return this;
        }

        public ILnConsumerJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }

        public ILnConsumerJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        public ILnConsumerJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        public ILnConsumerJourney GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }
    }
}

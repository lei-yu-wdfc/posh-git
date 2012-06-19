using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Journey
{
    class ZaMobileLnJourney : ILnConsumerJourney
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BasePageMobile CurrentPage { get; set; }

        public ZaMobileLnJourney(BasePageMobile homePage)
        {
            CurrentPage = homePage as HomePageMobile;
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
            //var homePage = CurrentPage as HomePageMobile;
            //homePage.Sliders.HowMuch = amount.ToString();
            //homePage.Sliders.HowLong = duration.ToString();
            //CurrentPage = homePage.Sliders.ApplyLn() as ApplyPage;
            return this;
        }

        public ILnConsumerJourney FillApplicationDetails()
        {
        //    var applyPage = CurrentPage as ApplyPage;
        //    CurrentPage = applyPage.Submit() as ProcessingPage;
            return this;
        }

        public ILnConsumerJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPageMobile;
            CurrentPage = processingPage.WaitFor<AcceptedPageMobile>() as AcceptedPageMobile;
            return this;
        }

        public ILnConsumerJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPageMobile;
            CurrentPage = processingPage.WaitFor<DeclinedPageMobile>() as DeclinedPageMobile;
            return this;
        }

        public ILnConsumerJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPageMobile;
            acceptedPage.SignConfirmZA();
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        public ILnConsumerJourney GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPageMobile;
            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Journey;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;

namespace Wonga.QA.Framework.UI
{
    class ZaLnJourney : BaseLnJourney
    {
        public ZaLnJourney(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;

            _amount = 200;
            _duration = 10;

            journey.Add(typeof(HomePage), ApplyForLoan);
            journey.Add(typeof(ApplyPage), FillApplicationDetails);
            journey.Add(typeof(ProcessingPage), WaitForAcceptedPage);
            journey.Add(typeof(AcceptedPage), FillAcceptedPage);
            journey.Add(typeof(DealDonePage), GoToMySummaryPage);
        }

        protected override BaseLnJourney ApplyForLoan()
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = _amount.ToString();
            homePage.Sliders.HowLong = _duration.ToString();
            CurrentPage = homePage.Sliders.ApplyLn() as ApplyPage;
            return this;
        }

        protected override BaseLnJourney FillApplicationDetails()
        {
            var applyPage = CurrentPage as ApplyPage;
            CurrentPage = applyPage.Submit() as ProcessingPage;
            return this;
        }

        protected override BaseLnJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }

        protected override BaseLnJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        protected override BaseLnJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            acceptedPage.SignConfirmZA();
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        protected override BaseLnJourney GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPage;
            return this;
        }
    }
}

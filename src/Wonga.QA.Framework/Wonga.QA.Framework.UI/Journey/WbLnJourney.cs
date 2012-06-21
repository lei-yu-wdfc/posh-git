using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Ui.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;

namespace Wonga.QA.Framework.UI.Journey
{
    public class WbLnJourney : BaseLnJourney
    {
        public WbLnJourney(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;

            _amount = 5500;
            _duration = 20;

            journey.Add(typeof(HomePage), ApplyForLoan);
            journey.Add(typeof(ApplyPage), FillApplicationDetails);
            journey.Add(typeof(ProcessingPage), WaitForApplyTermsPage);
            journey.Add(typeof(ApplyTermsPage), ApplyTerms);
            journey.Add(typeof(AcceptedPage), FillAcceptedPage);
            journey.Add(typeof(DealDonePage), GoHomePage);
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

        protected override BaseLnJourney WaitForApplyTermsPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<ApplyTermsPage>() as ApplyTermsPage;
            return this;
        }

        protected override BaseLnJourney WaitForAcceptedPage()
        {
            throw new NotImplementedException(message: "Don't used on Wb");
            // var processingPage = CurrentPage as ProcessingPage;
            // CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            // return this;
        }

        protected override BaseLnJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        protected override BaseLnJourney ApplyTerms()
        {
            var applyTermsPage = CurrentPage as ApplyTermsPage;
            CurrentPage = applyTermsPage.Next();
            return this;
        }

        protected override BaseLnJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
            CurrentPage = acceptedPage.Submit() as ReferPage;
            return this;
        }

        protected override BaseLnJourney GoHomePage()
        {
            var referPage = CurrentPage as ReferPage;
            CurrentPage = referPage.GoHome();
            return this;
        }

        #region Builder

        public override BaseLnJourney WithDeclineDecision()
        {
            journey.Remove(typeof(ProcessingPage));
            journey.Remove(typeof (ApplyTermsPage));
            journey.Remove(typeof(AcceptedPage));
            journey.Remove(typeof(DealDonePage));
            journey.Add(typeof(ProcessingPage), WaitForDeclinedPage);
            return this;
        }

        #endregion
    }
}

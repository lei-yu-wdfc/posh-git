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
    public class WbLnJourney
    {
        public BasePage CurrentPage { get; set; }

        public WbLnJourney(BasePage homePage)
        {
            CurrentPage = homePage as HomePage;
        }

        public WbLnJourney ApplyForLoan(int amount, int duration)
        {
            var homePage = CurrentPage as HomePage;
            homePage.Sliders.HowMuch = amount.ToString();
            homePage.Sliders.HowLong = duration.ToString();
            CurrentPage = homePage.Sliders.ApplyLn() as ApplyPage;
            return this;
        }
        public WbLnJourney ApplyNow()
        {
            var applyPage = CurrentPage as ApplyPage;
            CurrentPage = applyPage.Submit() as ProcessingPage;
            return this;
        }

        public WbLnJourney WaitForApplyTermsPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<ApplyTermsPage>() as ApplyTermsPage;
            return this;
        }

        
        public WbLnJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<AcceptedPage>() as AcceptedPage;
            return this;
        }

        public WbLnJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPage;
            CurrentPage = processingPage.WaitFor<DeclinedPage>() as DeclinedPage;
            return this;
        }

        public WbLnJourney ApplyTerms()
        {
            var applyTermsPage = CurrentPage as ApplyTermsPage;
            CurrentPage = applyTermsPage.Next();
            return this;
        }

        public WbLnJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPage;
            acceptedPage.SignTermsMainApplicant();
            acceptedPage.SignTermsGuarantor();
            CurrentPage = acceptedPage.Submit() as ReferPage;
            return this;
        }

        public WbLnJourney GoHomePage()
        {
            var referPage = CurrentPage as ReferPage;
            CurrentPage = referPage.GoHome();
            return this;
        }
    }
}

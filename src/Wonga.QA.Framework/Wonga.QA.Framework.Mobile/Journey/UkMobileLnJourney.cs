using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Journey
{
    public class UkMobileLnJourney : BaseLnJourney
    {
        public UkMobileLnJourney(BasePageMobile homePage)
        {
            CurrentPage = homePage as HomePageMobile;

            _amount = 200;
            _duration = 10;

            journey.Add(typeof(HomePageMobile), ApplyForLoan);
            journey.Add(typeof(ApplyPageMobile), FillApplicationDetails);
            journey.Add(typeof(ProcessingPageMobile), WaitForAcceptedPage);
            journey.Add(typeof(AcceptedPageMobile), FillAcceptedPage);
            journey.Add(typeof(DealDonePage), GoToMySummaryPage);
            
        }

        protected override BaseLnJourney ApplyForLoan()
        {
            var homePage = CurrentPage as HomePageMobile;
            homePage.Sliders.HowMuch = _amount.ToString();
            homePage.Sliders.HowLong = _duration.ToString();
            CurrentPage = homePage.Sliders.ApplyLn() as ApplyPageMobile;
            return this;
        }

        protected override BaseLnJourney FillApplicationDetails()
        {
            var applyPage = CurrentPage as ApplyPageMobile;
            applyPage.ApplicationSection.SetSecurityCode = "000";
            applyPage.ApplicationSection.SetMinCash = "100";
            CurrentPage = applyPage.ClickApplyNowButton() as ProcessingPageMobile;
            return this;
        }

        protected override BaseLnJourney FillApplicationDetailsWithNewMobilePhone()
        {
            var applyPage = CurrentPage as ApplyPageMobile;
            applyPage.SetNewMobilePhone = _mobilePhone;
            applyPage.ApplicationSection.SetPin = "0000";
            applyPage.ApplicationSection.SetSecurityCode = "000";
            applyPage.ApplicationSection.SetMinCash = "100";
            CurrentPage = applyPage.ClickApplyNowButton() as ProcessingPageMobile;
            return this;
        }

        protected override BaseLnJourney WaitForAcceptedPage()
        {
            var processingPage = CurrentPage as ProcessingPageMobile;
            CurrentPage = processingPage.WaitFor<AcceptedPageMobile>() as AcceptedPageMobile;
            return this;
        }

        protected override BaseLnJourney WaitForDeclinedPage()
        {
            var processingPage = CurrentPage as ProcessingPageMobile;
            CurrentPage = processingPage.WaitFor<DeclinedPageMobile>() as DeclinedPageMobile;
            return this;
        }

        protected override BaseLnJourney FillAcceptedPage()
        {
            var acceptedPage = CurrentPage as AcceptedPageMobile;
            CurrentPage = acceptedPage.Submit() as DealDonePage;
            return this;
        }

        protected override BaseLnJourney GoToMySummaryPage()
        {
            var dealDonePage = CurrentPage as DealDonePage;
            CurrentPage = dealDonePage.ContinueToMyAccount() as MySummaryPageMobile;
            return this;
        }

        public override BaseLnJourney WithNewMobilePhone(string mobilePhone = null)
        {
            _mobilePhone = mobilePhone ?? Get.GetMobilePhone();

            journey.Clear();
            journey.Add(typeof(HomePageMobile), ApplyForLoan);
            journey.Add(typeof(ApplyPageMobile), FillApplicationDetailsWithNewMobilePhone);
            journey.Add(typeof(ProcessingPageMobile), WaitForAcceptedPage);
            journey.Add(typeof(AcceptedPageMobile), FillAcceptedPage);
            journey.Add(typeof(DealDonePage), GoToMySummaryPage);
            return this;
        }
    }
}

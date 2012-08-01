using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;

namespace Wonga.QA.Framework.UI.Journey
{
    public class UkFALnJourney : BaseFALnJourney
    {
        public UkFALnJourney(BasePage financialAssessmentPage)
        {
            CurrentPage = financialAssessmentPage as FinancialAssessmentPage;

            _firstName = Get.GetName();
            _lastName = Get.RandomString(10);
            _houseNumber = Get.RandomString(10);
            _dateOfBirth = "";
            _postCode = Get.GetPostcode();
            _yourHousehold = Get.RandomInt(1, 5).ToString();
            _childrenInHousehold = Get.RandomInt(1, 5).ToString();
            _numberOfVehiles = Get.RandomInt(1, 5).ToString();

            journey.Add(typeof(FinancialAssessmentPage), GetStarted);
            journey.Add(typeof(FAAboutYouPage), PassAboutYou);
            journey.Add(typeof(FAIncomePage), PassIncomePage);
        }

        protected override BaseFALnJourney GetStarted(bool submit = true)
        {
            var financialAssessmentPage = CurrentPage as FinancialAssessmentPage;
            CurrentPage = financialAssessmentPage.GetStartedClick() as FAAboutYouPage;
            return this;
        }

        protected override BaseFALnJourney PassAboutYou(bool submit = true)
        {
            var faAboutYouPage = CurrentPage as FAAboutYouPage;
            faAboutYouPage.FirstName = _firstName;
            faAboutYouPage.LastName = _lastName;
            faAboutYouPage.DateOfBirth = "1977-10-10";
            faAboutYouPage.HouseNumber = _houseNumber;
            faAboutYouPage.PostCode = _postCode;
            faAboutYouPage.Employer = _employer;
            faAboutYouPage.AdultsInHousehold = _yourHousehold;
            faAboutYouPage.ChidrenInHousehold = _childrenInHousehold;
            faAboutYouPage.NumberOfVehiles = _numberOfVehiles;

            if (submit)
            {
                CurrentPage = faAboutYouPage.NextClick() as FAIncomePage;
            }
            return this;
        }

        protected override BaseFALnJourney PassIncomePage(bool submit = true)
        {
            return this;
        }
    }
}

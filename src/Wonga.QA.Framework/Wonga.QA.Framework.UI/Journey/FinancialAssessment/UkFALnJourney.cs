using System;
using System.Collections.Generic;
using System.Globalization;
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

            _submit = true;

            _firstName = Get.GetName();
            _lastName = Get.RandomString(10);
            _houseNumber = Get.RandomString(10);
            _dateOfBirth = Get.GetDoB().ToString();
            _postCode = Get.GetPostcode();
            _yourHousehold = Get.RandomInt(1, 5).ToString();
            _childrenInHousehold = Get.RandomInt(1, 5).ToString();
            _numberOfVehiles = Get.RandomInt(1, 5).ToString();

            _salaryAfterTax = Get.RandomInt(0,10000).ToString() + Get.Random().ToString(".00");
            _partnerSalaryAfterTax = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _jobseekerAllowance = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _incomeSupport = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _workingTaxCredit = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _childTaxCredit = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _statePension = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _privateOrWorkPension = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _pensionCredit = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _other = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _maintenenceOrChildSupport = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _incomeFromBoardersOrLodgers = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _studentLoansOrGrants = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _otherIncome = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");

            journey.Add(typeof(FinancialAssessmentPage), GetStarted);
            journey.Add(typeof(FAAboutYouPage), PassAboutYou);
            journey.Add(typeof(FAIncomePage), PassIncomePage);
            journey.Add(typeof(FAExpenditurePage), PassExpenditurePage);
            journey.Add(typeof(FADebtsPage), PassDebtsPage);
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
            faAboutYouPage.DateOfBirth = _dateOfBirth;
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
            var faIncomePage = CurrentPage as FAIncomePage;
            faIncomePage.SalaryAfterTax = _salaryAfterTax;
            faIncomePage.PartnerSalaryAfterTax = _partnerSalaryAfterTax;
            faIncomePage.JobseekerAllowance = _jobseekerAllowance;
            faIncomePage.IncomeSupport = _incomeSupport;
            faIncomePage.WorkingTaxCredit = _workingTaxCredit;
            faIncomePage.ChildTaxCredit = _childTaxCredit;
            faIncomePage.StatePension = _statePension;
            faIncomePage.PrivateOrWorkPension = _privateOrWorkPension;
            faIncomePage.PensionCredit = _pensionCredit;
            faIncomePage.Other = _other;
            faIncomePage.MaintenenceOrChildSupport = _maintenenceOrChildSupport;
            faIncomePage.IncomeFromBoardersOrLodgers = _incomeFromBoardersOrLodgers;
            faIncomePage.StudentLoansOrGrants = _studentLoansOrGrants;
            faIncomePage.OtherIncome = _otherIncome;

            if (submit)
            {
                CurrentPage = faIncomePage.NextClick() as FAExpenditurePage;
            }
            return this;
        }

        protected override BaseFALnJourney PassExpenditurePage(bool submit = true)
        {
            var faExpenditurePage = CurrentPage as FAExpenditurePage;
           
            if (submit)
            {
                CurrentPage = faExpenditurePage.NextClick() as FADebtsPage;
            }
            return this;
        }

        protected override BaseFALnJourney PassDebtsPage(bool submit = true)
        {
            var faDebtsPage = CurrentPage as FADebtsPage;

            if (submit)
            {
               // CurrentPage = faDebtsPage.NextClick() as FADebtsPage;
            }
            return this;
        }
    }
}

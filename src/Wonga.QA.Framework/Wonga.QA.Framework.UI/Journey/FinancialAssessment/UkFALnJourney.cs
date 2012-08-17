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

            _salaryAfterTax = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _partnerSalaryAfterTax = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _jobseekerAllowance = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _incomeSupport = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _workingTaxCredit = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _childTaxCredit = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _statePension = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _privateOrWorkPension = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _pensionCredit = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _otherOnIncome = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _maintenenceOrChildSupport = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _incomeFromBoardersOrLodgers = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _studentLoansOrGrants = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _otherIncome = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");

            _rentPayments = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _mortgage = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _otherSecuredLoans = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _councilTax = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _maintenceOrChildSupport = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _gas = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _electricity = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _hirePurchaseOrConditionalSale = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _otherOnDebts = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor0 = Get.GetName();
            _nonPriorityDebtsAmount0 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor1 = Get.GetName();
            _nonPriorityDebtsAmount1 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor2 = Get.GetName();
            _nonPriorityDebtsAmount2 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor3 = Get.GetName();
            _nonPriorityDebtsAmount3 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor4 = Get.GetName();
            _nonPriorityDebtsAmount4 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor5 = Get.GetName();
            _nonPriorityDebtsAmount5 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor6 = Get.GetName();
            _nonPriorityDebtsAmount6 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor7 = Get.GetName();
            _nonPriorityDebtsAmount7 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor8 = Get.GetName();
            _nonPriorityDebtsAmount8 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
            _nonPriorityDebtsCreditor9 = Get.GetName();
            _nonPriorityDebtsAmount9 = Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");

            journey.Add(typeof(FinancialAssessmentPage), GetStarted);
            journey.Add(typeof(FAAboutYouPage), PassAboutYou);
            journey.Add(typeof(FAIncomePage), PassIncomePage);
            journey.Add(typeof(FAExpenditurePage), PassExpenditurePage);
            journey.Add(typeof(FADebtsPage), PassDebtsPage);
            journey.Add(typeof(FARepaymentPlanPage), PassRepaymentPlanPage);
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
            if (!_isQuickJump)
            {
                faIncomePage.PartnerSalaryAfterTax = _partnerSalaryAfterTax;
                faIncomePage.JobseekerAllowance = _jobseekerAllowance;
                faIncomePage.IncomeSupport = _incomeSupport;
                faIncomePage.WorkingTaxCredit = _workingTaxCredit;
                faIncomePage.ChildTaxCredit = _childTaxCredit;
                faIncomePage.StatePension = _statePension;
                faIncomePage.PrivateOrWorkPension = _privateOrWorkPension;
                faIncomePage.PensionCredit = _pensionCredit;
                faIncomePage.Other = _otherOnIncome;
                faIncomePage.MaintenenceOrChildSupport = _maintenenceOrChildSupport;
                faIncomePage.IncomeFromBoardersOrLodgers = _incomeFromBoardersOrLodgers;
                faIncomePage.StudentLoansOrGrants = _studentLoansOrGrants;
            }
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

            if (!_isQuickJump)
            {
                faDebtsPage.RentPayments = _rentPayments;
                faDebtsPage.Mortgage = _mortgage;
                faDebtsPage.OtherSecuredLoans = _otherSecuredLoans;
                faDebtsPage.CouncilTax = _councilTax;
                faDebtsPage.MaintenanceOrChildSupport = _maintenceOrChildSupport;
                faDebtsPage.Gas = _gas;
                faDebtsPage.Electricity = _electricity;
                faDebtsPage.HirePurchaseOrConditionalSale = _hirePurchaseOrConditionalSale;
                faDebtsPage.Other = _otherOnDebts;
                faDebtsPage.NonPriorityDebtsCreditor0 = _nonPriorityDebtsCreditor0;
                faDebtsPage.NonPriorityDebtsAmount0 = _nonPriorityDebtsAmount0;
                faDebtsPage.NonPriorityDebtsCreditor1 = _nonPriorityDebtsCreditor1;
                faDebtsPage.NonPriorityDebtsAmount1 = _nonPriorityDebtsAmount1;
                faDebtsPage.NonPriorityDebtsCreditor2 = _nonPriorityDebtsCreditor2;
                faDebtsPage.NonPriorityDebtsAmount2 = _nonPriorityDebtsAmount2;
                faDebtsPage.NonPriorityDebtsCreditor3 = _nonPriorityDebtsCreditor3;
                faDebtsPage.NonPriorityDebtsAmount3 = _nonPriorityDebtsAmount3;
                faDebtsPage.NonPriorityDebtsCreditor4 = _nonPriorityDebtsCreditor4;
                faDebtsPage.NonPriorityDebtsAmount4 = _nonPriorityDebtsAmount4;
                faDebtsPage.NonPriorityDebtsCreditor5 = _nonPriorityDebtsCreditor5;
                faDebtsPage.NonPriorityDebtsAmount5 = _nonPriorityDebtsAmount5;
                faDebtsPage.NonPriorityDebtsCreditor6 = _nonPriorityDebtsCreditor6;
                faDebtsPage.NonPriorityDebtsAmount6 = _nonPriorityDebtsAmount6;
                faDebtsPage.NonPriorityDebtsCreditor7 = _nonPriorityDebtsCreditor7;
                faDebtsPage.NonPriorityDebtsAmount7 = _nonPriorityDebtsAmount7;
                faDebtsPage.NonPriorityDebtsCreditor8 = _nonPriorityDebtsCreditor8;
                faDebtsPage.NonPriorityDebtsAmount8 = _nonPriorityDebtsAmount8;
                faDebtsPage.NonPriorityDebtsCreditor9 = _nonPriorityDebtsCreditor9;
                faDebtsPage.NonPriorityDebtsAmount9 = _nonPriorityDebtsAmount9;
            }

            if (submit)
            {
                CurrentPage = faDebtsPage.NextClick() as FARepaymentPlanPage;
            }
            return this;
        }

        protected override BaseFALnJourney PassRepaymentPlanPage(bool submit = true)
        {
            return this;
        }
    }
}

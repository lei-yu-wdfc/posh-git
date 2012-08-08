using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;

namespace Wonga.QA.Framework.UI
{
    public abstract class BaseFALnJourney
    {
        protected Dictionary<Type, Func<bool, BaseFALnJourney>> journey = new Dictionary<Type, Func<bool, BaseFALnJourney>>();
        protected bool _submit;

        protected String _firstName;
        protected String _lastName;
        protected String _dateOfBirth;
        protected String _houseNumber;
        protected String _postCode;
        protected String _employer = "";
        protected String _yourHousehold;
        protected String _childrenInHousehold;
        protected String _numberOfVehiles;

        protected String _salaryAfterTax;
        protected String _partnerSalaryAfterTax;
        protected String _jobseekerAllowance;
        protected String _incomeSupport;
        protected String _workingTaxCredit;
        protected String _childTaxCredit;
        protected String _statePension;
        protected String _privateOrWorkPension;
        protected String _pensionCredit;
        protected String _other;
        protected String _maintenenceOrChildSupport;
        protected String _incomeFromBoardersOrLodgers;
        protected String _studentLoansOrGrants;
        protected String _otherIncome;

        public BasePage CurrentPage { get; set; }

        public BasePage Teleport<T>()
        {
            var pageType = typeof(T);
            var currentIndex = CurrentPage == null ? 0 : journey.Keys.ToList().IndexOf(CurrentPage.GetType());
            for (int i = currentIndex; i < journey.Keys.Count; i++)
            {
                if (CurrentPage.GetType() == pageType && pageType != typeof(FinancialAssessmentPage))
                {
                    if (!_submit)
                    {
                        journey.ElementAt(i).Value.Invoke(_submit);
                    }
                    return CurrentPage;
                }
                else
                {
                    journey.ElementAt(i).Value.Invoke(true);
                }
            }
            return CurrentPage;
        }

        protected abstract BaseFALnJourney GetStarted(bool submit = true);
        protected abstract BaseFALnJourney PassAboutYou(bool submit = true);
        protected abstract BaseFALnJourney PassIncomePage(bool submit = true);
        protected abstract BaseFALnJourney PassExpenditurePage(bool submit = true);
        protected abstract BaseFALnJourney PassDebtsPage(bool submit = true);

        public virtual BaseFALnJourney FillAndStop()
        {
            _submit = false;
            return this;
        }

        public virtual BaseFALnJourney WithEmployer(string employer)
        {
            _employer = employer;
            return this;
        }
    }
}

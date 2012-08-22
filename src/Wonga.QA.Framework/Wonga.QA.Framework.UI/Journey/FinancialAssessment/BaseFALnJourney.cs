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
        protected bool _isQuickJump;

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
        protected String _otherOnIncome;
        protected String _maintenenceOrChildSupport;
        protected String _incomeFromBoardersOrLodgers;
        protected String _studentLoansOrGrants;
        protected String _otherIncome;

        protected String _rent;

        protected String _rentPayments;
        protected String _mortgage;
        protected String _otherSecuredLoans;
        protected String _councilTax;
        protected String _maintenceOrChildSupport;
        protected String _gas;
        protected String _electricity;
        protected String _hirePurchaseOrConditionalSale;
        protected String _otherOnDebts;
        protected String _nonPriorityDebtsCreditor0;
        protected String _nonPriorityDebtsAmount0;
        protected String _nonPriorityDebtsCreditor1;
        protected String _nonPriorityDebtsAmount1;
        protected String _nonPriorityDebtsCreditor2;
        protected String _nonPriorityDebtsAmount2;
        protected String _nonPriorityDebtsCreditor3;
        protected String _nonPriorityDebtsAmount3;
        protected String _nonPriorityDebtsCreditor4;
        protected String _nonPriorityDebtsAmount4;
        protected String _nonPriorityDebtsCreditor5;
        protected String _nonPriorityDebtsAmount5;
        protected String _nonPriorityDebtsCreditor6;
        protected String _nonPriorityDebtsAmount6;
        protected String _nonPriorityDebtsCreditor7;
        protected String _nonPriorityDebtsAmount7;
        protected String _nonPriorityDebtsCreditor8;
        protected String _nonPriorityDebtsAmount8;
        protected String _nonPriorityDebtsCreditor9;
        protected String _nonPriorityDebtsAmount9;

        protected String _firstRepaymentDate;
        protected String _paymentFrequency;
        protected String _repaymentAmount;

        protected BasePage _desisionPage;

        public BasePage CurrentPage { get; set; }

        public BasePage Teleport<T>()
        {
            var pageType = typeof (T);

            if (pageType == typeof (FAAcceptedPage))
                WithRepaymentAmount("12414");

            if (pageType == typeof (FACounterOfferPage))
            {
                WithTotalIncome("121234");

                WithRepaymentAmount("0");
            }
        

    if (pageType == typeof(FARejectedPage))
    {
        WithTotalExpediture("121234");
        WithRepaymentAmount("0");
    }

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
        protected abstract BaseFALnJourney PassRepaymentPlanPage(bool submit = true);
        protected abstract BaseFALnJourney PassAcceptedPage(bool submit = true);
        protected abstract BaseFALnJourney PassCounterOfferPage(bool submit = true);
        protected abstract BaseFALnJourney PassRejectedPage(bool submit = true);

        public virtual BaseFALnJourney FillAndStop()
        {
            _submit = false;
            return this;
        }

        public virtual BaseFALnJourney QuickJump()
        {
            _isQuickJump = true;
            return this;
        }

        public virtual BaseFALnJourney WithEmployer(string employer)
        {
            _employer = employer;
            return this;
        }

        public virtual BaseFALnJourney WithFirstRepaymentDate(string firstRepaymentDate)
        {
            _firstRepaymentDate = firstRepaymentDate;
            return this;
        }

        public virtual BaseFALnJourney WithPaymentFrequency(string paymentFrequency)
        {
            _paymentFrequency = paymentFrequency;
            return this;
        }

        public virtual BaseFALnJourney WithRepaymentAmount(string repaymentAmount)
        {
            _repaymentAmount = repaymentAmount;
            return this;
        }

        public virtual BaseFALnJourney WithTotalIncome(string totalIncome)
        {
            _salaryAfterTax = totalIncome;
            return this;
        }

        public virtual BaseFALnJourney WithTotalExpediture(string totalExpediture)
        {
            _rent = totalExpediture;
            return this;
        }

        public virtual BaseFALnJourney WithTotalNonPriorityDebts(string nonPriorityDebts)
        {
            _nonPriorityDebtsAmount1 = nonPriorityDebts;
            return this;
        }

        public virtual BaseFALnJourney WithTotalPriorityDebts(string priorityDebts)
        {
            _rentPayments = priorityDebts;
            return this;
        }
    }
}

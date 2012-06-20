using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Journey
{
    public abstract class BaseLnJourney
    {
        protected Dictionary<Type, Func<bool, BaseL0Journey>> journey = new Dictionary<Type, Func<bool, BaseL0Journey>>();

        protected bool _submit;

        protected int _amount;
        protected int _duration;

        protected String _firstName;
        protected String _lastName;

        public BasePage CurrentPage { get; set; }

        public BasePage Teleport<T>()
        {
            var pageType = typeof(T);
            var currentIndex = CurrentPage == null ? 0 : journey.Keys.ToList().IndexOf(CurrentPage.GetType());
            for (int i = currentIndex; i < journey.Keys.Count; i++)
            {
                if (CurrentPage.GetType() == pageType && pageType != typeof(HomePage))
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

        protected abstract BaseLnJourney SetName(string forename, string surname);
        protected abstract BaseLnJourney ApplyForLoan(int amount, int duration);
        protected abstract BaseLnJourney FillApplicationDetails();
        protected abstract BaseLnJourney WaitForAcceptedPage();
        protected abstract BaseLnJourney WaitForDeclinedPage();
        protected abstract BaseLnJourney FillAcceptedPage();
        protected abstract BaseLnJourney GoToMySummaryPage();
    }
}

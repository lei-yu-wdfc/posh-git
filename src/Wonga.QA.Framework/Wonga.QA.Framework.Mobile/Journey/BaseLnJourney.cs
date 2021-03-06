﻿using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile.Journey
{
    public abstract class BaseLnJourney
    {
        protected Dictionary<Type, Func<BaseLnJourney>> journey = new Dictionary<Type, Func<BaseLnJourney>>();

        protected int _amount;
        protected int _duration;

        protected String _mobilePhone ;

        protected String _firstName;
        protected String _lastName;

        public BasePageMobile CurrentPage { get; set; }

        public BasePageMobile Teleport<T>()
        {
            var pageType = typeof(T);
            var currentIndex = CurrentPage == null ? 0 : journey.Keys.ToList().IndexOf(CurrentPage.GetType());
            for (int i = currentIndex; i < journey.Keys.Count; i++)
            {
                if (CurrentPage.GetType() == pageType && pageType != typeof(HomePageMobile))
                {
                    return CurrentPage;
                }
                else
                {
                    journey.ElementAt(i).Value.Invoke();
                }
            }
            return CurrentPage;
        }

        protected abstract BaseLnJourney ApplyForLoan();
        protected virtual BaseLnJourney FillApplicationDetails()
        {
            return this;
        }

        protected virtual BaseLnJourney FillApplicationDetailsWithNewMobilePhone()
        {
            throw new NotImplementedException(message:"Used only on Uk");
        }

        protected abstract BaseLnJourney WaitForAcceptedPage();
        protected abstract BaseLnJourney WaitForDeclinedPage();
        protected abstract BaseLnJourney FillAcceptedPage();
        protected virtual BaseLnJourney GoToMySummaryPage()
        {
            return this;
        }

        protected virtual BaseLnJourney WaitForApplyTermsPage()
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseLnJourney ApplyTerms()
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        protected virtual BaseLnJourney GoHomePage()
        {
            throw new NotImplementedException(message: "Used only on Wb");
        }

        #region Builder
        public virtual BaseLnJourney WithFirstName(string firstNme)
        {
            throw new NotImplementedException(message: "Used only on Ca");
        }
        public virtual BaseLnJourney WithLastName(string lastName)
        {
            throw new NotImplementedException(message: "Used only on Ca");
        }

        public BaseLnJourney WithAmount(int amount)
        {
            _amount = amount;
            return this;
        }

        public BaseLnJourney WithDuration(int duration)
        {
            _duration = duration;
            return this;
        }

        public virtual BaseLnJourney WithDeclineDecision()
        {
            journey.Remove(typeof(ProcessingPageMobile));
            journey.Remove(typeof(AcceptedPageMobile));
            journey.Remove(typeof(DealDonePage));
            journey.Add(typeof(ProcessingPageMobile), WaitForDeclinedPage);
            return this;
        }

       public virtual BaseLnJourney WithNewMobilePhone(string mobilePhone = null)
       {
           throw new NotImplementedException(message: "Used only on Uk");
       }

        #endregion
    }
}

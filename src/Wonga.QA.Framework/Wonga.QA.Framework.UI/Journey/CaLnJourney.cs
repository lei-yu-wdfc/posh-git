using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
    class CaLnJourney : ILnConsumerJourney
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BasePage CurrentPage { get; set; }

        public CaLnJourney(BasePage basePage)
        {
            FirstName = Get.GetName();
            LastName = Get.RandomString(10);
        }
        public ILnConsumerJourney ApplyForLoan(int amount, int duration)
        {

            return this;
        }

        public ILnConsumerJourney FillApplicationDetails()
        {
            throw new NotImplementedException();
        }

        public ILnConsumerJourney WaitForAcceptedPage()
        {
            throw new NotImplementedException();
        }

        public ILnConsumerJourney WaitForDeclinedPage()
        {
            throw new NotImplementedException();
        }

        public ILnConsumerJourney FillAcceptedPage()
        {
            throw new NotImplementedException();
        }

        public ILnConsumerJourney GoToMySummaryPage()
        {
            throw new NotImplementedException();
        }
    }
}

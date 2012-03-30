using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
    public interface ILnConsumerJourney
    {
        String FirstName { get; set; }
        String LastName { get; set; }

        BasePage CurrentPage { get; set; }

        ILnConsumerJourney ApplyForLoan(int amount, int duration);
        ILnConsumerJourney FillApplicationDetails();
        ILnConsumerJourney WaitForAcceptedPage();
        ILnConsumerJourney WaitForDeclinedPage();
        ILnConsumerJourney FillAcceptedPage();
        ILnConsumerJourney GoToMySummaryPage();

    }
}

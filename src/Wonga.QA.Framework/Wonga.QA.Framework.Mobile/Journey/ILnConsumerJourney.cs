using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Mobile.Ui.Pages;

namespace Wonga.QA.Framework.Mobile
{
    public interface ILnConsumerJourney
    {
        String FirstName { get; set; }
        String LastName { get; set; }

        BasePageMobile CurrentPage { get; set; }

        ILnConsumerJourney SetName(string forename, string surname);
        ILnConsumerJourney ApplyForLoan(int amount, int duration);
        ILnConsumerJourney FillApplicationDetails();
        ILnConsumerJourney WaitForAcceptedPage();
        ILnConsumerJourney WaitForDeclinedPage();
        ILnConsumerJourney FillAcceptedPage();
        ILnConsumerJourney GoToMySummaryPage();

    }
}

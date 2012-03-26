using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
       
    public interface IConsumerJourney
    {
        String FirstName { get; set; }
        String LastName { get; set; }

        BasePage CurrentPage { get; set; }
        
        IConsumerJourney ApplyForLoan(int amount, int duration);
        IConsumerJourney FillPersonalDetails(string employerNameMask = null);
        IConsumerJourney FillAddressDetails();
        IConsumerJourney FillAccountDetails();
        IConsumerJourney FillBankDetails();
        IConsumerJourney FillCardDetails();
        IConsumerJourney WaitForAcceptedPage();
        IConsumerJourney WaitForDeclinedPage();
        IConsumerJourney FillAcceptedPage();
        IConsumerJourney GoToMySummaryPage();
    }
}

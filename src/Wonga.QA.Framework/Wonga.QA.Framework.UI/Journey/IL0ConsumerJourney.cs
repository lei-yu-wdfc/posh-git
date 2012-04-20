using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
       
    public interface IL0ConsumerJourney
    {
        String FirstName { get; set; }
        String LastName { get; set; }
		String NationalId { get; set; }
		DateTime DateOfBirth { get; set; }

        BasePage CurrentPage { get; set; }
        
        IL0ConsumerJourney ApplyForLoan(int amount, int duration);
        IL0ConsumerJourney FillPersonalDetails(string employerNameMask = null);
        IL0ConsumerJourney FillPersonalDetailsWithEmail(string employerNameMask = null, string email = null);
        IL0ConsumerJourney FillAddressDetails();
        IL0ConsumerJourney FillAccountDetails();
        IL0ConsumerJourney FillBankDetails();
        IL0ConsumerJourney FillCardDetails();
        IL0ConsumerJourney WaitForAcceptedPage();
        IL0ConsumerJourney WaitForDeclinedPage();
        IL0ConsumerJourney FillAcceptedPage();
        IL0ConsumerJourney GoToMySummaryPage();
    }
}

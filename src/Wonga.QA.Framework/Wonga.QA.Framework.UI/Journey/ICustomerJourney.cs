using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI
{
       
    public interface ICustomerJourney
    {
        String FirstName { get; set; }
        String LastName { get; set; }

        BasePage CurrentPage { get; set; }
        
        ICustomerJourney ApplyForLoan(int amount, int duration);
        ICustomerJourney FillPersonalDetails(string employerNameMask = null);
        ICustomerJourney FillAddressDetails();
        ICustomerJourney FillAccountDetails();
        ICustomerJourney FillBankDetails();
        ICustomerJourney FillCardDetails();
        ICustomerJourney WaitForAcceptedPage();
        ICustomerJourney WaitForDeclinedPage();
        ICustomerJourney FillAcceptedPage();
        ICustomerJourney GoToMySummaryPage();
    }
}

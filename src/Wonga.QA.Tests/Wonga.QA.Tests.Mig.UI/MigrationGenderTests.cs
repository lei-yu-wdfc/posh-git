using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Ui;
using MbUnit.Framework;

namespace Wonga.QA.Tests.Migration
{
    public class MigrationGenderTests:UiTest
    {
     [Test]  
        public void AddCustomerWithTitleMrAndGenderFemale()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home());

            var acceptedPage = journey.ApplyForLoan(200, 10)
                                     .FillPersonalDetails(null, null, null,"Female")
                                     .FillAddressDetails()
                                     .FillAccountDetails()
                                     .FillBankDetails()
                                     .FillCardDetails()
                                     .WaitForAcceptedPage().CurrentPage as AcceptedPage;
        }
    }
}

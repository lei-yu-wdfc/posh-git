using Wonga.QA.Framework.Api;
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
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithGender(GenderEnum.Female);

            var acceptedPage = journey.ApplyForLoan()
                                     .FillPersonalDetails()
                                     .FillAddressDetails()
                                     .FillAccountDetails()
                                     .FillBankDetails()
                                     .FillCardDetails()
                                     .WaitForAcceptedPage().CurrentPage as AcceptedPage;
        }
    }
}

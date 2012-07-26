using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using MbUnit.Framework;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.MigrationTests
{
    [Parallelizable(TestScope.All)]
    public class MigrationGenderTests : UiTest
    {
        [Test]
        public void AddCustomerWithTitleMrAndGenderFemale()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithGender(GenderEnum.Female);

            var acceptedPage = journey.Teleport<AcceptedPage>() as AcceptedPage;
        }
    }
}

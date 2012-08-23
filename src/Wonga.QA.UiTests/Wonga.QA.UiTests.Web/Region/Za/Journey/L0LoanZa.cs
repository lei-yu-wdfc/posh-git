using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.UiTests.Web.Region.Za.Journey
{
    [Parallelizable(TestScope.All)]
    public class L0LoanZa : UiTest
    {
        [Test, AUT(AUT.Za)]
        public void ZaAcceptedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask));
            var mySummary = journey.Teleport<MySummaryPage>() as MySummaryPage;
        }

        [Test, AUT(AUT.Za)]
        public void ZaDeclinedLoan()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
        }

        [Test, AUT(AUT.Za), JIRA("QA-278"), Pending("ZA-2302")]
        public void ZaDeclinedPageContainsHeaderLinks()
        {
            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithDeclineDecision();
            var declinedPage = journey.Teleport<DeclinedPage>() as DeclinedPage;
            declinedPage.LookForHeaderLinks();
        }

        [Test, AUT(AUT.Za), JIRA("ZA-2776"), Pending("Awaiting fix of ZA-2776"), Owner(Owner.StanDesyatnikov)]
        public void ZaL0SelfEmployedMonthlyIncome()
        {
            var email = Get.RandomEmail();
            var employerName = Get.EnumToString(RiskMask.TESTEmployedMask);
            var dateOfBirth = new DateTime(1957, 10, 30);
            var gender = GenderEnum.Female;
            Console.WriteLine("Email: {0}\n EmployerName={1}", email, employerName);

            var journey = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(employerName);
            var applyPage = journey.Teleport<PersonalDetailsPage>() as PersonalDetailsPage;

            var personalDetailsPage = new PersonalDetailsPage(Client);

            personalDetailsPage.YourName.FirstName = Get.GetName();
            personalDetailsPage.YourName.MiddleName = Get.GetMiddleName();
            personalDetailsPage.YourName.LastName = Get.GetName();
            personalDetailsPage.YourName.Title = "Mrs";
            personalDetailsPage.YourDetails.Number = Get.GetNationalNumber(dateOfBirth, gender == GenderEnum.Female);
            personalDetailsPage.YourDetails.DateOfBirth = dateOfBirth.ToString("d/MMM/yyyy");
            personalDetailsPage.YourDetails.Gender = gender.ToString();
            personalDetailsPage.YourDetails.HomeStatus = "Owner Occupier";
            personalDetailsPage.YourDetails.HomeLanguage = "English";
            personalDetailsPage.YourDetails.NumberOfDependants = "0";
            personalDetailsPage.YourDetails.MaritalStatus = "Single";
            personalDetailsPage.EmploymentDetails.EmploymentStatus = "Self Employed";
            personalDetailsPage.EmploymentDetails.SelfEmployedMonthlyIncome= "3000";
            personalDetailsPage.EmploymentDetails.WorkPhone = Get.GetPhone();
            personalDetailsPage.EmploymentDetails.SalaryPaidToBank = true;
            personalDetailsPage.EmploymentDetails.SelfNextPayDate = DateTime.Now.Add(TimeSpan.FromDays(5)).ToString("d/MMM/yyyy");
            personalDetailsPage.EmploymentDetails.IncomeFrequency = "Monthly";
            personalDetailsPage.ContactingYou.CellPhoneNumber = Get.GetMobilePhone();
            personalDetailsPage.ContactingYou.EmailAddress = email;
            personalDetailsPage.ContactingYou.ConfirmEmailAddress = email;
            personalDetailsPage.PrivacyPolicy = true;
            personalDetailsPage.CanContact = "Yes";
            personalDetailsPage.MarriedInCommunityProperty ="I am not married in community of property (I am single, married with antenuptial contract, divorced etc.)";
            var addressDetailsPage = personalDetailsPage.Submit() as AddressDetailsPage;
        }
       
    }
}

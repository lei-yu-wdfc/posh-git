using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssessmentDebtsTest : UiTest
    {
        public Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            return customer;
        }

        public double MoneyToDouble(String money)
        {
            return (double)(int.Parse(money.Split('.')[0]) * 100 +
                   int.Parse(money.Split('.')[1])) / 100;
        }

        public string GetRandomMoney()
        {
            return Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
        }

        public string GetRandomSpecialSymbolsString()
        {
            int lenght = Get.RandomInt(1, 10);
            String strForOut = "";
            List<char> specialsymbols = new List<char>();
            specialsymbols.AddRange(@"!@#$%^&*()_".ToCharArray());
            for (int i = 0; i < lenght; i++)
            {
                strForOut += Get.RandomElement<char>(specialsymbols).ToString();
            }

            return strForOut;
        }

        public delegate string InvalidMoneyGenerator();
        public void EnterInvalidDataPriorityDebts(FADebtsPage fadebtspage, InvalidMoneyGenerator GetData, String InvalidDataType)
        {
            fadebtspage.RentPayments = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.RentPaymentsErrorPresent(), "Enter " + InvalidDataType + " in RentPayments check");
            fadebtspage.RentPayments = GetRandomMoney();

            fadebtspage.Mortgage = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.MortgageErrorPresent(), "Enter " + InvalidDataType + " in Mortgage check");
            fadebtspage.Mortgage = GetRandomMoney();

            fadebtspage.OtherSecuredLoans = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.OtherSecuredLoansErrorPresent(), "Enter " + InvalidDataType + " in OtherSecuredLoans check");
            fadebtspage.OtherSecuredLoans = GetRandomMoney();

            fadebtspage.CouncilTax = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.CouncilTaxErrorPresent(), "Enter " + InvalidDataType + " in CouncilTax check");
            fadebtspage.CouncilTax = GetRandomMoney();

            fadebtspage.MaintenanceOrChildSupport = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.MaintenceOrChildSupportErrorPresent(), "Enter " + InvalidDataType + " in MaintenanceOrChildSupport check");
            fadebtspage.MaintenanceOrChildSupport = GetRandomMoney();

            fadebtspage.Gas = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.GasErrorPresent(), "Enter " + InvalidDataType + " in Gas check");
            fadebtspage.Gas = GetRandomMoney();

            fadebtspage.Electricity = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.ElectricityErrorPresent(), "Enter " + InvalidDataType + " in Electricity check");
            fadebtspage.Electricity = GetRandomMoney();

            fadebtspage.HirePurchaseOrConditionalSale = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.HirePurchaseOrConditionalSaleErrorPresent(), "Enter " + InvalidDataType + " in HirePurchaseOrConditionalSale check");
            fadebtspage.HirePurchaseOrConditionalSale = GetRandomMoney();

            fadebtspage.Other = GetData();
            fadebtspage.ClickNonPriorityDebtsAmount0();
            Assert.IsTrue(fadebtspage.OtherErrorPresent(), "Enter " + InvalidDataType + " in Other check");
            fadebtspage.Other = GetRandomMoney();
        }

        public void EnterInvalidDataNonPriorityDebts(FADebtsPage fadebtspage, InvalidMoneyGenerator GetData, String InvalidDataType)
        {
            fadebtspage.NonPriorityDebtsAmount0 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount0ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount0 check");
            fadebtspage.NonPriorityDebtsAmount0 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount1 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount1ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount1 check");
            fadebtspage.NonPriorityDebtsAmount1 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount2 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount2ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount2 check");
            fadebtspage.NonPriorityDebtsAmount2 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount3 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount3ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount3 check");
            fadebtspage.NonPriorityDebtsAmount3 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount4 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount4ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount4 check");
            fadebtspage.NonPriorityDebtsAmount4 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount5 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount5ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount5 check");
            fadebtspage.NonPriorityDebtsAmount5 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount6 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount6ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount6 check");
            fadebtspage.NonPriorityDebtsAmount6 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount7 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount7ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount7 check");
            fadebtspage.NonPriorityDebtsAmount7 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount8 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount8ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount8 check");
            fadebtspage.NonPriorityDebtsAmount8 = GetRandomMoney();

            fadebtspage.NonPriorityDebtsAmount9 = GetData();
            fadebtspage.ClickOther();
            Assert.IsTrue(fadebtspage.NonPriorityDebtsAmount9ErrorPresent(),
                          "Enter " + InvalidDataType + " in NonPriorityDebtsAmount9 check");
            fadebtspage.NonPriorityDebtsAmount9 = GetRandomMoney();
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-708"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void PrepopulatedCurrentBalanceCheck()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var fadebtspage = journeyLn.Teleport<FADebtsPage>() as FADebtsPage;

            Assert.AreEqual((double)customer.GetApplication().GetBalanceToday(), MoneyToDouble(fadebtspage.GetPrepopulatedCurrentBalance()));
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-708"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void EnterInvalidDataErrorCheck()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var fadebtspage = journeyLn.FillAndStop().Teleport<FADebtsPage>() as FADebtsPage;

            EnterInvalidDataPriorityDebts(fadebtspage, () => Get.GetName(), "Letters");
            EnterInvalidDataPriorityDebts(fadebtspage, () => GetRandomMoney().Insert(0, "-"), "Negative Numbers");
            EnterInvalidDataPriorityDebts(fadebtspage, () => GetRandomSpecialSymbolsString(), "Special symbols");
            EnterInvalidDataPriorityDebts(fadebtspage, () => GetRandomMoney().Replace('.', ','), "Right Number split by coma");
            EnterInvalidDataPriorityDebts(fadebtspage, () => GetRandomMoney() + Get.RandomInt(0, 10000), "Right Number with many numbers after split");

            EnterInvalidDataNonPriorityDebts(fadebtspage, () => Get.GetName(), "Letters");
            EnterInvalidDataNonPriorityDebts(fadebtspage, () => GetRandomMoney().Insert(0, "-"), "Negative Numbers");
            EnterInvalidDataNonPriorityDebts(fadebtspage, () => GetRandomSpecialSymbolsString(), "Special symbols");
            EnterInvalidDataNonPriorityDebts(fadebtspage, () => GetRandomMoney().Replace('.', ','), "Right Number split by coma");
            EnterInvalidDataNonPriorityDebts(fadebtspage, () => GetRandomMoney() + Get.RandomInt(0, 10000), "Right Number with many numbers after split");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-708"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithValidDataInPrioritySectionAndCheckDataAfterGoBack()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var fadebtspage = journeyLn.FillAndStop().Teleport<FADebtsPage>() as FADebtsPage;

            var rentPayments = fadebtspage.RentPayments;
            var mortgage = fadebtspage.Mortgage;
            var otherSecuredLoans = fadebtspage.OtherSecuredLoans;
            var councilTax = fadebtspage.CouncilTax;
            var maintenanceOrChildSupport = fadebtspage.MaintenanceOrChildSupport;
            var gas = fadebtspage.Gas;
            var electricity = fadebtspage.Electricity;
            var hirePurchaseOrConditionalSale = fadebtspage.HirePurchaseOrConditionalSale;
            var other = fadebtspage.Other;

            var farepaymentplanpage = fadebtspage.NextClick() as FARepaymentPlanPage;
            Assert.Contains(farepaymentplanpage.Url, "/repayment-plan", "Pass to rapayment plan page, with right data in fields");

            fadebtspage = farepaymentplanpage.PreviousClick() as FADebtsPage;

            Assert.AreEqual(fadebtspage.RentPayments, rentPayments, "RentPayments check after go back");
            Assert.AreEqual(fadebtspage.Mortgage, mortgage, "Mortgage check after go back");
            Assert.AreEqual(fadebtspage.OtherSecuredLoans, otherSecuredLoans, "OtherSecuredLoans check after go back");
            Assert.AreEqual(fadebtspage.CouncilTax, councilTax, "CouncilTax check after go back");
            Assert.AreEqual(fadebtspage.MaintenanceOrChildSupport, maintenanceOrChildSupport, "MaintenanceOrChildSupport check after go back");
            Assert.AreEqual(fadebtspage.Gas, gas, "Gas check after go back");
            Assert.AreEqual(fadebtspage.Electricity, electricity, "Electricity check after go back");
            Assert.AreEqual(fadebtspage.HirePurchaseOrConditionalSale, hirePurchaseOrConditionalSale, "HirePurchaseOrConditionalSale check after go back");
            Assert.AreEqual(fadebtspage.Other, other, "Other check after go back");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-708"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithValidDataInNonPrioritySectionAndCheckDataAfterGoBack()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var fadebtspage = journeyLn.FillAndStop().Teleport<FADebtsPage>() as FADebtsPage;

            var nonPriorityDebtsCreditor0 = fadebtspage.NonPriorityDebtsCreditor0;
            var nonPriorityDebtsCreditor1 = fadebtspage.NonPriorityDebtsCreditor1;
            var nonPriorityDebtsCreditor2 = fadebtspage.NonPriorityDebtsCreditor2;
            var nonPriorityDebtsCreditor3 = fadebtspage.NonPriorityDebtsCreditor3;
            var nonPriorityDebtsCreditor4 = fadebtspage.NonPriorityDebtsCreditor4;
            var nonPriorityDebtsCreditor5 = fadebtspage.NonPriorityDebtsCreditor5;
            var nonPriorityDebtsCreditor6 = fadebtspage.NonPriorityDebtsCreditor6;
            var nonPriorityDebtsCreditor7 = fadebtspage.NonPriorityDebtsCreditor7;
            var nonPriorityDebtsCreditor8 = fadebtspage.NonPriorityDebtsCreditor8;
            var nonPriorityDebtsCreditor9 = fadebtspage.NonPriorityDebtsCreditor9;

            var nonPriorityDebtsAmount0 = fadebtspage.NonPriorityDebtsAmount0;
            var nonPriorityDebtsAmount1 = fadebtspage.NonPriorityDebtsAmount1;
            var nonPriorityDebtsAmount2 = fadebtspage.NonPriorityDebtsAmount2;
            var nonPriorityDebtsAmount3 = fadebtspage.NonPriorityDebtsAmount3;
            var nonPriorityDebtsAmount4 = fadebtspage.NonPriorityDebtsAmount4;
            var nonPriorityDebtsAmount5 = fadebtspage.NonPriorityDebtsAmount5;
            var nonPriorityDebtsAmount6 = fadebtspage.NonPriorityDebtsAmount6;
            var nonPriorityDebtsAmount7 = fadebtspage.NonPriorityDebtsAmount7;
            var nonPriorityDebtsAmount8 = fadebtspage.NonPriorityDebtsAmount8;
            var nonPriorityDebtsAmount9 = fadebtspage.NonPriorityDebtsAmount9;

            var farepaymentplanpage = fadebtspage.NextClick() as FARepaymentPlanPage;
            Assert.Contains(farepaymentplanpage.Url, "/repayment-plan", "Pass to rapayment plan page, with right data in fields");

            fadebtspage = farepaymentplanpage.PreviousClick() as FADebtsPage;

            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor0, nonPriorityDebtsCreditor0, "NonPriorityDebtsCreditor0 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor1, nonPriorityDebtsCreditor1, "NonPriorityDebtsCreditor1 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor2, nonPriorityDebtsCreditor2, "NonPriorityDebtsCreditor2 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor3, nonPriorityDebtsCreditor3, "NonPriorityDebtsCreditor3 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor4, nonPriorityDebtsCreditor4, "NonPriorityDebtsCreditor4 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor5, nonPriorityDebtsCreditor5, "NonPriorityDebtsCreditor5 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor6, nonPriorityDebtsCreditor6, "NonPriorityDebtsCreditor6 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor7, nonPriorityDebtsCreditor7, "NonPriorityDebtsCreditor7 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor8, nonPriorityDebtsCreditor8, "NonPriorityDebtsCreditor8 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsCreditor9, nonPriorityDebtsCreditor9, "NonPriorityDebtsCreditor9 check after go back");


            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount0, nonPriorityDebtsAmount0, "NonPriorityDebtsAmount0 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount1, nonPriorityDebtsAmount1, "NonPriorityDebtsAmount1 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount2, nonPriorityDebtsAmount2, "NonPriorityDebtsAmount2 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount3, nonPriorityDebtsAmount3, "NonPriorityDebtsAmount3 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount4, nonPriorityDebtsAmount4, "NonPriorityDebtsAmount4 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount5, nonPriorityDebtsAmount5, "NonPriorityDebtsAmount5 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount6, nonPriorityDebtsAmount6, "NonPriorityDebtsAmount6 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount7, nonPriorityDebtsAmount7, "NonPriorityDebtsAmount7 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount8, nonPriorityDebtsAmount8, "NonPriorityDebtsAmount8 check after go back");
            Assert.AreEqual(fadebtspage.NonPriorityDebtsAmount9, nonPriorityDebtsAmount9, "NonPriorityDebtsAmount9 check after go back");
        }
    }
}

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
    public class FinancialAssessmentIncomeTest : UiTest
    {
        public Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            return customer;
        }

        public int GetIncomeSum(FAIncomePage faincomepage)
        {
            int sum = int.Parse(faincomepage.SalaryAfterTax.Split('.')[0]) * 100 + int.Parse(faincomepage.SalaryAfterTax.Split('.')[1]);
            sum += int.Parse(faincomepage.PartnerSalaryAfterTax.Split('.')[0]) * 100 + int.Parse(faincomepage.PartnerSalaryAfterTax.Split('.')[1]);
            sum += int.Parse(faincomepage.JobseekerAllowance.Split('.')[0]) * 100 + int.Parse(faincomepage.JobseekerAllowance.Split('.')[1]);
            sum += int.Parse(faincomepage.IncomeSupport.Split('.')[0]) * 100 + int.Parse(faincomepage.IncomeSupport.Split('.')[1]);
            sum += int.Parse(faincomepage.WorkingTaxCredit.Split('.')[0]) * 100 + int.Parse(faincomepage.WorkingTaxCredit.Split('.')[1]);
            sum += int.Parse(faincomepage.ChildTaxCredit.Split('.')[0]) * 100 + int.Parse(faincomepage.ChildTaxCredit.Split('.')[1]);
            sum += int.Parse(faincomepage.StatePension.Split('.')[0]) * 100 + int.Parse(faincomepage.StatePension.Split('.')[1]);
            sum += int.Parse(faincomepage.PrivateOrWorkPension.Split('.')[0]) * 100 + int.Parse(faincomepage.PrivateOrWorkPension.Split('.')[1]);
            sum += int.Parse(faincomepage.PensionCredit.Split('.')[0]) * 100 + int.Parse(faincomepage.PensionCredit.Split('.')[1]);
            sum += int.Parse(faincomepage.Other.Split('.')[0]) * 100 + int.Parse(faincomepage.Other.Split('.')[1]);
            sum += int.Parse(faincomepage.MaintenenceOrChildSupport.Split('.')[0]) * 100 + int.Parse(faincomepage.MaintenenceOrChildSupport.Split('.')[1]);
            sum += int.Parse(faincomepage.IncomeFromBoardersOrLodgers.Split('.')[0]) * 100 + int.Parse(faincomepage.IncomeFromBoardersOrLodgers.Split('.')[1]);
            sum += int.Parse(faincomepage.StudentLoansOrGrants.Split('.')[0]) * 100 + int.Parse(faincomepage.StudentLoansOrGrants.Split('.')[1]);
            sum += int.Parse(faincomepage.OtherIncome.Split('.')[0]) * 100 + int.Parse(faincomepage.OtherIncome.Split('.')[1]);
            return sum;
        }

        public string GetRandomMoney()
        {
            return Get.RandomInt(0, 10000).ToString() + Get.Random().ToString(".00");
        }

        public string GetRandomSpecialSymbolsString()
        {
            int lenght = Get.RandomInt(0, 10);
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
        public void EnterInvalidData(FAIncomePage faincomepage, InvalidMoneyGenerator GetData, String InvalidDataType)
        {
            faincomepage.SalaryAfterTax = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.SalaryAfterTaxErrorPresent(), "Enter " + InvalidDataType + " in SalaryAfterTax check");
            faincomepage.SalaryAfterTax = GetRandomMoney();

            faincomepage.PartnerSalaryAfterTax = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.PartnerSalaryAfterTaxErrorPresent(), "Enter " + InvalidDataType + " in PartnerSalaryAfterTax check");
            faincomepage.PartnerSalaryAfterTax = GetRandomMoney();

            faincomepage.JobseekerAllowance = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.JobseekerAllowanceErrorPresent(), "Enter " + InvalidDataType + " in JobseekerAllowance check");
            faincomepage.JobseekerAllowance = GetRandomMoney();

            faincomepage.IncomeSupport = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.IncomeSupportErrorPresent(), "Enter " + InvalidDataType + " in IncomeSupport check");
            faincomepage.IncomeSupport = GetRandomMoney();

            faincomepage.WorkingTaxCredit = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.WorkingTaxCreditErrorPresent(), "Enter " + InvalidDataType + " in WorkingTaxCredit check");
            faincomepage.WorkingTaxCredit = GetRandomMoney();

            faincomepage.ChildTaxCredit = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.ChildTaxCreditErrorPresent(), "Enter " + InvalidDataType + " in ChildTaxCredit check");
            faincomepage.ChildTaxCredit = GetRandomMoney();

            faincomepage.StatePension = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.StatePensionErrorPresent(), "Enter " + InvalidDataType + " in StatePension check");
            faincomepage.StatePension = GetRandomMoney();

            faincomepage.PrivateOrWorkPension = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.PrivateOrWorkPensionErrorPresent(), "Enter " + InvalidDataType + " in PrivateOrWorkPension check");
            faincomepage.PrivateOrWorkPension = GetRandomMoney();

            faincomepage.PensionCredit = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.PensionCreditErrorPresent(), "Enter " + InvalidDataType + " in PensionCredit check");
            faincomepage.PensionCredit = GetRandomMoney();

            faincomepage.Other = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.OtherErrorPresent(), "Enter " + InvalidDataType + " in Other check");
            faincomepage.Other = GetRandomMoney();

            faincomepage.MaintenenceOrChildSupport = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.MaintenenceOrChildSupportErrorPresent(), "Enter " + InvalidDataType + " in MaintenenceOrChildSupport check");
            faincomepage.MaintenenceOrChildSupport = GetRandomMoney();

            faincomepage.IncomeFromBoardersOrLodgers = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.IncomeFromBoardersOrLodgersErrorPresent(), "Enter " + InvalidDataType + " in IncomeFromBoardersOrLodgers check");
            faincomepage.IncomeFromBoardersOrLodgers = GetRandomMoney();

            faincomepage.StudentLoansOrGrants = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.StudentLoansOrGrantsErrorPresent(), "Enter " + InvalidDataType + " in StudentLoansOrGrants check");
            faincomepage.StudentLoansOrGrants = GetRandomMoney();

            faincomepage.OtherIncome = GetData();
            faincomepage.ClickOnTotalIncome();
            Assert.IsTrue(faincomepage.OtherIncomeErrorPresent(), "Enter " + InvalidDataType + " in OtherIncome check");
            faincomepage.OtherIncome = GetRandomMoney();
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-706"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void CheckTotalIncomefterEnterSomeValueIntoFieldsAndChangeValue()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faincomepage = journeyLn.FillAndStop().Teleport<FAIncomePage>() as FAIncomePage;

            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "First enter all value in fields");

            faincomepage.SalaryAfterTax = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change SalaryAfterTax and check TotalIncome");

            faincomepage.PartnerSalaryAfterTax = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change PartnerSalaryAfterTax and check TotalIncome");

            faincomepage.JobseekerAllowance = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change JobseekerAllowance and check TotalIncome");

            faincomepage.IncomeSupport = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change IncomeSupport and check TotalIncome");

            faincomepage.WorkingTaxCredit = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change WorkingTaxCredit and check TotalIncome");

            faincomepage.ChildTaxCredit = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change ChildTaxCredit and check TotalIncome");

            faincomepage.StatePension = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change StatePension and check TotalIncome");

            faincomepage.PrivateOrWorkPension = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change PrivateOrWorkPension and check TotalIncome");

            faincomepage.PensionCredit = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change PensionCredit and check TotalIncome");

            faincomepage.Other = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change Other and check TotalIncome");

            faincomepage.MaintenenceOrChildSupport = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change MaintenenceOrChildSupport and check TotalIncome");

            faincomepage.IncomeFromBoardersOrLodgers = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change IncomeFromBoardersOrLodgers and check TotalIncome");

            faincomepage.StudentLoansOrGrants = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change StudentLoansOrGrants and check TotalIncome");

            faincomepage.OtherIncome = GetRandomMoney();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Change OtherIncome and check TotalIncome");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-706"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void CheckTotalIncomeAndClicNextkWithoutEditFields()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);
            BasePage basepage;

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faincomepage = journeyLn.Teleport<FAIncomePage>() as FAIncomePage;

            try { basepage = faincomepage.NextClick(error: true); } catch { basepage = new FAExpenditurePage(Client); }
            Assert.Contains(faincomepage.Url, "/income", "Pass to expediture page with zero TotalIncome");
            if (faincomepage.Url.Contains("/expenditure")) { faincomepage = (basepage as FAExpenditurePage).PreviousClick() as FAIncomePage; }
            else { faincomepage = (basepage as FAIncomePage); }

            Assert.AreEqual(0, int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "TotalIncome without entering any data");

            faincomepage.TotalIncome = GetRandomMoney();
            faincomepage.ClickOnOtherIncome();
            faincomepage.ClickOnTotalIncome();
            Assert.AreEqual(0, int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Try change TotalIncome from zero");

            faincomepage.SalaryAfterTax = GetRandomMoney();
            faincomepage.TotalIncome = GetRandomMoney();
            faincomepage.ClickOnOtherIncome();
            Assert.AreEqual(int.Parse(faincomepage.SalaryAfterTax.Split('.')[0]) * 100 + int.Parse(faincomepage.SalaryAfterTax.Split('.')[1]),
                int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 + 
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Try change TotalIncome from not zero");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-706"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void NextClickWithValidDataAndCheckDataAfterGoBack()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faincomepage = journeyLn.FillAndStop().Teleport<FAIncomePage>() as FAIncomePage;

            var salaryAfterTax = faincomepage.SalaryAfterTax;
            var partnerSalaryAfterTax = faincomepage.PartnerSalaryAfterTax;
            var jobseekerAllowance = faincomepage.JobseekerAllowance;
            var incomeSupport = faincomepage.IncomeSupport;
            var workingTaxCredit = faincomepage.WorkingTaxCredit;
            var childTaxCredit = faincomepage.ChildTaxCredit;
            var statePension = faincomepage.StatePension;
            var privateOrWorkPension = faincomepage.PrivateOrWorkPension;
            var pensionCredit = faincomepage.PensionCredit;
            var other = faincomepage.Other;
            var maintenenceOrChildSupport = faincomepage.MaintenenceOrChildSupport;
            var incomeFromBoardersOrLodgers = faincomepage.IncomeFromBoardersOrLodgers;
            var studentLoansOrGrants = faincomepage.StudentLoansOrGrants;
            var otherIncome = faincomepage.OtherIncome;

            var faexpenditurepage = faincomepage.NextClick() as FAExpenditurePage;
            Assert.Contains(faincomepage.Url, "/expenditure", "Pass to expenditure page, with right data in fields");

            faincomepage = faexpenditurepage.PreviousClick() as FAIncomePage;

            Assert.AreEqual(faincomepage.SalaryAfterTax, salaryAfterTax, "SalaryAfterTax check after go back");
            Assert.AreEqual(faincomepage.PartnerSalaryAfterTax, partnerSalaryAfterTax, "PartnerSalaryAfterTax check after go back");
            Assert.AreEqual(faincomepage.JobseekerAllowance, jobseekerAllowance, "JobseekerAllowance check after go back");
            Assert.AreEqual(faincomepage.IncomeSupport, incomeSupport, "IncomeSupport check after go back");
            Assert.AreEqual(faincomepage.WorkingTaxCredit, workingTaxCredit, "WorkingTaxCredit check after go back");
            Assert.AreEqual(faincomepage.ChildTaxCredit, childTaxCredit, "ChildTaxCredit check after go back");
            Assert.AreEqual(faincomepage.StatePension, statePension, "StatePension check after go back");
            Assert.AreEqual(faincomepage.PrivateOrWorkPension, privateOrWorkPension, "PrivateOrWorkPension check after go back");
            Assert.AreEqual(faincomepage.PensionCredit, pensionCredit, "PensionCredit check after go back");
            Assert.AreEqual(faincomepage.Other, other, "Other check after go back");
            Assert.AreEqual(faincomepage.MaintenenceOrChildSupport, maintenenceOrChildSupport, "MaintenenceOrChildSupport check after go back");
            Assert.AreEqual(faincomepage.IncomeFromBoardersOrLodgers, incomeFromBoardersOrLodgers, "IncomeFromBoardersOrLodgers check after go back");
            Assert.AreEqual(faincomepage.StudentLoansOrGrants, studentLoansOrGrants, "StudentLoansOrGrants check after go back");
            Assert.AreEqual(faincomepage.OtherIncome, otherIncome, "OtherIncome check after go back");

            Assert.AreEqual(GetIncomeSum(faincomepage), int.Parse(faincomepage.TotalIncome.Split('.')[0]) * 100 +
                int.Parse(faincomepage.TotalIncome.Split('.')[1]), "Total Income check after go back");
        }

        [Test, AUT(AUT.Uk), JIRA("UKOPS-706"), MultipleAsserts, Owner(Owner.SaveliyProkopenko), Pending("Financial Assessment")]
        public void EnterInvalidDataErrorCheck()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            var faincomepage = journeyLn.FillAndStop().Teleport<FAIncomePage>() as FAIncomePage;

            EnterInvalidData(faincomepage, () => Get.GetName(), "Letters");
            EnterInvalidData(faincomepage, () => GetRandomMoney().Insert(0, "-"), "Negative Numbers");
            EnterInvalidData(faincomepage, () => GetRandomSpecialSymbolsString(), "Special symbols");
            EnterInvalidData(faincomepage, () => GetRandomMoney().Replace('.', ','), "Right Number split by coma");
            EnterInvalidData(faincomepage, () => GetRandomMoney() + Get.RandomInt(0, 10000), "Right Number with many numbers after split");
        }
    }
}

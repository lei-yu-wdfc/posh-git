using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Msmq.Messages.Ops;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.UiTests.Web;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings.Pages.FinancialAssessment;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.Ui.Pages.Helpers;
using Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;
using Wonga.QA.Framework.UI.Ui.Enums;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq.Messages.Payments;
using UI = Wonga.QA.Framework.UI.UiElements.Pages.FinancialAssessment;

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssesmentRepaymentPlanTest : UiTest
    {

        public delegate void ExecutableFunction(UI.FARepaymentPlanPage page, String fieldName, String value);
        public delegate String CustomFunction();

        private Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).WithLoanTerm(4).Build();

            return customer;
        }

        private UI.FARepaymentPlanPage RepaymentPlanPageTeleport()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            UI.FARepaymentPlanPage faRepaymentPlanPage =
                journeyLn.FillAndStop().Teleport<UI.FARepaymentPlanPage>() as UI.FARepaymentPlanPage;
            return faRepaymentPlanPage;
        }

        private List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> GetData(UI.FARepaymentPlanPage faRepaymentPlanPage, Boolean isSuccessValidation)
        {
            List<KeyValuePair<Int32, Delegate>> customDateFunctions = new List<KeyValuePair<Int32, Delegate>>();
            List<KeyValuePair<Int32, Delegate>> customSelectFunctions = new List<KeyValuePair<Int32, Delegate>>();
            List<KeyValuePair<Int32, Delegate>> customStringFunctions = new List<KeyValuePair<Int32, Delegate>>();

            if (isSuccessValidation)
            {
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.Equal, new CustomFunction(MinStartDate)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.Equal, new CustomFunction(MaxStartDate)));

                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(Weekly)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(Biweekly)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(Monthly)));

                customStringFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeString.Numbers, new CustomFunction(CorrectNumber)));
            }
            else
            {
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.Equal, new CustomFunction(CurrentDate)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.Past, new CustomFunction(PastDate)));

                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(EveryFourWeek)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(EveryTwoWeek)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(PleaseSelect)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(WeeklyFake)));

                customStringFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeString.Numbers, new CustomFunction(IncorrectNumber)));
            }

            List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields = new List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>>();
            fields.Add(new KeyValuePair<DataValidation, List<KeyValuePair<int, Delegate>>>(new DataValidation("FirstRepaymentDate", faRepaymentPlanPage.FirstRepaymentDate, FieldType.Date,
                       FieldTypeList.IncludeArray, new Int32[] { (Int32)FieldTypeDate.Past, (Int32)FieldTypeDate.Equal }.ToList()), customDateFunctions));
            /*fields.Add(new KeyValuePair<DataValidation, List<KeyValuePair<int, Delegate>>>(new DataValidation("PaymentFrequency", faRepaymentPlanPage.PaymentFrequency, FieldType.Select,
                       FieldTypeList.IncludeArray, new Int32[] { (Int32)FieldTypeSelect.Equal }.ToList()), customSelectFunctions));*/
            fields.Add(new KeyValuePair<DataValidation, List<KeyValuePair<int, Delegate>>>(new DataValidation("RepaymentAmount", faRepaymentPlanPage.RepaymentAmount, FieldType.String,
               FieldTypeList.IncludeArray, new Int32[] { (Int32)FieldTypeString.Numbers }.ToList()), customStringFunctions));
            return fields;
        }

        private void CheckValidation(UI.FARepaymentPlanPage faRepaymentPlanPage, List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields, Delegate callBack)
        {
            Dictionary<FieldType, Int32[]> dictionary = new Dictionary<FieldType, Int32[]>();
            dictionary.Add(FieldType.String, new Int32[] { (Int32)FieldTypeString.Numbers });

            foreach (var field in fields)
            {
                String oldfieldValue = (field.Key.FieldType == FieldType.Select && field.Key.FieldValue == String.Empty) ? "-- Please Select --" : field.Key.FieldValue;
                if (!field.Key.IsExtended)
                    ValidationHelper<UI.FARepaymentPlanPage>.CheckValidation(faRepaymentPlanPage, field.Key.FieldName, dictionary[field.Key.FieldType].ToList(), FieldTypeList.ExcludeArray, callBack, field.Value, field.Key.FieldType);
                else
                    ValidationHelper<UI.FARepaymentPlanPage>.CheckValidation(faRepaymentPlanPage, field.Key.FieldName, field.Key.RulesArray, field.Key.FieldTypeList, callBack, field.Value, field.Key.FieldType);
                PropertiesHelper<UI.FARepaymentPlanPage>.SetFieldValue(faRepaymentPlanPage, field.Key.FieldName, oldfieldValue);
            }
        }

        private void FailedValidationCallBack(UI.FARepaymentPlanPage faRepaymentPlanPage, String fieldName, String value)
        {
            BasePage basepage;
            PropertiesHelper<UI.FARepaymentPlanPage>.SetFieldValue(faRepaymentPlanPage, fieldName, value);
            try { basepage = faRepaymentPlanPage.NextClick(error: true); }
            catch { basepage = new UI.FARepaymentPlanPage(Client); }
            Assert.Contains(faRepaymentPlanPage.Url, "/repayment-plan", String.Format("Pass to repaymentPlan page, with invalid {0} field", fieldName));
        }

        private void SuccessValidationCallBack(UI.FARepaymentPlanPage faRepaymentPlanPage, String fieldName, String value)
        {
            BasePage basepage;
            PropertiesHelper<UI.FARepaymentPlanPage>.SetFieldValue(faRepaymentPlanPage, fieldName, value);
            try { basepage = faRepaymentPlanPage.NextClick(error: true); }
            catch { basepage = new UI.FARepaymentPlanPage(Client); }
            Assert.Contains(faRepaymentPlanPage.Url, "/wait", String.Format("Pass to wait page successfully"));
        }

        private String Weekly() { return "Weekly"; }
        private String EveryTwoWeek() { return "Every two week"; }
        private String EveryFourWeek() { return "Every four week"; }
        private String Biweekly() { return "Biweekly"; }
        private String Monthly() { return "Monthly"; }
        private String PleaseSelect() { return "--- Please Select ---"; }
        private String WeeklyFake() { return "Fake"; }
        private String MinStartDate()
        {
            ServiceConfigurationEntity minStartDays = Drive.Db.Ops.ServiceConfigurations.Where(sc => sc.Key == "Payments.RepaymentArrangementFirstRepaymentMinDays").Single();
            DateTime minStartDate = DateTime.UtcNow.AddDays(Convert.ToInt32(minStartDays.Value));
            return minStartDate.ToShortDate();
        }
        private String MaxStartDate()
        {
            ServiceConfigurationEntity maxStartDays = Drive.Db.Ops.ServiceConfigurations.Where(sc => sc.Key == "Payments.RepaymentArrangementFirstRepaymentMaxDays").Single();
            DateTime maxStartDate = DateTime.UtcNow.AddDays(Convert.ToInt32(maxStartDays));
            return maxStartDate.ToShortDate();
        }
        private String CurrentDate() { return DateTime.UtcNow.ToShortDate(); }
        private String PastDate() { return DateTime.UtcNow.AddDays(-1).ToShortDate(); }
        private String IncorrectNumber() { return Get.GetName(); }
        private String CorrectNumber() { return Get.RandomInt(0, 10000).ToString(); }

        private void SetCustomConfigurations()
        {
            String period1Interval = "25"; //25
            String period2Interval = "45"; //45
            String period1MaxMonths = "4"; //4
            String period2MaxMonths = "6"; //6
            String minStartDays = "1"; //1
            String maxStartDays = "30"; //30

            List<ServiceConfigurationEntity> serviceConfigurations = Drive.Db.Ops.ServiceConfigurations.Where(sc => sc.Key.Contains("Payments.RepaymentArrang")).ToList();
            serviceConfigurations.Single(sc => sc.Key.Contains("Payments.RepaymentArrangementPeriod1Interval")).Value = period1Interval;
            serviceConfigurations.Single(sc => sc.Key.Contains("Payments.RepaymentArrangementPeriod2Interval")).Value = period2Interval;
            serviceConfigurations.Single(sc => sc.Key.Contains("Payments.RepaymentArrangmentPeriod1MaxMonths")).Value = period1MaxMonths;
            serviceConfigurations.Single(sc => sc.Key.Contains("Payments.RepaymentArrangmentPeriod2MaxMonths")).Value = period2MaxMonths;
            serviceConfigurations.Single(sc => sc.Key.Contains("Payments.RepaymentArrangementFirstRepaymentMinDays")).Value = minStartDays;
            serviceConfigurations.Single(sc => sc.Key.Contains("Payments.RepaymentArrangementFirstRepaymentMaxDays")).Value = maxStartDays;
            foreach (ServiceConfigurationEntity serviceConfiguration in serviceConfigurations)
                serviceConfiguration.Submit();
        }

        //1
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckFailedValidation()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            Application application = customer.GetApplication();
            application.UpdateNextDueDate(-2);
            uint arrearsDays = application.GetDaysInArrears();
            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            UI.FARepaymentPlanPage faRepaymentPlanPage =
                journeyLn.FillAndStop().Teleport<UI.FARepaymentPlanPage>() as UI.FARepaymentPlanPage;
            faRepaymentPlanPage.FirstRepaymentDate = MinStartDate();
            faRepaymentPlanPage.PaymentFrequency = Weekly();

            List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields = GetData(faRepaymentPlanPage, false);
            ExecutableFunction callBack = new ExecutableFunction(FailedValidationCallBack);

            CheckValidation(faRepaymentPlanPage, fields, callBack);
        }

        //2
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckSuccessValidation()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            Application application = customer.GetApplication();
            application.UpdateNextDueDate(-2);
            uint arrearsDays = application.GetDaysInArrears();
            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            UI.FARepaymentPlanPage faRepaymentPlanPage =
                journeyLn.FillAndStop().Teleport<UI.FARepaymentPlanPage>() as UI.FARepaymentPlanPage;
            faRepaymentPlanPage.FirstRepaymentDate = MinStartDate();
            List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields = GetData(faRepaymentPlanPage, true);
            ExecutableFunction callBack = new ExecutableFunction(SuccessValidationCallBack);

            CheckValidation(faRepaymentPlanPage, fields, callBack);
        }
    }
}

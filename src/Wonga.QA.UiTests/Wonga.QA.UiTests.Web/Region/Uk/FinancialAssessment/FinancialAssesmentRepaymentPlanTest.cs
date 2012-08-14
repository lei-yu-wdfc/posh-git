using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
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

namespace Wonga.QA.Tests.Ui.FinancialAssessment
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class FinancialAssesmentRepaymentPlanTest : UiTest
    {

        public delegate void CallBackFunction(FARepaymentPlanPage page, String fieldName, String value);
        public delegate String CustomFunction();

        private Customer Init()
        {
            var email = Get.RandomEmail();
            Customer customer = CustomerBuilder.New().WithEmailAddress(email).Build();
            Application application = ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
            application.PutIntoArrears(10);
            return customer;
        }

        private FARepaymentPlanPage RepaymentPlanPageTeleport()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            loginPage.LoginAs(customer.Email);

            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            FARepaymentPlanPage faRepaymentPlanPage =
                journeyLn.FillAndStop().Teleport<FARepaymentPlanPage>() as FARepaymentPlanPage;
            return faRepaymentPlanPage;
        }

        private List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> GetData(FARepaymentPlanPage faRepaymentPlanPage, Boolean isSuccessValidation)
        {
            List<KeyValuePair<Int32, Delegate>> customDateFunctions = new List<KeyValuePair<Int32, Delegate>>();
            List<KeyValuePair<Int32, Delegate>> customSelectFunctions = new List<KeyValuePair<Int32, Delegate>>();

            if (isSuccessValidation)
            {
                /*customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.LessThan, new CustomFunction(Good)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.LessThan, new CustomFunction(Good)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.MoreThan, new CustomFunction(Good)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.MoreThan, new CustomFunction(Good)));*/

                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(Weekly)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(EveryTwoWeek)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(EveryFourWeek)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(Monthly)));
            }
            else
            {
                /*customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.LessThan, new CustomFunction(Good)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.LessThan, new CustomFunction(Good)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.MoreThan, new CustomFunction(Good)));
                customDateFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeDate.MoreThan, new CustomFunction(Good)));*/

                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(PleaseSelect)));
                customSelectFunctions.Add(new KeyValuePair<int, Delegate>((Int32)FieldTypeSelect.Equal, new CustomFunction(WeeklyFake)));
            }

            List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields = new List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>>();
            fields.Add(new KeyValuePair<DataValidation, List<KeyValuePair<int, Delegate>>>(new DataValidation("FirstRepaymentDate", faRepaymentPlanPage.FirstRepaymentDate, FieldType.Date), customDateFunctions));
            fields.Add(new KeyValuePair<DataValidation, List<KeyValuePair<int, Delegate>>>(new DataValidation("PaymentFrequency", faRepaymentPlanPage.PaymentFrequency, FieldType.Select,
                       FieldTypeList.IncludeArray, new Int32[] { (Int32)FieldTypeSelect.Equal }.ToList()), customSelectFunctions));
            fields.Add(new KeyValuePair<DataValidation, List<KeyValuePair<int, Delegate>>>(new DataValidation("RepaymentAmount", faRepaymentPlanPage.RepaymentAmount, FieldType.String), null));
            return fields;
        }

        private void CheckValidation(FARepaymentPlanPage faRepaymentPlanPage, List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields, Delegate callBack)
        {
            Dictionary<FieldType, Int32[]> dictionary = new Dictionary<FieldType, Int32[]>();
            dictionary.Add(FieldType.String, new Int32[] { (Int32)FieldTypeString.Numbers });

            foreach (var field in fields)
            {
                if (!field.Key.IsExtended)
                    ValidationHelper<FARepaymentPlanPage>.CheckValidation(faRepaymentPlanPage, field.Key.FieldName, dictionary[field.Key.FieldType].ToList(), FieldTypeList.ExcludeArray, callBack, field.Value, field.Key.FieldType);
                else
                    ValidationHelper<FARepaymentPlanPage>.CheckValidation(faRepaymentPlanPage, field.Key.FieldName, field.Key.RulesArray, field.Key.FieldTypeList, callBack, field.Value, field.Key.FieldType);
                PropertiesHelper<FARepaymentPlanPage>.SetFieldValue(faRepaymentPlanPage, field.Key.FieldName, field.Key.FieldValue);
            }
        }

        private void SuccessValidationCallBack(FARepaymentPlanPage faRepaymentPlanPage, String fieldName, String value)
        {
            //  BasePage basepage;
            PropertiesHelper<FARepaymentPlanPage>.SetFieldValue(faRepaymentPlanPage, fieldName, value);
            /*  try { basepage = faRepaymentPlanPage.NextClick(error: true); }
              catch { basepage = new FADebtsPage(Client); }
              Assert.Contains(faexpenditurepage.Url, "/expenditure", String.Format("Pass to expedenture page, with invalid {0} field", fieldName));
              if (faexpenditurepage.Url.Contains("/debts")) { faexpenditurepage = (basepage as FADebtsPage).PreviousClick() as FAExpenditurePage; }*/
        }

        private void FailedValidationCallBack(FARepaymentPlanPage faRepaymentPlanPage, String fieldName, String value)
        {
            // BasePage basepage;
            PropertiesHelper<FARepaymentPlanPage>.SetFieldValue(faRepaymentPlanPage, fieldName, value);
            /* try { basepage = faRepaymentPlanPage.NextClick(error: true); }
             catch { basepage = new FADebtsPage(Client); }
             Assert.Contains(faRepaymentPlanPage.Url, "/expenditure", String.Format("Pass to expedenture page, with invalid {0} field", fieldName));
             if (faRepaymentPlanPage.Url.Contains("/debts")) { faRepaymentPlanPage = (basepage as FADebtsPage).PreviousClick() as FAExpenditurePage; }*/
        }

        private String Weekly() { return "Weekly"; }
        private String EveryTwoWeek() { return "Every two week"; }
        private String EveryFourWeek() { return "Every four week"; }
        private String Monthly() { return "Monthly"; }
        private String PleaseSelect() { return "--- Please Select ---"; }
        private String WeeklyFake() { return "WeeklyFake"; }

        private void SetCustomConfigurations()
        {
            String period1Interval = "1"; //25
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
        public void CheckSuccessValidation()
        {
            FARepaymentPlanPage faRepaymentPlanPage = RepaymentPlanPageTeleport();
            List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields = GetData(faRepaymentPlanPage, true);
            CallBackFunction callBack = new CallBackFunction(SuccessValidationCallBack);

            CheckValidation(faRepaymentPlanPage, fields, callBack);
        }

        //2
        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void CheckFailedValidation()
        {
            FARepaymentPlanPage faRepaymentPlanPage = RepaymentPlanPageTeleport();
            List<KeyValuePair<DataValidation, List<KeyValuePair<Int32, Delegate>>>> fields = GetData(faRepaymentPlanPage, false);
            CallBackFunction callBack = new CallBackFunction(FailedValidationCallBack);

            CheckValidation(faRepaymentPlanPage, fields, callBack);
        }


        [Test, AUT(AUT.Uk), JIRA(""), MultipleAsserts, Owner(Owner.DmytroRomanii), Pending("Financial Assessment functionality is under development")]
        public void Test()
        {
            var loginPage = Client.Login();
            Customer customer = Init();
            Application custApp = customer.GetApplication();

            Boolean isUserHasArrears = custApp.IsInArrears();
            // loginPage.LoginAs(customer.Email);

            //Application custApp = customer.GetApplication();
            //custApp.PutIntoArrears(2);
            // Int32 arrearDays = custApp.GetArrearDays();
            //custApp.LoanTerm = 40;
            //DateTime? NextPayDate = (customer.GetNextPayDate() == null) ? (DateTime?)null : DateTime.Parse(customer.GetNextPayDate());
            var journeyLn = JourneyFactory.GetFaLnJourney(Client.FinancialAssessment());
            FARepaymentPlanPage faRepaymentPlanPage =
                journeyLn.FillAndStop().Teleport<FARepaymentPlanPage>() as FARepaymentPlanPage;

            SetCustomConfigurations();


        }
        // CheckNextButton
        //CheckSubmitButton
    }
}

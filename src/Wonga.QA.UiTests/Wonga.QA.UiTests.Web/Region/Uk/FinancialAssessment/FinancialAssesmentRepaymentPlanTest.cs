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
            ApplicationBuilder.New(customer).WithLoanTerm(4).Build();
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
        // var serviceConfigurations = Drive.Data.Ops.Db.ServiceConfigurations;

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
            FARepaymentPlanPage faRepaymentPlanPage = RepaymentPlanPageTeleport();
            var serviceConfigurations = Drive.Data.Ops.Db.ServiceConfigurations;
            var period1Interval = serviceConfigurations.FindByKey("Payments.RepaymentArrangementPeriod1Interval").Value.ToString();
            var period1MaxMonths = serviceConfigurations.FindByKey("Payments.RepaymentArrangmentPeriod1MaxMonths").Value.ToString();
            var period2Interval = serviceConfigurations.FindByKey("Payments.RepaymentArrangementPeriod2Interval").Value.ToString();
            var period2MaxMonths = serviceConfigurations.FindByKey("Payments.RepaymentArrangmentPeriod2MaxMonths").Value.ToString();
            var minStartDays = serviceConfigurations.FindByKey("Payments.RepaymentArrangementFirstRepaymentMinDays").Value.ToString();
            var maxStartDays = serviceConfigurations.FindByKey("Payments.RepaymentArrangementFirstRepaymentMaxDays").Value.ToString();


        }
        // CheckNextButton
        //CheckSubmitButton
    }
}

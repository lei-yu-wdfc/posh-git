using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Properties;

namespace Wonga.QA.Tests.Risk.ZScoreTest
{
    class ZScoreTest
    {
        public const string CallReportMockMode = "Mocks.CallReportEnabled";
        private string _savedCallReportMockMode;
        [SetUp]
        public void Setup()
        {
            _savedCallReportMockMode = Drive.Db.GetServiceConfiguration(CallReportMockMode).Value;
            SetCallReportTrialMode(false);
        }

        [TearDown]
        public void TearDown()
        {
            Drive.Db.SetServiceConfiguration(CallReportMockMode, _savedCallReportMockMode);
        }
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-956"), Description("score card test with CallCredit")]
        public void ScoreCardTest1000WithCallCredit()
        {
            var data = Resources.CustomerDataforCallCredit;
            var delimiters = new[] { ',', ';' };
            string[] customerdetails = data.Split(delimiters[1]);
            foreach (var customerdetail in customerdetails)
            {
                try
                {
                    string[] custdata = customerdetail.Split(delimiters[0]);
                    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                    DateTime theDateTime = DateTime.ParseExact(custdata[2], "yyyy-mm-dd", theCultureInfo);
                    var forename = custdata[0];
                    var surname = custdata[1];
                    Date dateofBirth = new Date(theDateTime, DateFormat.Date);
                    var HouseNumber = custdata[3];
                    var PostCode = custdata[4];
                    var application = CreateApplicationWithAsserts(forename, surname, dateofBirth, PostCode, HouseNumber);
                    var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
                }
                catch
                {
                }
            }
        }
        private static Application CreateApplicationWithAsserts(String forename, String surname, Date dateOfBirth, String postCode, String houseNumber)
        {
            var customer =
    CustomerBuilder.New().WithDateOfBirth(dateOfBirth).WithForename(forename).WithSurname(surname).
        WithPostcodeInAddress(postCode).WithHouseNumberInAddress(houseNumber).Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            var riskDb = Drive.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            return application;
        }
        public static void SetCallReportTrialMode(Boolean value)
        {
            var db = new DbDriver();
            var bgw = db.Ops.ServiceConfigurations.Single(bg => bg.Key == CallReportMockMode);
            var dbValue = bgw.Value;
            var wantedValue = value.ToString().ToLower();
            if (dbValue != wantedValue)
            {
                bgw.Value = wantedValue;
                db.Ops.SubmitChanges();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Properties;

namespace Wonga.QA.Tests.Risk.ZScoreTest
{
    class ZScoreTest
    {
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-956"), Description("score card test with CallCredit")]
        public void ScoreCardTest1000WithCallCredit()
        {
            var forename = "";
            var surname = "";
            var PostCode = "";
            var dateofBirth = new Date(new DateTime(1973, 5, 11), DateFormat.Date);
            var HouseNumber = "";
            string data = Resources.CustomerDataforCallCredit;
            var delimiters = new char[] { ',', ';' };
            string[] customerdetails = data.Split(delimiters[1]);
            foreach (var customerdetail in customerdetails)
            {
                string[] custdata = customerdetail.Split(delimiters[0]);
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                DateTime theDateTime = DateTime.ParseExact(custdata[2], "yyyy-mm-dd", theCultureInfo);
                forename = custdata[0];
                surname = custdata[1];
                dateofBirth = new Date(theDateTime, DateFormat.Date);
                HouseNumber = custdata[3];
                PostCode = custdata[4];
                var application = CreateApplicationWithAsserts(forename, surname, dateofBirth, PostCode, HouseNumber);
                var riskWorkflows = Application.GetWorkflowsForApplication(application.Id);
                Assert.AreEqual(riskWorkflows.Count, 1, "There should be 1 risk workflow");
            }
        }
        private static Application CreateApplicationWithAsserts(String forename, String surname, Date dateOfBirth, String postCode, String houseNumber)
        {
            var customer =
    CustomerBuilder.New().WithDateOfBirth(dateOfBirth).WithForename(forename).WithSurname(surname).
        WithPostcodeInAddress(postCode).WithHouseNumberInAddress(houseNumber).Build();
            var organization = OrganisationBuilder.New(customer).Build();
            var application = ApplicationBuilder.New(customer, organization).Build();
            Assert.IsNotNull(application);

            var riskDb = Drive.Db.Risk;
            var riskApplicationEntity = Do.Until(() => riskDb.RiskApplications.SingleOrDefault(p => p.ApplicationId == application.Id));
            Assert.IsNotNull(riskApplicationEntity, "Risk application should exist");

            var riskAccountEntity = Do.Until(() => riskDb.RiskAccounts.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(riskAccountEntity, "Risk account should exist");

            var socialDetailsEntity = Do.Until(() => riskDb.SocialDetails.SingleOrDefault(p => p.AccountId == riskApplicationEntity.AccountId));
            Assert.IsNotNull(socialDetailsEntity, "Risk Social details should exist");

            return application;
        }
    }
}

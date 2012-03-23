using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
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
            var data = Resources.CustomerDataforCallCredit;
            var delimiters = new[] { ',', ';' };
            string[] customerdetails = data.Split(delimiters[1]);
            foreach (var riskWorkflows in from customerdetail in customerdetails
                                          select customerdetail.Split(delimiters[0])
                                          into custdata let theCultureInfo = new System.Globalization.CultureInfo("en-GB", true) 
                                          let theDateTime = DateTime.ParseExact(custdata[2], "yyyy-mm-dd", theCultureInfo) 
                                          let forename = custdata[0] 
                                          let surname = custdata[1] 
                                          let dateofBirth = new Date(theDateTime, DateFormat.Date) 
                                          let HouseNumber = custdata[3] 
                                          let PostCode = custdata[4] select CreateApplicationWithAsserts(forename, surname, dateofBirth, PostCode, HouseNumber)
                                          into application select Application.GetWorkflowsForApplication(application.Id))
            {
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

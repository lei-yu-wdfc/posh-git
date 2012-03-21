using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    public class DocumentsEmailsTests
    {
        [Test,AUT(AUT.Wb), JIRA("SME-976"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)"), Explicit("Required risk event not being published yet")]
        public void RunPartialL0AndCheckForGuarantorDocumentsAndEmailGenerated()
        {
            Customer cust = CustomerBuilder.New().Build();
            var organisationBuilder = OrganisationBuilder.New(cust);
            var company = organisationBuilder.WithSoManySecondaryDirectors(2).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(cust, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();
            organisationBuilder.BuildSecondaryDirectors();
            businessApplicationBuilder.SignApplicationForSecondaryDirectors();

            DoBuilder _do = new DoBuilder(new TimeSpan(0,2,0), new TimeSpan(0,0,20));
            var emailCorrelationRecords = _do.Until(() => Drive.Db.Comms.EmailReturnLinkCorrelationWbUks.Count(p => p.OrganisationId == company.Id) == 2);

            var directors = company.GetSecondaryDirectors();
            foreach (var director in directors)
            {
                _do.Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId==director.AccountId && p.DocumentType == 9) == 2);
            }
            
            _do.Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.DocumentType == 12) == 2);
        }

        [Test, AUT(AUT.Wb), JIRA("SME-951"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)"), Explicit("Required risk event not being published yet")]
        public void RunPartialL0AndCheckForPrimaryDirectorDocumentsAndEmailGenerated()
        {
            Customer cust = CustomerBuilder.New().Build();
            var organisationBuilder = OrganisationBuilder.New(cust);
            var company = organisationBuilder.WithSoManySecondaryDirectors(3).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(cust, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();
            organisationBuilder.BuildSecondaryDirectors();
            businessApplicationBuilder.SignApplicationForSecondaryDirectors();

            DoBuilder _do = new DoBuilder(new TimeSpan(0, 2, 0), new TimeSpan(0, 0, 20));            
            //_do.Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.DocumentType == 11) ==1);
            _do.Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId== cust.Id && p.DocumentType == 9) == 2);
            _do.Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == cust.Id && p.DocumentType == 10) == 2);
            //Assert.IsTrue(Drive.ThirdParties.ExactTarget.CheckSMEInitialPrimaryDirectorEmailSent(cust.GetEmail()), "Email should have been sent");
        }
    }
}

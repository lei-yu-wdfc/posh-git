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
        protected const int NumberOfSecondaryDirectors=2;

        [Test,AUT(AUT.Wb), JIRA("SME-976"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForGuarantorUnsignedDocumentsAndEmailGenerated()
        {
            Customer cust = CustomerBuilder.New().Build();
            var organisationBuilder = OrganisationBuilder.New(cust);
            var company = organisationBuilder.WithSoManySecondaryDirectors(NumberOfSecondaryDirectors).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(cust, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();
            organisationBuilder.BuildSecondaryDirectors();

            var emailCorrelationRecords = Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.EmailReturnLinkCorrelationWbUks.Count(p => p.OrganisationId == company.Id) == NumberOfSecondaryDirectors);

            var directors = company.GetSecondaryDirectors();
            foreach (var director in directors)
            {
                Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 9) == 1);
                Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 12) == 1);
            }
        }

        [Test, AUT(AUT.Wb), JIRA("SME-951"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForPrimaryUnsignedDirectorDocumentsAndEmailGenerated()
        {
            Customer cust = CustomerBuilder.New().Build();
            var organisationBuilder = OrganisationBuilder.New(cust);
            var company = organisationBuilder.WithSoManySecondaryDirectors(NumberOfSecondaryDirectors).Build();
            
            var businessApplicationBuilder = ApplicationBuilder.New(cust, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();            
            organisationBuilder.BuildSecondaryDirectors();
            businessApplicationBuilder.SignApplicationForSecondaryDirectors();
                        
            Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == cust.Id && p.DocumentType == 9) == 1);
            Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == cust.Id && p.DocumentType == 10) == 1);
            Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == cust.Id && p.DocumentType == 11) == 1);            
        }

        [Test, AUT(AUT.Wb), JIRA("SME-209"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForPrimaryDirectorFundsAdvancedDocumentsAndEmail()
        {
            Customer cust = CustomerBuilder.New().Build();
            var organisationBuilder = OrganisationBuilder.New(cust);
            var company = organisationBuilder.WithSoManySecondaryDirectors(NumberOfSecondaryDirectors).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(cust, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();
            organisationBuilder.BuildSecondaryDirectors();
            businessApplicationBuilder.SignApplicationForSecondaryDirectors();

            var directors = company.GetSecondaryDirectors();
            foreach (var director in directors)
            {
                Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 14) == 1);
                Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 15) == 1);
            }
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1043"), Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForGuarantorsFundsAdvancedDocumentsAndEmail()
        {
            Customer cust = CustomerBuilder.New().Build();
            var organisationBuilder = OrganisationBuilder.New(cust);
            var company = organisationBuilder.WithSoManySecondaryDirectors(NumberOfSecondaryDirectors).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(cust, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.Build();
            organisationBuilder.BuildSecondaryDirectors();
            businessApplicationBuilder.SignApplicationForSecondaryDirectors();

            var directors = company.GetSecondaryDirectors();
            foreach (var director in directors)
            {
                Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 14) == 1);
                Do.With().Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 15) == 1);
            }
        }
      
    }
}

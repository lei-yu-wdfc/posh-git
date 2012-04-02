using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [TestFixture]
    public class DocumentsEmailsTests
    {
        protected const int NumberOfSecondaryDirectors=2;

        [Test,AUT(AUT.Wb), JIRA("SME-976")]
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForGuarantorUnsignedDocumentsAndEmailGenerated()
        {
            var mainApplicant = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NumberOfSecondaryDirectors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }

            var company = OrganisationBuilder.New(mainApplicant).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(mainApplicant, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.WithGuarantors(listOfGuarantors).WithUnsignedGuarantors().Build();

            var emailCorrelationRecords = Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.EmailReturnLinkCorrelationWbUks.Count(p => p.OrganisationId == company.Id) == NumberOfSecondaryDirectors);

            var directors = company.GetSecondaryDirectors();
            foreach (var director in directors)
            {
                Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 9) == 1);
                Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 12) == 1);
            }
        }

        [Test, AUT(AUT.Wb), JIRA("SME-951")]
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForPrimaryUnsignedDirectorDocumentsAndEmailGenerated()
        {
            //Vaklav can you please double check what exactly needs to be done here?
            var mainDirector = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NumberOfSecondaryDirectors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }

            var company = OrganisationBuilder.New(mainDirector).Build();
            
            var businessApplicationBuilder = ApplicationBuilder.New(mainDirector, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.WithGuarantors(listOfGuarantors).Build();            
                        
            Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == mainDirector.Id && p.DocumentType == 9) == 1);
            Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == mainDirector.Id && p.DocumentType == 10) == 1);
            Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == mainDirector.Id && p.DocumentType == 11) == 1);            
        }

        [Test, AUT(AUT.Wb), JIRA("SME-209")]
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForPrimaryDirectorFundsAdvancedDocumentsAndEmail()
        {
            //This too -> Looking at the code you want to build and not sign for secondary 
            var mainDirector = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NumberOfSecondaryDirectors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }

            var company = OrganisationBuilder.New(mainDirector).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(mainDirector, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.WithGuarantors(listOfGuarantors).WithUnsignedGuarantors().Build();
            //SignBusinessApplicationWbUkCommand -> It is in the build()
            //Drive.Api.Commands.Post(new SignBusinessApplicationWbUkCommand { AccountId = mainDirector.Id, ApplicationId = application.Id });
            //organisationBuilder.BuildSecondaryDirectors();
            //businessApplicationBuilder.SignApplicationForSecondaryDirectors();

            Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == mainDirector.Id && p.DocumentType == 15) == 1);
            Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == mainDirector.Id && p.DocumentType == 16) == 1);            
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1043")] 
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForGuarantorsFundsAdvancedDocumentsAndEmail()
        {
            var mainDirector = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NumberOfSecondaryDirectors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }

            var company = OrganisationBuilder.New(mainDirector).Build();
            //var company = organisationBuilder.WithSoManySecondaryDirectors(NumberOfSecondaryDirectors).Build();

            var businessApplicationBuilder = ApplicationBuilder.New(mainDirector, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.WithGuarantors(listOfGuarantors).Build();
            //Drive.Api.Commands.Post(new SignBusinessApplicationWbUkCommand { AccountId = mainDirector.Id, ApplicationId = application.Id });
            //organisationBuilder.BuildSecondaryDirectors();
            //businessApplicationBuilder.SignApplicationForSecondaryDirectors();

            var directors = company.GetSecondaryDirectors();
            foreach (var director in directors)
            {
                Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 14) == 1);
                Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 15) == 1);
            }
        }

        [Test, AUT(AUT.Wb), JIRA("SME-232")]
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialDeclineL0AndCheckForDeclineEmail()
        {
            //main applicant will fail because the riskMaks is disabled
            var mainApplicant = CustomerBuilder.New().WithMiddleName("hahaha").Build();
            var guarantorList = new List<CustomerBuilder>
                                    {
                                        CustomerBuilder.New(),
                                        CustomerBuilder.New()
                                    };


            var company = OrganisationBuilder.New(mainApplicant).Build();
            var businessApplicationBuilder = ApplicationBuilder.New(mainApplicant, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.WithGuarantors(guarantorList).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            
            var directors = company.GetSecondaryDirectors();
            Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == mainApplicant.Id && p.DocumentType == 17) == 1);
            foreach (var director in directors)
            {
                Do.With.Timeout(2).Interval(20).Until(() => Drive.Db.Comms.LegalDocuments.Count(p => p.ApplicationId == application.Id && p.AccountId == director.AccountId && p.DocumentType == 17) == 1);                
            }
        }
    }
}

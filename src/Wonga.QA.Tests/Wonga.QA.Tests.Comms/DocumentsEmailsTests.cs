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
    [Parallelizable(TestScope.All)]
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

            var emailRetLinkCorreclationWbUksTab = Drive.Data.Comms.Db.EmailReturnLinkCorrelationWbUks;
            var emailCorrelationRecords = Do.With.Timeout(2).Interval(20).Until(() => emailRetLinkCorreclationWbUksTab.FindAll(emailRetLinkCorreclationWbUksTab.OrganisationId == company.Id).Count() == NumberOfSecondaryDirectors);

            var directors = company.GetSecondaryDirectors();
            var legalDocTab = Drive.Data.Comms.Db.LegalDocuments;
            foreach (var director in directors)
            {
                Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == director.AccountId && legalDocTab.DocumentType == 9).Count() == 1);
                Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == director.AccountId && legalDocTab.DocumentType == 12).Count() == 1);
            }
        }

        [Test, AUT(AUT.Wb), JIRA("SME-951")]
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForPrimaryUnsignedDirectorDocumentsAndEmailGenerated()
        {
            var mainDirector = CustomerBuilder.New().Build();
            var listOfGuarantors = new List<CustomerBuilder>();
            for (var i = 0; i < NumberOfSecondaryDirectors; i++)
            {
                listOfGuarantors.Add(CustomerBuilder.New());
            }

            var company = OrganisationBuilder.New(mainDirector).Build();
            
            var businessApplicationBuilder = ApplicationBuilder.New(mainDirector, company) as BusinessApplicationBuilder;
            var application = businessApplicationBuilder.WithGuarantors(listOfGuarantors).Build();

            var legalDocTab = Drive.Data.Comms.Db.LegalDocuments;
            Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == mainDirector.Id && legalDocTab.DocumentType == 9).Count() == 1);
            Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == mainDirector.Id && legalDocTab.DocumentType == 10).Count() == 1);
            Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == mainDirector.Id && legalDocTab.DocumentType == 11).Count() == 1);            
        }

        [Test, AUT(AUT.Wb), JIRA("SME-209")]
        [Description("This test verifies documents being generated as part of L0 process, which is a key prerequisite for emails to be sent (this last step involves 3rd party)")]
        public void RunPartialL0AndCheckForPrimaryDirectorFundsAdvancedDocumentsAndEmail()
        {
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

            var legalDocTab = Drive.Data.Comms.Db.LegalDocuments;
            Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == mainDirector.Id && legalDocTab.DocumentType == 15).Count() == 1);
            Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == mainDirector.Id && legalDocTab.DocumentType == 16).Count() == 1);            
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
            var legalDocTab = Drive.Data.Comms.Db.LegalDocuments;
            foreach (var director in directors)
            {
                Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == director.AccountId && legalDocTab.DocumentType == 14).Count() == 1);
                Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == director.AccountId && legalDocTab.DocumentType == 15).Count() == 1);
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
            var legalDocTab = Drive.Data.Comms.Db.LegalDocuments;
            Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == mainApplicant.Id && legalDocTab.DocumentType == 17).Count() == 1);
            foreach (var director in directors)
            {
                Do.With.Timeout(2).Interval(20).Until(() => legalDocTab.FindAll(legalDocTab.ApplicationId == application.Id && legalDocTab.AccountId == director.AccountId && legalDocTab.DocumentType == 17).Count() == 1);                
            }
        }
    }
}

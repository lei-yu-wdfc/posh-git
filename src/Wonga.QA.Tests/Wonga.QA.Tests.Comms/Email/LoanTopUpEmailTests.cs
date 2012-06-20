using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using CreateFixedTermLoanTopupCommand = Wonga.QA.Framework.Api.CreateFixedTermLoanTopupCommand;

namespace Wonga.QA.Tests.Comms.Email
{

    [TestFixture]
    public class LoanTopUpEmailTests
    {

        [Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
        public void CreateFixedTermLoanTopUpSecciTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            Application application = ApplicationBuilder.New(customer).Build();

            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = 150
            });

            var appTab = Drive.Data.Payments.Db.Applications;
            var app = appTab.FindAll(appTab.ExternalId == application.Id).Single();
            var topupsTab = Drive.Data.Payments.Db.Topups;
            Assert.IsNotNull(Do.Until(() => topupsTab.FindAll(topupsTab.ApplicationId == app.ApplicationId).Single()));

            var legalDocsTab = Drive.Data.Comms.Db.LegalDocuments;
            var legalDocumentEntity = Do.Until(() => legalDocsTab.FindAll(legalDocsTab.ApplicationId == application.Id &&legalDocsTab.AccountId == customer.Id && legalDocsTab.DocumentType == 4).Single()); //SECCI Document
            var filesTab = Drive.Data.FileStorage.Db.Files;
            Do.Until(() => filesTab.FindAll(filesTab.FileId == legalDocumentEntity.ExternalId).Single());
        }



        [Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
        public void CreateFixedTermLoanTopUpAgreementTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            Application application = ApplicationBuilder.New(customer).Build();

            var fixedTermLoanTopupId = Guid.NewGuid();
            Drive.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = fixedTermLoanTopupId,
                TopupAmount = 150
            });
            var appTab = Drive.Data.Payments.Db.Applications;
            var app = appTab.FindAll(appTab.ExternalId == application.Id).Single();
            var topupsTab = Drive.Data.Payments.Db.Topups;
            Assert.IsNotNull(Do.Until(() => topupsTab.FindAll(topupsTab.ApplicationId == app.ApplicationId).Single()));
            var legDocsTab = Drive.Data.Comms.Db.LegalDocuments;
            Do.Until(() => legDocsTab.FindAll(legDocsTab.ApplicationId == application.Id && legDocsTab.AccountId == customer.Id && legDocsTab.DocumentType == 5).Single()); //Agreement document

            var emailTopupAgreementTab = Drive.Data.OpsSagas.Db.EmailTopupAgreementEntity;
            Do.Until(() => emailTopupAgreementTab.FindAll(emailTopupAgreementTab.TopUpId == fixedTermLoanTopupId).Single()); //Email TopUp Agreement Saga in progress

            Drive.Msmq.Comms.Send(new ILoanToppedUpEvent
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                CreatedOn = DateTime.UtcNow,
                TopupId = fixedTermLoanTopupId,
            }
                );
            Do.Until(() => !emailTopupAgreementTab.FindAll(emailTopupAgreementTab.TopUpId == fixedTermLoanTopupId).Any()); //Email TopUp Agreement Saga completed
        }
    }
}

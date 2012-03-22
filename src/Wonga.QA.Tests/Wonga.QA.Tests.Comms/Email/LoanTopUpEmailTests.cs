using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
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

            Driver.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = Guid.NewGuid(),
                TopupAmount = 150
            });

            var app = Driver.Db.Payments.Applications.Single(x => x.ExternalId == application.Id);
            Assert.IsNotNull(Do.Until(() => Driver.Db.Payments.Topups.Single(x => x.ApplicationId == app.ApplicationId)));

            var legalDocumentEntity = Do.Until(() => Driver.Db.Comms.LegalDocuments.Single(p => p.ApplicationId == application.Id && p.AccountId == customer.Id && p.DocumentType == 4)); //SECCI Document
            Do.Until(() => Driver.Db.FileStorage.Files.Single(f => f.FileId == legalDocumentEntity.ExternalId));
        }



        [Test, AUT(AUT.Uk), JIRA("UK-789"), Parallelizable]
        public void CreateFixedTermLoanTopUpAgreementTest()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            Application application = ApplicationBuilder.New(customer).Build();

            var fixedTermLoanTopupId = Guid.NewGuid();
            Driver.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                FixedTermLoanTopupId = fixedTermLoanTopupId,
                TopupAmount = 150
            });

            var app = Driver.Db.Payments.Applications.Single(x => x.ExternalId == application.Id);
            Assert.IsNotNull(Do.Until(() => Driver.Db.Payments.Topups.Single(x => x.ApplicationId == app.ApplicationId)));
            Do.Until(() => Driver.Db.Comms.LegalDocuments.Single(p => p.ApplicationId == application.Id && p.AccountId == customer.Id && p.DocumentType == 5)); //Agreement document

            Do.Until(() => Driver.Db.OpsSagas.EmailTopUpAgreementEntities.Single(x => x.TopUpId == fixedTermLoanTopupId)); //Email TopUp Agreement Saga in progress

            Driver.Msmq.Comms.Send(new ILoanToppedUpEvent
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                CreatedOn = DateTime.UtcNow,
                TopupId = fixedTermLoanTopupId,
            }
                );
            Do.Until(() => !Driver.Db.OpsSagas.EmailTopUpAgreementEntities.Any(x => x.TopUpId == fixedTermLoanTopupId)); //Email TopUp Agreement Saga completed
        }
    }
}

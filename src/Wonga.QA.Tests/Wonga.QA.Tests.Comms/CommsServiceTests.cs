using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [Parallelizable(TestScope.All)]
    public class CommsServiceTests
    {
        [Test]
        public void CommsServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Comms.IsRunning());
        }

        [Test, AUT(AUT.Uk)]
        public void LoanExtensionSecciEmailIsSent()
        {
            Customer cust = CustomerBuilder.New().Build();
            Do.Until(cust.GetBankAccount);
            Do.Until(cust.GetPaymentCard);
            Application app = ApplicationBuilder.New(cust)
                .WithPromiseDate(new Date(DateTime.Now.AddDays(6), DateFormat.Date))
                .WithLoanAmount(100)
                .Build();
            var ftApp = Driver.Db.Payments.FixedTermLoanApplications.Single(a => a.ApplicationEntity.ExternalId == app.Id);
            Assert.IsTrue(Driver.Svc.DocumentGeneration.IsRunning());
            Assert.IsTrue(Driver.Svc.Payments.IsRunning());
            Driver.Msmq.Payments.Send(new ExtendLoanStartedInternalCommand
                                        {
                                            AccountId = cust.Id, 
                                            ApplicationId = app.Id, 
                                            PartPaymentRequired = 10m,
                                            ExtendDate = ftApp.NextDueDate ?? ftApp.PromiseDate,
                                            ExtensionId = Get.GetId(),
                                            NewFinalBalance = ftApp.LoanAmount
                                        });

            new DoBuilder(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1))
                                .Until(() => Driver.Db.Comms.LegalDocuments.Single(ld => ld.ApplicationId == app.Id && ld.DocumentType == 2));//ExtensionSeccii

            new DoBuilder(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1))
                                .Until(() => Driver.Db.Comms.LegalDocuments.Single(ld => ld.ApplicationId == app.Id && ld.DocumentType == 3));//Pre Agreement


        }


        [Test, 
            AUT(AUT.Uk), 
            JIRA("UK-598"), 
            Description("Check that emails are received, when extension is not completed."), 
            Ignore("Not implemented in version 3.16.0")]
        public void LoanExtensionNotCompleteEmailSent()
        {

        }


    }
}

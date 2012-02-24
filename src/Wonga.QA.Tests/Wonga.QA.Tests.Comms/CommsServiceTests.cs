using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [Parallelizable(TestScope.All)]
    public class CommsServiceTests
    {
        [Test, AUT]
        public void CommsServiceIsRunning()
        {
            Assert.IsTrue(Driver.Svc.Comms.IsRunning());
        }

		[Test, AUT(AUT.Uk)]
		public void LoanExtensionSecciEmail()
		{
			Customer cust = CustomerBuilder.New().Build();
			Application app = ApplicationBuilder.New(cust)
				.WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted)
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
											ExtensionId = Data.GetId(),
											NewFinalBalance = ftApp.LoanAmount
			                           	});

			var secciDocument = Do.Until(() => Driver.Db.Comms.LegalDocuments.Single(ld => ld.ApplicationId == app.Id && ld.DocumentType == 2));//ExtensionSeccii
			
		}

    }
}

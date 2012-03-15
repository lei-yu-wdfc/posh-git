using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
	[Parallelizable(TestScope.All), AUT(AUT.Za)]
	class CollectionsChaseTests
	{

		[Test, Explicit("Release")]
		public void CollectionsChaseEmails()
		{
			var customer = CustomerBuilder.New().Build();
			var application = ApplicationBuilder.New(customer).Build();

			Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).FixedTermLoanApplicationEntity.NextDueDate = DateTime.UtcNow;
			Driver.Db.Payments.SubmitChanges();

			var nextDueDate =
				Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).FixedTermLoanApplicationEntity.NextDueDate;

			Driver.Msmq.Comms.Send(new IInArrearsAddedEvent{AccountId = customer.Id, ApplicationId = application.Id});

			Do.Until(() => Driver.Db.OpsSagas.CollectionsChaseSagaEntities.Single(a => a.ApplicationId == application.Id));
		}


	}
}

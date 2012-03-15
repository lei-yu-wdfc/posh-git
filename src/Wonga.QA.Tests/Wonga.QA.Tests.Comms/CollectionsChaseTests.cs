using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
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

			PutApplicationInArrears(application);

			Do.Until(() => Driver.Db.OpsSagas.CollectionsChaseSagaEntities.Single(a => a.ApplicationId == application.Id));
		}

		private void PutApplicationInArrears(Application application)
		{
			var fixedTermLoanApplication = Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id).FixedTermLoanApplicationEntity;
			Driver.Db.SetServiceConfiguration("Payments.ProcessScheduledPaymentSaga.DateTime.UtcNow", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

			Driver.Msmq.Payments.Send(new ProcessScheduledPaymentCommand { ApplicationId = fixedTermLoanApplication.ApplicationId });
			Do.Until(() => Driver.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(a => a.ApplicationGuid == application.Id));
			Do.Until(() => Driver.Db.Payments.Arrears.Single(a => a.ApplicationId == fixedTermLoanApplication.ApplicationId));
		}

	}
}

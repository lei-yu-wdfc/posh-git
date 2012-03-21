using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[AUT(AUT.Za)]
	public class NaedoRetryTests
	{
		private const string BankGatewayIsTestModeKey = "BankGateway.IsTestMode";
		private string _bankGatewayIsTestMode;

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ServiceConfigurationEntity entity =
				Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == BankGatewayIsTestModeKey);
			entity.Value = _bankGatewayIsTestMode;
			entity.Submit();
		}

		[Test, JIRA("ZA-1969"), Parallelizable, Pending("Work in progress")]
		public void SecondNaedoIsPostedWhenFirstSucceededButAmountIsOutstandingInArrears()
		{
			const decimal amount = 1000m;

			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).WithLoanAmount(amount).Build();
			PaymentsDatabase paymentsDatabase = Drive.Db.Payments;
			MsmqQueue paymentsQueue = Drive.Msmq.Payments;
			var applicationEntity = paymentsDatabase.Applications.Single(a => a.ExternalId == application.Id);

			TimeSpan span = applicationEntity.FixedTermLoanApplicationEntity.NextDueDate.Value - DateTime.Today;
			applicationEntity.FixedTermLoanApplicationEntity.PromiseDate -= span;
			applicationEntity.FixedTermLoanApplicationEntity.NextDueDate -= span;
			applicationEntity.Submit();

			var scheduledsaga =
				Drive.Db.OpsSagas.FixedTermLoanSagaEntities
					.Single(e => e.ApplicationGuid == applicationEntity.ExternalId);

			paymentsQueue.Send(new TimeoutMessage {Expires = DateTime.UtcNow, SagaId = scheduledsaga.Id});

			var processSaga =
				Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(s => s.ApplicationId == applicationEntity.ApplicationId));

			paymentsQueue.Send(new TimeoutMessage {Expires = DateTime.UtcNow, SagaId = processSaga.Id});

			Do.Until(() => paymentsDatabase.Arrears.Single(s => s.ApplicationId == applicationEntity.ApplicationId));
			SetBankGatewayTestMode();

			paymentsQueue.Send(new ProcessScheduledPaymentCommand
			                   	{
			                   		ApplicationId = applicationEntity.ApplicationId,
			                   		CollectAmount = amount/2,
			                   		CollectDate = DateTime.Now,
			                   		IsRetry = false,
			                   		TrackingDays = 7
			                   	});

			Do.Until(() =>
			         paymentsDatabase.Transactions
			         	.Count(t => t.ApplicationId == applicationEntity.ApplicationId &&
			         	            t.Type == "DirectBankPayment") >= 2);

			var ddTransactions = paymentsDatabase.Transactions
				.Where(t => t.ApplicationId == applicationEntity.ApplicationId &&
				            t.Type == "DirectBankPayment")
				.OrderBy(t => t.CreatedOn)
				.ToList();

			Assert.AreEqual(-amount/2, ddTransactions[0].Amount);
			Assert.GreaterThan(-amount/2, ddTransactions[1].Amount);
		}

		private void SetBankGatewayTestMode()
		{
			ServiceConfigurationEntity entity =
				Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == BankGatewayIsTestModeKey);
			_bankGatewayIsTestMode = entity.Value;
			entity.Value = "true";
			entity.Submit();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
	[AUT(AUT.Za), Pending("Work in progress")]
	public class NaedoRetryTests
	{
		private bool _bankGatewayTestModeOriginal;

		[FixtureSetUp]
		public void FixtureSetup()
		{
			_bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
		}

		//TODO: Complete this
		[Test, JIRA("ZA-1969"), Pending, Parallelizable]
		public void SecondNaedoIsPostedWhenFirstSucceededButAmountIsOutstandingInArrears()
		{
			const decimal amount = 1000m;

			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).WithLoanAmount(amount).Build();
			PaymentsDatabase paymentsDatabase = Drive.Db.Payments;
			MsmqQueue paymentsQueue = Drive.Msmq.Payments;
			var applicationEntity = paymentsDatabase.Applications.Single(a => a.ExternalId == application.Id);

			application.PutIntoArrears(20);

			Do.Until(() => paymentsDatabase.Arrears.Single(s => s.ApplicationId == applicationEntity.ApplicationId));
			ConfigurationFunctions.SetBankGatewayTestMode(true);

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
	}
}

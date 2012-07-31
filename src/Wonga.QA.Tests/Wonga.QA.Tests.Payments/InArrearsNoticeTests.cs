using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, AUT(AUT.Za, AUT.Uk, AUT.Ca), Parallelizable(TestScope.Descendants)]
	public class InArrearsNoticeTests
	{
		private bool _bankGatewayTestModeOriginal;
		private Application _application;

		[FixtureSetUp]
		public void FixtureSetup()
		{
			_bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
			ConfigurationFunctions.SetBankGatewayTestMode(false);

			_application = ArrangeApplication();
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
		}

		[Test, JIRA("ZA-1676"), Category(TestCategories.CoreTest), Owner(Owner.ImreKoszo)]
		public void ApplicationInArrearsSagaCreatedTest()
		{
			//check if saga created.
			var saga =
				Do.Until(() => Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Single(e => e.AccountId == _application.AccountId));
			Assert.AreEqual(0, saga.DaysInArrears);
		}

        [Test, JIRA("ZA-1676"), DependsOn("ApplicationInArrearsSagaCreatedTest"), Category(TestCategories.CoreTest), Owner(Owner.ImreKoszo)]
		public void ApplicationClosedSagaCompleteTest()
		{
			//staff message for application is closed.
			Drive.Msmq.Payments.Send(new IApplicationClosed
			                         	{
			                         		ApplicationId = _application.Id,
			                         		AccountId = _application.AccountId,
			                         		ClosedOn = DateTime.UtcNow,
			                         		CreatedOn = DateTime.UtcNow
			                         	});
			Do.Until(() => !Drive.Db.OpsSagas.InArrearsNoticeSagaEntities.Any(e => e.AccountId == _application.AccountId));
		}

		private static Application ArrangeApplication()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			if (Config.AUT == AUT.Uk)
			{
				return ApplicationBuilder.New(customer).Build()
					.ExpireCard()
					.PutIntoArrears(20);
			}
			else
			{
				return ApplicationBuilder.New(customer).Build()
					.PutIntoArrears(20);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Comms.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
	[TestFixture, Parallelizable(TestScope.Descendants)] //Can be only on level 3 because it changes configuration
	class CollectionsChaseEmailTest
	{
		private bool _bankGatewayTestModeOriginal;
		private static readonly dynamic EmailTable = Drive.Data.QaData.Db.Email;

		private Application _application;

		private const int TemplateA2 = 22843;
		private const int TemplateA3 = 22879;
		private const int TemplateA4 = 22880;
		private const int TemplateA5 = 22881;
		private const int TemplateA6 = 22882;
		private const int TemplateA7 = 22883;
		private const int TemplateA8 = 22884;

		[FixtureSetUp]
		public void FixtureSetup()
		{
			_bankGatewayTestModeOriginal = ConfigurationFunctions.GetBankGatewayTestMode();
			ConfigurationFunctions.SetBankGatewayTestMode(false);

			var customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).Build();
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ConfigurationFunctions.SetBankGatewayTestMode(_bankGatewayTestModeOriginal);
		}

		[Test, JIRA("QA-206")]
		public void A2EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(0, TemplateA2);
		}

		[Test, JIRA("QA-206"), DependsOn("A2EmailIsSent")]
		public void A3EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(3, TemplateA3);
		}

		[Test, JIRA("QA-206"), DependsOn("A3EmailIsSent")]
		public void A4EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(15, TemplateA4);
		}

		[Test, JIRA("QA-206"), DependsOn("A4EmailIsSent")]
		public void A5EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(20, TemplateA5);
		}

		[Test, JIRA("QA-206"), DependsOn("A5EmailIsSent")]
		public void A6EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(30, TemplateA6);
		}

		[Test, JIRA("QA-206"), DependsOn("A6EmailIsSent")]
		public void A7EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(40, TemplateA7);
		}

		[Test, JIRA("QA-206"), DependsOn("A7EmailIsSent")]
		public void A8EmailIsSent()
		{
			VerifyEmailIsSentAfterDaysInArrears(45, TemplateA8);
		}

		#region Helpers

		private void VerifyEmailIsSentAfterDaysInArrears(uint daysInArrears, int template)
		{
			_application.PutApplicationIntoArrears(daysInArrears);

			if( daysInArrears > 0) //Saga is created after first email sent
				TimeoutCollectionsChaseSagaForDays(_application, daysInArrears);

			AssertEmailIsSent(_application.GetCustomer().Email, template);
		}

		private void TimeoutCollectionsChaseSagaForDays(Application application, uint days)
		{
			var sagaId = (Guid) Do.Until(() => (Drive.Data.OpsSagas.Db.CollectionsChaseSagaEntity.FindByApplicationId(application.Id))).Id;
			
			Drive.Data.OpsSagas.Db.CollectionsChaseSagaEntity.UpdateById(Id: sagaId, DueDate: DateTime.Today.AddDays(-days));
			Drive.Msmq.Comms.Send(new TimeoutMessage{SagaId = sagaId});
		}

		private void AssertEmailIsSent(string email, int template)
		{
			Assert.IsNotNull(
				Do.Until(() =>
						 EmailTable.Find(
							EmailTable.EmailAddress == email &&
							EmailTable.TemplateName == template)));
		}

		#endregion
	}
}

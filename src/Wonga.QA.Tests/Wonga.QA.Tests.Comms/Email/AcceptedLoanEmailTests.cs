using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
	[TestFixture, Parallelizable(TestScope.All), Category(TestCategories.CoreTest)]
	public class AcceptedLoanEmailTests
	{
		private Application _application;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			if(Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
				Assert.Inconclusive("Bankgateway is in test mode");
		}

		[Test, AUT(AUT.Za), JIRA("QA-204"), Pending]
		public void AgreementEmailSentAfterApplicationAccepted()
		{
			Customer customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).Build();

			var email = Do.Until(() => Drive.Data.QaData.Db.Email.FindByEmailAddress(customer.Email));

			Assert.AreEqual("18432", email.TemplateName );
		}

		[Test, AUT(AUT.Za), JIRA("QA-204"), DependsOn("AgreementEmailSentAfterApplicationAccepted"), Pending]
		public void PaymentConfirmationEmailSentAfterCustomerIsFunded()
		{
			//Need to timeout saga manually
			var sagaId = Drive.Data.OpsSagas.Db.HyphenBatchCashOutEntity.FindAll().Last();
		}
	}
}

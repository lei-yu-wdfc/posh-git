using System.Diagnostics;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
	[TestFixture, Parallelizable(TestScope.All), AUT(AUT.Za, AUT.Uk)]
	public class AcceptedLoanEmailTests
	{
		private Application _application;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			if (Config.AUT != AUT.Za) return;

			//Why does ZA require BG be enabled to test this??
			if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
				Assert.Inconclusive("Bankgateway is in test mode");
		}

		[Test, AUT(AUT.Za, AUT.Uk), JIRA("QA-204"), Owner(Owner.MichaelDoyle), Explicit]
		[AUTRow(AUT.Za, "18432")]
		[AUTRow(AUT.Uk, "9951")]
		public void AgreementEmailSentAfterApplicationAccepted(string expectedEmailTemplateName)
		{
			Debug.Print(expectedEmailTemplateName);
			Customer customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).Build();


			Assert.DoesNotThrow(
				Do.Until(
					() => Drive.Data.QaData.Db.Email.FindBy(EmailAddress: customer.Email, TemplateName: expectedEmailTemplateName)),
				"No e-mail with TemplateName:{0} and Email-address:{1} in db table QAData.Email.", customer.Email, expectedEmailTemplateName);

			/*	If the test passes, but emails are still not being received - check the exact target admin system;
				Sometimes the "interaction" gests stuck.
				Logon and goto Interactions/Messages/Email/Trigggered - locate the interaction for the number above (hint: sorting by Modified-Date helps)
				Check the number of queued and if it's >0, pause and restart the interaction.
				*/
		}


		[Test, AUT(AUT.Za), JIRA("QA-204"), DependsOn("AgreementEmailSentAfterApplicationAccepted"), Pending]
		public void PaymentConfirmationEmailSentAfterCustomerIsFunded()
		{
			//Need to timeout saga manually
			var sagaId = Drive.Data.OpsSagas.Db.HyphenBatchCashOutEntity.FindAll().Last();
		}
	}
}

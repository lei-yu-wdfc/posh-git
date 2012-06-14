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
	[TestFixture, Parallelizable(TestScope.Descendants)]
	public class AcceptedLoanEmailTests
	{
		private static readonly bool BankGatewayIsTestMode = Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode");
		private Application _application;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			Drive.Data.Ops.SetServiceConfiguration("BankGateway.IsTestMode", false);
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			Drive.Data.Ops.SetServiceConfiguration("BankGateway.IsTestMode", BankGatewayIsTestMode);
		}

		[Test, AUT(AUT.Za), JIRA("QA-204"), Pending]
		public void AgreementEmailSentAfterApplicationAccepted()
		{
			Customer customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).Build();

			var email = Do.Until(() => Drive.Data.QaData.Db.Email.FindByEmailAddress(customer.Email));

			Assert.AreEqual("18432", email.TemplateName );
		}
	}
}

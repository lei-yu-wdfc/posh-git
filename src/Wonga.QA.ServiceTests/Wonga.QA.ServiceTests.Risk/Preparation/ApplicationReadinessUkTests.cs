
using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.Preparation
{

	[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	public class ApplicationReadinessUkTests : RiskServiceTestBase
	{
		[Test, AUT(AUT.Uk)]
		public void ApplicationIsReadyIfAllDataIsReceived()
		{
			SetupLegitCustomer();
			RunL0Journey();
			AssertVerificationStarted();
		}

		protected override void InitialiseCommands()
		{
			base.InitialiseCommands();

			Messages.Add<RiskSaveCustomerDetailsCommand>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.DateOfBirth = new DateTime(1990, 08, 09);
					x.Forename = "John";
					x.HomePhone = "0207050520";
					x.MiddleName = "Arnie";
					x.Surname = "Conor";
					x.WorkPhone = "0207450510";
				});

			Messages.Add<RiskSaveCustomerAddressCommand>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.AddressId = AddressId;
					x.HouseNumber = "1";
					x.Postcode = "NW1 7SN";
					x.Street = "Prince Albert Road";
					x.Town = "London";
					x.County = "UK";
					x.HouseName = "1";
					x.Flat = "1";
					x.District = "1";
				});

			Messages.Add<SubmitApplicationBehaviourCommand>(
				x =>
				{
					x.ApplicationId = ApplicationId;
					x.TermSliderPosition = "Default";
					x.AmountSliderPosition = "Default";
				});


		}
	}
}

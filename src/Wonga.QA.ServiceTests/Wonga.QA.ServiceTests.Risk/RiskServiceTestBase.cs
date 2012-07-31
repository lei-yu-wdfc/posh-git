using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Svc.Mocks;
using RiskSaveCustomerAddressCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerAddress;
using RiskSaveCustomerDetailsCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerDetails;

namespace Wonga.QA.ServiceTests.Risk
{
	public abstract class RiskServiceTestBase : ServiceTestBase
	{
		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();
			CheckpointTestSettings = new CheckpointTestSettings();
			EndpointMock = new EndpointMock("servicetest",Drive.Msmq.Risk);
			EndpointMock.Start();
		}

		protected override void AfterEachTest()
		{
			base.AfterEachTest();
			if(EndpointMock!=null)
				EndpointMock.Dispose();
		}

		#region Assertions

		protected void AssertVerificationStarted()
		{
			AssertApplicationDecisionIsNot(ApplicationDecisionStatus.WaitForData);
		}

		protected void AssertLoanIsAccepted()
		{
			AssertApplicationDecisionIs(ApplicationDecisionStatus.Accepted);
		}

		protected void AssertLoanIsDeclined()
		{
			AssertApplicationDecisionIs(ApplicationDecisionStatus.Declined);
		}

		//TODO[seb]: this is a duplication from QAF application builder. Move all messages and this to a custom QAF builder
		private void AssertApplicationDecisionIs(ApplicationDecisionStatus expectedDecision)
		{
			Do.With.Timeout(2).Message("Risk didn't return expected status \"{0}\"", expectedDecision).Until(
				() =>
				(ApplicationDecisionStatus)
				Enum.Parse(typeof (ApplicationDecisionStatus),
				           Drive.Api.Queries.Post(new GetApplicationDecisionQuery {ApplicationId = ApplicationId}).Values[
				           	"ApplicationDecisionStatus"].Single()) == expectedDecision);
		}

		private void AssertApplicationDecisionIsNot(ApplicationDecisionStatus unexpectedDecision)
		{
			Do.With.Timeout(2).Message("Risk returned uiexpected status \"{0}\"", unexpectedDecision).Until(
				() =>
				(ApplicationDecisionStatus)
				Enum.Parse(typeof(ApplicationDecisionStatus),
						   Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = ApplicationId }).Values[
							"ApplicationDecisionStatus"].Single()) != unexpectedDecision);
		}

		protected void Background(RiskMask maskName, string checkpointName = null, string responsibleVerification = null)
		{
			CheckpointTestSettings.MaskName = maskName;
			CheckpointTestSettings.CheckpointName = checkpointName;
			CheckpointTestSettings.ResponsibleVerification = responsibleVerification;
		}

		#endregion

		#region Mocks
		protected EndpointMock EndpointMock;

		#endregion
		protected override void GenerateIds()
		{
			base.GenerateIds();
			PaymentCardId = Guid.NewGuid();
			BankAccountId = Guid.NewGuid();
			AddressId = Guid.NewGuid();
			VerificationId = Guid.NewGuid();
			MainApplicantAccountId = Guid.NewGuid();
		}

		protected void RunL0Journey()
		{
			SendAllMessages();
		}

		#region Customer Profiles
		protected virtual void SetupLegitCustomer(DateTime? dateOfBirth = null)
		{
			////todo: use variables instead
			SetupKathleenAs(
				Messages.Get<Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerDetailsUkCommand>(),
				Messages.Get<Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerAddressUkCommand>(), dateOfBirth);
		}

		protected void SetupKathleenAs(Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerDetailsUkCommand detailsCommand,
									   Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerAddressUkCommand addressCommand,
									   DateTime? dateOfBirth = null)
		{
			detailsCommand.DateOfBirth = (dateOfBirth ?? DateTime.Parse("24-Jan-1992")).ToDate(DateFormat.Date);

			detailsCommand.Forename = "kathleen";
			detailsCommand.Surname = "bridson";

			addressCommand.HouseNumber = "6";
			addressCommand.Street = "Green lane";
			addressCommand.Town = "Shanklin";
			addressCommand.Postcode = "BB12 0NL";
		}
		#endregion

		protected Guid MainApplicantAccountId { get; private set; }
		protected Guid PaymentCardId { get; private set; }
		protected Guid BankAccountId { get; private set; }
		protected Guid AddressId { get; private set; }
		protected Guid VerificationId { get; private set; }

		protected CheckpointTestSettings CheckpointTestSettings { get; set; }

		protected void ThenTheRiskServiceShouldApproveTheLoan()
		{
			AssertLoanIsAccepted();
		}

		protected void ThenTheRiskServiceShouldDeclineTheLoan()
		{
			AssertLoanIsDeclined();
		}
	}
}
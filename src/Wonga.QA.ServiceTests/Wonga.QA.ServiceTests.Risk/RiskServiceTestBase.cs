using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Risk.Queries;
using Wonga.QA.Framework.Core;
using RiskSaveCustomerAddressCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerAddress;
using RiskSaveCustomerDetailsCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerDetails;

namespace Wonga.QA.ServiceTests.Risk
{
	public abstract class RiskServiceTestBase : ServiceTestBase
	{
		protected void AssertVerificationStarted()
		{
			AssertApplicationDecisionIsNot(ApplicationDecisionStatus.WaitForData);
		}

		protected override void GenerateIds()
		{
			base.GenerateIds();
			PaymentCardId = Guid.NewGuid();
			BankAccountId = Guid.NewGuid();
			AddressId = Guid.NewGuid();
			VerificationId = Guid.NewGuid();
			MainApplicantAccountId = Guid.NewGuid();
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

		

		protected void RunL0Journey()
		{
			SendAllMessages();
		}

		#region Customer Profiles
		protected virtual void SetupLegitCustomer(DateTime? dateOfBirth = null)
		{
			////todo: use automapper with a dto (or the msmq version of this command)
			SetupKathleenAs(
				Messages.Get<Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerDetailsUkCommand>(),
				Messages.Get<Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerAddressUkCommand>(), dateOfBirth);
		}

		protected void SetupKathleenAs(Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerDetailsUkCommand detailsCommand,
									   Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk.RiskSaveCustomerAddressUkCommand addressCommand,
									   DateTime? dateOfBirth = null)
		{
			detailsCommand.DateOfBirth = (dateOfBirth ?? DateTime.Parse("24-Jan-1992")).ToDate(DateFormat.Date);

			detailsCommand.Forename = "kathleen";
			detailsCommand.MiddleName = "nicole";
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
	}
}
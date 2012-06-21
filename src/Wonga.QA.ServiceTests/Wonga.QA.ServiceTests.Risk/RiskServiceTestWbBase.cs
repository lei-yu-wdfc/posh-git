using System;
using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages.events;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Framework.Msmq.Messages.Risk.Commands;
using RiskAddBankAccountCommand = Wonga.QA.Framework.Msmq.RiskAddBankAccountCommand;
using RiskAddPaymentCardCommand = Wonga.QA.Framework.Msmq.RiskAddPaymentCardCommand;
using RiskSaveCustomerAddressCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerAddressCommand;
using RiskSaveCustomerDetailsCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerDetailsCommand;
using SubmitApplicationBehaviourCommand = Wonga.QA.Framework.Msmq.Messages.Risk.SubmitApplicationBehaviourCommand;
using SubmitNumberOfGuarantorsCommand = Wonga.QA.Framework.Msmq.Messages.Risk.SubmitNumberOfGuarantorsCommand;

namespace Wonga.QA.ServiceTests.Risk
{
	public abstract class RiskServiceTestWbBase : RiskServiceTestBase
	{
		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();
			
			Messages.Get<RiskCreateBusinessFixedInstallmentLoanApplicationWbCommand >() .ApplicationDate = TestAsOf;
		}

		#region IDs

		protected Guid MainApplicantAccountId { get; private set; }
		protected Guid PaymentCardId { get; private set; }
		protected Guid BankAccountId { get; private set; }
		protected Guid BusinessPaymentCardId { get; private set; }
		protected Guid BusinessBankAccountId { get; private set; }
		protected Guid AddressId { get; private set; }
		protected Guid VerificationId { get; private set; }
		protected Guid OrganisationId { get; private set; }

		protected override void GenerateIds()
		{
			base.GenerateIds();

			MainApplicantAccountId = Guid.NewGuid();
			Console.WriteLine("Main Applicant Account ID:" + MainApplicantAccountId);

			OrganisationId = Guid.NewGuid();
			PaymentCardId = Guid.NewGuid();
			BusinessPaymentCardId = Guid.NewGuid();
			BankAccountId = Guid.NewGuid();
			BusinessBankAccountId = Guid.NewGuid();
			AddressId = Guid.NewGuid();
			VerificationId = Guid.NewGuid();
		}
		#endregion

		#region 
		protected virtual void SetupLegitCustomer(DateTime? dateOfBirth = null)
		{
			SetupKathleenAs(Messages.Get<RiskSaveCustomerDetailsCommand>(), Messages.Get<RiskSaveCustomerAddressCommand>(), dateOfBirth);
		}

		protected void SetupKathleenAs(RiskSaveCustomerDetailsCommand detailsCommand,
		                               RiskSaveCustomerAddressCommand addressCommand,
		                               DateTime? dateOfBirth = null)
		{
			detailsCommand.DateOfBirth = dateOfBirth ?? DateTime.Parse("24-Jan-1992");

			detailsCommand.Forename = "kathleen";
			detailsCommand.MiddleName = "nicole";
			detailsCommand.Surname = "bridson";

			addressCommand.HouseNumber = "6";
			addressCommand.Street = "Green lane";
			addressCommand.Town = "Shanklin";
			addressCommand.Postcode = "BB12 0NL";
		}
		#endregion

		protected void RunL0Journey()
		{
			SendAllMessages();
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
						AddressId = AddressId;
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

			Messages.Add<RiskCreateBusinessFixedInstallmentLoanApplicationWbCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
						x.BusinessBankAccountId = BusinessBankAccountId;
						x.BusinessPaymentCardId = BusinessPaymentCardId;
						x.MainApplicantBankAccountId = BankAccountId;
						x.MainApplicantPaymentCardId = PaymentCardId;
						x.Currency = CurrencyCodeIso4217Enum.GBP;
						x.Term = 10;
						x.LoanAmount = 3000;
						x.ApplicationDate = TestAsOf;
						x.OrganisationId = OrganisationId;
					});

			Messages.Add<RiskAddBankAccountCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.AccountNumber = "33069079";
						x.BankAccountId = BankAccountId;
						x.BankName = "Barclays";
					});

			Messages.Add<VerifyMainBusinessApplicantWbCommand>(
				x =>
					{
						x.CreatedOn = DateTime.UtcNow;
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
					});

			Messages.Add<SubmitNumberOfGuarantorsCommand>(
				x =>
					{
						x.CreatedOn = DateTime.UtcNow;
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
						x.NumberOfGuarantors = 0;
					});

			Messages.Add<IPersonalDetailsAddedEvent>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<IFoundACompanyEvent>(x => x.OrganisationId = OrganisationId);

			Messages.Add<ICurrentAddressAddedEvent>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<IBusinessApplicationAddedEvent>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.OrganisationId = OrganisationId;
						x.ApplicationId = ApplicationId;
					});

			Messages.Add<IBusinessBankAccountAddedEvent>(
				x =>
					{
						x.OrganisationId = OrganisationId;
						x.BankAccountId = BusinessBankAccountId;
					});

			Messages.Add<IBusinessPaymentCardAddedEvent>(
				x =>
					{
						x.OrganisationId = OrganisationId;
						x.PaymentCardId = BusinessPaymentCardId;
					});

			Messages.Add<IRiskPaymentCardAddedEvent>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.PaymentCardId = PaymentCardId;
					});
			Messages.Add<RiskAddPaymentCardCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.CardType = "Visa";
						x.CreatedOn = DateTime.UtcNow;
						CARD_EXPIRY_DATE_FORMAT = "yyyy-MM";
						x.ExpiryDateXml = DateTime.UtcNow.AddYears(2).ToString(CARD_EXPIRY_DATE_FORMAT);
						x.HolderName = "HolderName";
						x.Number = "123456789";
						x.PaymentCardId = PaymentCardId;
						x.SecurityCode = "123";
						x.StartDateXml = DateTime.UtcNow.AddYears(-1).ToString(CARD_EXPIRY_DATE_FORMAT);
					}
				);


			Messages.Add<IMobilePhoneUpdatedEvent>(x => x.AccountId = MainApplicantAccountId);

			Messages.Initialise();
		}
	}
}


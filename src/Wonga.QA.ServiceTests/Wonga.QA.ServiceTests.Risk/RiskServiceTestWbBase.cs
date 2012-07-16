using System;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages.events;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Framework.Msmq.Messages.Risk.Commands;
using RiskAddBankAccountCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskAddBankAccount;
using RiskAddPaymentCardCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskAddPaymentCard;
using RiskSaveCustomerAddressCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerAddress;
using RiskSaveCustomerDetailsCommand = Wonga.QA.Framework.Msmq.Messages.Risk.RiskSaveCustomerDetails;
using SubmitApplicationBehaviourCommand = Wonga.QA.Framework.Msmq.Messages.Risk.SubmitApplicationBehaviour;
using SubmitNumberOfGuarantorsCommand = Wonga.QA.Framework.Msmq.Messages.Risk.SubmitNumberOfGuarantors;

namespace Wonga.QA.ServiceTests.Risk
{
	public abstract class RiskServiceTestWbBase : RiskServiceTestBase
	{
		protected override void BeforeEachTest()
		{
			base.BeforeEachTest();
			
			Messages.Get<RiskCreateBusinessFixedInstallmentLoanApplication >() .ApplicationDate = TestAsOf;
		}

		#region IDs

		protected Guid BusinessPaymentCardId { get; private set; }
		protected Guid BusinessBankAccountId { get; private set; }
		protected Guid OrganisationId { get; private set; }

		protected override void GenerateIds()
		{
			base.GenerateIds();
			OrganisationId = Guid.NewGuid();
			BusinessPaymentCardId = Guid.NewGuid();
			BusinessBankAccountId = Guid.NewGuid();
		}
		#endregion

		protected override void InitialiseCommands()
		{
			base.InitialiseCommands();

			Messages.Add<RiskSaveCustomerDetailsCommand>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<RiskSaveCustomerAddressCommand>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<SubmitApplicationBehaviourCommand>(x => x.ApplicationId = ApplicationId);
			Messages.Add<RiskCreateBusinessFixedInstallmentLoanApplication>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
						x.BusinessBankAccountId = BusinessBankAccountId;
						x.BusinessPaymentCardId = BusinessPaymentCardId;
						x.MainApplicantBankAccountId = BankAccountId;
						x.MainApplicantPaymentCardId = PaymentCardId;
						x.ApplicationDate = TestAsOf;
						x.OrganisationId = OrganisationId;
					});

			Messages.Add<RiskAddBankAccountCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.BankAccountId = BankAccountId;
					});

			Messages.Add<VerifyMainBusinessApplicant>(
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

			Messages.Add<IPersonalDetailsAdded>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<IFoundACompany>(x => x.OrganisationId = OrganisationId);

			Messages.Add<ICurrentAddressAdded>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<IBusinessApplicationAdded>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.OrganisationId = OrganisationId;
						x.ApplicationId = ApplicationId;
					});

			Messages.Add<IBusinessBankAccountAdded>(
				x =>
					{
						x.OrganisationId = OrganisationId;
						x.BankAccountId = BusinessBankAccountId;
					});

			Messages.Add<IBusinessPaymentCardAdded>(
				x =>
					{
						x.OrganisationId = OrganisationId;
						x.PaymentCardId = BusinessPaymentCardId;
					});

			Messages.Add<IRiskPaymentCardAdded>(
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


			Messages.Add<IMobilePhoneUpdated>(x => x.AccountId = MainApplicantAccountId);

			Messages.Initialise();
		}
	}
}


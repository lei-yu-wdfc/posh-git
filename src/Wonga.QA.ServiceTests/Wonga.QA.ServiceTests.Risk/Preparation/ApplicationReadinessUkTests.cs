
using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
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

			Messages.Add<RiskSaveCustomerDetailsCommand>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<RiskSaveCustomerAddressCommand>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<SubmitApplicationBehaviourCommand>(x => x.ApplicationId = ApplicationId);
			Messages.Add<RiskCreateFixedTermLoanApplicationCommand>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.ApplicationId = ApplicationId;
					x.BankAccountId = BankAccountId;
					x.PaymentCardId = PaymentCardId;
				});
			Messages.Add<IApplicationAddedEvent>(
			x =>
			{
				x.AccountId = MainApplicantAccountId;
				x.ApplicationId = ApplicationId;
				x.CreatedOn = TestAsOf;
			});
			Messages.Add<RiskAddBankAccountCommand>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.BankAccountId = BankAccountId;
				});

			Messages.Add<VerifyFixedTermLoanCommand>(
				x =>
				{
					x.CreatedOn = DateTime.UtcNow;
					x.AccountId = MainApplicantAccountId;
					x.ApplicationId = ApplicationId;
				});

			Messages.Add<IFixedTermApplicationAddedEvent>(
			x =>
			{
				x.AccountId = MainApplicantAccountId;
				x.ApplicationId = ApplicationId;
				x.CreatedOn = TestAsOf;
			});

			Messages.Add<IBankAccountActivatedEvent>(
				x =>
					{
						x.BankAccountId = BankAccountId;
						x.CreatedOn = TestAsOf;
					});
			
			Messages.Add<IPersonalDetailsAddedEvent>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<ICurrentAddressAddedEvent>(x => x.AccountId = MainApplicantAccountId);

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

			Messages.Add<IPaymentCardAddedEvent>(
				x =>
				{
					x.PaymentCardId = PaymentCardId;
					x.AccountId = MainApplicantAccountId;
				});

			Messages.Add<IBankAccountAddedEvent>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.BankAccountId = BankAccountId;
				});

			Messages.Add<IMobilePhoneUpdatedEvent>(x => x.AccountId = MainApplicantAccountId);

		}
	}
}

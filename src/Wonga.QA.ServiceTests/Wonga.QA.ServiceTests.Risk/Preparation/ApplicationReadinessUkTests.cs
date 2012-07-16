
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

			Messages.Add<RiskSaveCustomerDetails>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<RiskSaveCustomerAddress>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<SubmitApplicationBehaviour>(x => x.ApplicationId = ApplicationId);
			Messages.Add<RiskCreateFixedTermLoanApplication>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.ApplicationId = ApplicationId;
					x.BankAccountId = BankAccountId;
					x.PaymentCardId = PaymentCardId;
				});
			Messages.Add<IApplicationAdded>(
			x =>
			{
				x.AccountId = MainApplicantAccountId;
				x.ApplicationId = ApplicationId;
				x.CreatedOn = TestAsOf;
			});
			Messages.Add<RiskAddBankAccount>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.BankAccountId = BankAccountId;
				});

			Messages.Add<VerifyFixedTermLoan>(
				x =>
				{
					x.CreatedOn = DateTime.UtcNow;
					x.AccountId = MainApplicantAccountId;
					x.ApplicationId = ApplicationId;
				});

			Messages.Add<IFixedTermApplicationAdded>(
			x =>
			{
				x.AccountId = MainApplicantAccountId;
				x.ApplicationId = ApplicationId;
				x.CreatedOn = TestAsOf;
			});

			Messages.Add<IBankAccountActivated>(
				x =>
					{
						x.BankAccountId = BankAccountId;
						x.CreatedOn = TestAsOf;
					});
			
			Messages.Add<IPersonalDetailsAdded>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<ICurrentAddressAdded>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<IRiskPaymentCardAdded>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.PaymentCardId = PaymentCardId;
				});
			Messages.Add<RiskAddPaymentCard>(
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

			Messages.Add<IPaymentCardAdded>(
				x =>
				{
					x.PaymentCardId = PaymentCardId;
					x.AccountId = MainApplicantAccountId;
				});

			Messages.Add<IBankAccountAdded>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.BankAccountId = BankAccountId;
				});

			Messages.Add<IMobilePhoneUpdated>(x => x.AccountId = MainApplicantAccountId);

		}
	}
}

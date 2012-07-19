
using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using apiCommands = Wonga.QA.Framework.Api.Requests.Risk.Commands;

namespace Wonga.QA.ServiceTests.Risk.CL.uk
{
	//[Parallelizable(TestScope.All), AUT(AUT.Uk)]
	public class ApplicationReadinessWbTests : RiskServiceTestBase
	{
		[Test]
		public void ApplicationIsReadyIfAllDataIsReceived()
		{
			SetupLegitCustomer();
			RunL0Journey();
			AssertVerificationStarted();
		}

		protected override void DeclareCommands()
		{
			base.DeclareCommands();

			Messages.Add<apiCommands.Uk.RiskSaveCustomerDetailsUkCommand>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<apiCommands.Uk.RiskSaveCustomerAddressUkCommand>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<apiCommands.SubmitApplicationBehaviourCommand>(x => x.ApplicationId = ApplicationId);
			Messages.Add<apiCommands.RiskCreateFixedTermLoanApplicationCommand>(
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
			Messages.Add<IAccountCreated>(x => x.AccountId = MainApplicantAccountId);
			Messages.Add<apiCommands.Uk.RiskAddBankAccountUkCommand>(
				x =>
				{
					x.AccountId = MainApplicantAccountId;
					x.BankAccountId = BankAccountId;
				});

			Messages.Add<apiCommands.VerifyFixedTermLoanCommand>(
				x =>
				{
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
					x.ExpiryDateXml = DateTime.UtcNow.AddYears(2).ToString(CardExpiryDateFormat);
					x.HolderName = "HolderName";
					x.Number = "123456789";
					x.PaymentCardId = PaymentCardId;
					x.SecurityCode = "123";
					x.StartDateXml = DateTime.UtcNow.AddYears(-1).ToString(CardExpiryDateFormat);
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

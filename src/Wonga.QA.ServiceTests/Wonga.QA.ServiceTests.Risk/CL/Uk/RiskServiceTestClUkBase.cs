using System;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Framework.Msmq.Messages.Risk.BlackList;
using Wonga.QA.Framework.Mocks.Service;

namespace Wonga.QA.ServiceTests.Risk.CL.uk
{
	public class RiskServiceTestClUkBase : RiskServiceTestBase
	{
		protected override void DeclareCommands()
		{
			base.DeclareCommands();

			Messages.Add<RiskSaveCustomerDetailsUkCommand>(x =>
			                                               	{
			                                               		x.AccountId = MainApplicantAccountId;
			                                               		x.MiddleName = CheckpointTestSettings.MaskName;
			                                               	});

			Messages.Add<RiskSaveCustomerAddressUkCommand>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<SubmitApplicationBehaviourCommand>(x => x.ApplicationId = ApplicationId);

			Messages.Add<RiskCreateFixedTermLoanApplicationCommand>(
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

			Messages.Add<RiskAddBankAccountUkCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.BankAccountId = BankAccountId;
					});

			Messages.Add<VerifyFixedTermLoanCommand>(
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

		protected void WhenTheL0UserAppliesForALoan()
		{
			RunL0Journey();
		}

		protected void GivenThatApplicantIsOnBlackList()
		{ 
			EndpointMock
				.OnArrivalOf<ConsumerBlackListRequestMessage>()
				.Matching(x => x.ApplicationId == ApplicationId)
				.ThenDoThis(receivedMsg =>
				{
					var response = CreateBlacklistedMessage();
					response.SagaId = receivedMsg.SagaId;
					Send(response);
				})

				.SeemsLegit().Dude();
		}

		private ConsumerBlackListResponseMessage CreateBlacklistedMessage()
		{
			return new ConsumerBlackListResponseMessage {PresentInBlackList = true};
		}
	}
}
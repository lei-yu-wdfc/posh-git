using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Queries.PayLater.Uk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Risk;
using Wonga.QA.Framework.Msmq.PublicMessages.Payments.PayLater.UK;

namespace Wonga.QA.ServiceTests.Risk.PayLater
{
    public class RiskServiceTestPayLaterUkBase : RiskServiceTestBase
	{
		protected override void DeclareCommands()
		{
			base.DeclareCommands();

			Messages.Add<RiskSavePayLaterCustomerDetailsPayLaterUkCommand>(x =>
			                                               	{
			                                               		x.AccountId = MainApplicantAccountId;
			                                               		x.Surname = CheckpointTestSettings.MaskName;
			                                               	});

			Messages.Add<RiskSavePayLaterCustomerAddressPayLaterUkCommand>(x => x.AccountId = MainApplicantAccountId);

            Messages.Add<RiskSavePayLaterEmploymentDetailsPayLaterUkCommand>(x => x.AccountId = MainApplicantAccountId);
			
            Messages.Add<SubmitClientWatermarkCommand>(
            x =>
            {
		        x.ApplicationId = ApplicationId;
                x.AccountId = MainApplicantAccountId;
                x.ClientIPAddress = "127.0.0.1";
                x.BlackboxData = "fdafjksdgsajdgbagsfgtwauiofdh";
            });

			Messages.Add<RiskCreatePayLaterApplicationUkCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
						x.PaymentCardId = PaymentCardId;
					});

            Messages.Add<IPayLaterApplicationAdded>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
						x.CreatedOn = TestAsOf;
					});

		    Messages.Add<ICustomerTransactionFeeAdded>(x =>
		                                                   {
		                                                       x.AccountId = MainApplicantAccountId;
		                                                       x.ApplicationId = ApplicationId;
		                                                       x.TransactionFee = 7.0m;
		                                                   });

            Messages.Add<IInstalmentAdded>(x =>
            {
                x.AccountId = MainApplicantAccountId;
                x.ApplicationId = ApplicationId;
                x.InstallmentAmount = 5.0m;
                x.InstallmentNumber = 1;
            });
            
			Messages.Add<IAccountCreated>(x => x.AccountId = MainApplicantAccountId);

			Messages.Add<RiskAddBankAccountUkCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.BankAccountId = BankAccountId;
					});

            Messages.Add<VerifyPaylaterApplicationUkCommand>(
				x =>
					{
						x.AccountId = MainApplicantAccountId;
						x.ApplicationId = ApplicationId;
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

        #region Customer Profiles
        protected override void SetupLegitCustomer(DateTime? dateOfBirth = null)
        {
            ////todo: use variables instead
            SetupKathleenAs(
                Messages.Get<RiskSavePayLaterCustomerDetailsPayLaterUkCommand>(),
                Messages.Get<RiskSavePayLaterCustomerAddressPayLaterUkCommand>(),
                Messages.Get<RiskSavePayLaterEmploymentDetailsPayLaterUkCommand>(), dateOfBirth);
        }

        private void SetupKathleenAs(RiskSavePayLaterCustomerDetailsPayLaterUkCommand detailsCommand,
                                       RiskSavePayLaterCustomerAddressPayLaterUkCommand addressCommand,
                                       RiskSavePayLaterEmploymentDetailsPayLaterUkCommand employmentCommand,
                                       DateTime? dateOfBirth = null)
        {
            detailsCommand.DateOfBirth = (dateOfBirth ?? DateTime.Parse("24-Jan-1992")).ToDate(DateFormat.Date);

            detailsCommand.Forename = "kathleen";
            detailsCommand.Surname = "bridson";

            addressCommand.HouseNumber = "6";
            addressCommand.Street = "Green lane";
            addressCommand.Town = "Shanklin";
            addressCommand.Postcode = "BB12 0NL";

            employmentCommand.NetIncome = 299.0m;
            employmentCommand.NextPayDate = DateTime.UtcNow.AddDays(15).ToDate(DateFormat.Date);
        }
        #endregion

        // Todo: Will defer to base class when GetPaylaterApplicationDecision removed
        protected new void AssertVerificationStarted()
        {
            AssertApplicationDecisionIsNot(ApplicationDecisionStatus.WaitForData);
        }

        private void AssertApplicationDecisionIsNot(ApplicationDecisionStatus unexpectedDecision)
        {
            Do.With.Timeout(2).Message("Risk returned uiexpected status \"{0}\"", unexpectedDecision).Until(
                () =>
                (ApplicationDecisionStatus)
                Enum.Parse(typeof(ApplicationDecisionStatus),
                           Drive.Api.Queries.Post(new GetPaylaterApplicationDecision { ApplicationId = ApplicationId }).Values[
                            "ApplicationDecisionStatus"].Single()) != unexpectedDecision);
        }
	} 
}
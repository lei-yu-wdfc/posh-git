using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;
using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Messages.Comms.Commands;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.AddPaymentCardCommand;
using CreateFixedTermLoanExtensionCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateFixedTermLoanExtensionCommand;

namespace Wonga.QA.Tests.Comms.Sms
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class ExtensionSmsTests
	{
		private LoanExtensionEntity _extension;
		private Guid _accountId;
		private Guid _applicationId;
		private static readonly dynamic SmsMessages = Drive.Data.Sms.Db.SmsMessages;
		private static readonly dynamic Applications = Drive.Data.Payments.Db.Applications;
		private static readonly dynamic FixedTermLoanApplications = Drive.Data.Payments.Db.FixedTermLoanApplications;
		private static readonly dynamic PaymentCardsBase = Drive.Data.Payments.Db.PaymentCardsBases;
		private static readonly dynamic LoanExtensions = Drive.Data.Payments.Db.LoanExtensions;
		private static readonly dynamic MobilePhoneVerifications = Drive.Data.Comms.Db.MobilePhoneVerifications;
		private static readonly dynamic CustomerDetails = Drive.Data.Comms.Db.CustomerDetails;
		
		[Test, AUT(AUT.Uk), JIRA("UK-2113")]
		public void ExtensionCancellationSms()
		{	
			var mobileNumber = Get.GetMobilePhone();
			var formattedPhoneNumber = string.Format("{0}{1}", "447", mobileNumber.Substring(2, (mobileNumber.Length - 2)));
			var startTime = DateTime.Now;
			const string messageText =
				"Hi. Your promise date has not been changed, as you did not sign your new loan agreement in time. Sorry.";

			_extension = CreateLoanAndExtend(mobileNumber);

			var extensionId = _extension.ExternalId;

			Do.With.Interval(1).Until(() => Applications.Single(Applications.ApplicationId == _extension.ApplicationId));

			Drive.Msmq.Comms.Send(new IExtensionCancelledEvent
			                      	{
										AccountId = _accountId,
										ApplicationId = _applicationId,
			                      		ExtensionId = extensionId,
			                      		CreatedOn = DateTime.UtcNow
			                      	});
			
			Assert.IsNotNull(Do.Until(() =>
					SmsMessages.Find(SmsMessages.MobilePhoneNumber == formattedPhoneNumber && 
					SmsMessages.CreatedOn > startTime &&
					SmsMessages.MessageText == messageText)));
		}

		private LoanExtensionEntity CreateLoanAndExtend(string mobileNumber)
		{
			const decimal trustRating = 400.00M;
			 _accountId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			_applicationId = Guid.NewGuid();
			var extensionId = Guid.NewGuid();

			var setupData = new AccountSummarySetupFunctions();
			var clientId = Guid.NewGuid();

			CreateCommsData(clientId, _accountId, mobileNumber);

			setupData.Scenario03Setup(_applicationId, paymentCardId, bankAccountId, _accountId, trustRating);

			var app = Do.With.Interval(1).Until(() => Applications.FindByExternalId(_applicationId));
			var fixedTermApp =
				Do.With.Interval(1).Until(
					() => FixedTermLoanApplications.FindByApplicationId(app.ApplicationId));

			Drive.Api.Commands.Post(new AddPaymentCardCommand
			{
				AccountId = _accountId,
				PaymentCardId = paymentCardId,
				CardType = "VISA",
				Number = "4444333322221111",
				HolderName = "Test Holder",
				StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
				ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
				IssueNo = "000",
				SecurityCode = "666",
				IsCreditCard = false,
				IsPrimary = true,
			});

			Do.With.Interval(1).Until(
				() => PaymentCardsBase.FindByExternalId(paymentCardId).AuthorizedOn != null);

			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			{
				ApplicationId = _applicationId,
				ExtendDate = new Date(fixedTermApp.NextDueDate.AddDays(2), DateFormat.Date),
				ExtensionId = extensionId,
				PartPaymentAmount = 20M,
				PaymentCardCv2 = "000",
				PaymentCardId = paymentCardId
			});

			var loanExtension =
				Do.With.Interval(1).Until(
					() =>
					LoanExtensions.Find(LoanExtensions.ExternalId == extensionId && LoanExtensions.ApplicationId == app.ApplicationId
						&& LoanExtensions.PartPaymentTakenOn != null));

			Assert.IsNotNull(loanExtension, "A loan extension should be created");

			return loanExtension;
		}

		private static void CreateCommsData(Guid clientId, Guid accountId, string mobileNumber)
		{
			var forename = string.Format("Joe_{0}", DateTime.UtcNow.Ticks);

			Drive.Msmq.Comms.Send(new
									  SaveCustomerDetailsCommand
			{
				AccountId = accountId,
				ClientId = clientId,
				CreatedOn = DateTime.UtcNow,
				DateOfBirth = new DateTime(1956, 10, 17),
				Email = Get.RandomEmail(),
				Forename = forename,
				Gender = GenderEnum.Male,
				HomePhone = string.Format("02{0}", DateTime.UtcNow.Ticks.ToString().Substring(0, 8)),
				MiddleName = "X",
				MobilePhone = mobileNumber,
				Surname = string.Format("Doe{0}", DateTime.UtcNow.Ticks),
				Title = TitleEnum.Dr,
				WorkPhone = "02078889999"
			});
			Drive.Msmq.Comms.Send(new IAccountCreatedEvent { AccountId = accountId });


			Drive.Msmq.Comms.Send(new
									  SaveCustomerAddressCommand
			{
				CreatedOn = DateTime.UtcNow,
				AccountId = accountId,
				AddressId = Guid.NewGuid(),
				AtAddressFrom = new DateTime(2000, 1, 1),
				ClientId = clientId,
				CountryCode = CountryCodeEnum.Uk,
				Flat = "22",
				HouseName = "7",
				Postcode = "W7 3BX",
				Town = "London",
				Street = "Church Road",
				District = "East",
			});

			var verificationId = Guid.NewGuid();

			Drive.Msmq.Comms.Send(new VerifyMobilePhoneCommand
			                      	{
			                      		AccountId = accountId,
			                      		ClientId = clientId,
			                      		CreatedOn = DateTime.UtcNow,
			                      		MobilePhone = mobileNumber,
			                      		Forename = forename,
			                      		VerificationId = verificationId
			                      	});

			var pin = Do.With.Interval(1).Until(() => MobilePhoneVerifications.FindByVerificationId(verificationId));//Single(MobilePhoneVerifications.VerificationId == verificationId));

			Drive.Msmq.Comms.Send(new CompleteMobilePhoneVerificationCommand
			                      	{
			                      		ClientId = clientId,
			                      		CreatedOn = DateTime.UtcNow,
			                      		Pin = pin.Pin,
			                      		VerificationId = verificationId
			                      	});

			Do.With.Interval(1).Until(
				() => CustomerDetails.FindByAccountId(accountId).MobilePhone != null);
		}
	}
}

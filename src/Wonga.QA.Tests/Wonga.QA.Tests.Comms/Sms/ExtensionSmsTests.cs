using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.AddPaymentCardCommand;
using CreateFixedTermLoanExtensionCommand = Wonga.QA.Framework.Api.CreateFixedTermLoanExtensionCommand;

namespace Wonga.QA.Tests.Comms.Sms
{
	[TestFixture]
	public class ExtensionSmsTests
	{
		private LoanExtensionEntity _extension;
		private Guid _accountId;
		private static readonly dynamic SmsMessages = Drive.Data.Sms.Db.SmsMessages;

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

			var app = Do.With.Interval(1).Until(() => Drive.Db.Payments.Applications.Single(x => x.ApplicationId == _extension.ApplicationId));

			Drive.Msmq.Comms.Send(new IExtensionCancelledEvent
			                      	{
										AccountId = _accountId,
			                      		ApplicationId = app.ExternalId,
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
			var appId = Guid.NewGuid();
			var extensionId = Guid.NewGuid();

			var setupData = new AccountSummarySetupFunctions();
			var clientId = Guid.NewGuid();

			CreateCommsData(clientId, _accountId, mobileNumber);

			setupData.Scenario03Setup(appId, paymentCardId, bankAccountId, _accountId, trustRating);

			var app = Do.With.Interval(1).Until(() => Drive.Db.Payments.Applications.Single(x => x.ExternalId == appId));
			var fixedTermApp =
				Do.With.Interval(1).Until(
					() => Drive.Db.Payments.FixedTermLoanApplications.Single(x => x.ApplicationId == app.ApplicationId));

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
				() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == paymentCardId && x.AuthorizedOn != null));

			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			{
				ApplicationId = appId,
				ExtendDate = new Date(fixedTermApp.NextDueDate.Value.AddDays(2), DateFormat.Date),
				ExtensionId = extensionId,
				PartPaymentAmount = 20M,
				PaymentCardCv2 = "000",
				PaymentCardId = paymentCardId
			});

			var loanExtension =
				Do.With.Interval(1).Until(
					() =>
					Drive.Db.Payments.LoanExtensions.Single(x => x.ExternalId == extensionId && x.ApplicationId == app.ApplicationId
						&& x.PartPaymentTakenOn != null));

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
				Gender = Framework.Msmq.GenderEnum.Male,
				HomePhone = string.Format("02{0}", DateTime.UtcNow.Ticks.ToString().Substring(0, 8)),
				MiddleName = "X",
				MobilePhone = mobileNumber,
				Surname = string.Format("Doe{0}", DateTime.UtcNow.Ticks),
				Title = Framework.Msmq.TitleEnum.Dr,
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
				CountryCode = Framework.Msmq.CountryCodeEnum.Uk,
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

			var pin = Do.With.Interval(1).Until(() => Drive.Db.Comms.MobilePhoneVerifications.Single(x => x.VerificationId == verificationId));

			Drive.Msmq.Comms.Send(new CompleteMobilePhoneVerificationCommand
			                      	{
			                      		ClientId = clientId,
			                      		CreatedOn = DateTime.UtcNow,
			                      		Pin = pin.Pin,
			                      		VerificationId = verificationId
			                      	});

			Do.With.Interval(1).Until(
				() => Drive.Db.Comms.CustomerDetails.Single(x => x.AccountId == accountId).MobilePhone != null);
		}
	}
}

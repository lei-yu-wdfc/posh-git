﻿using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Comms.Commands;
using Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages;
using Wonga.QA.Framework.ThirdParties;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.AddPaymentCardCommand;
using CountryCodeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums.CountryCodeEnum;
using CreateFixedTermLoanExtensionCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateFixedTermLoanExtensionCommand;
using GenderEnum = Wonga.QA.Framework.Msmq.Enums.Common.Enums.GenderEnum;
using TitleEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums.TitleEnum;


namespace Wonga.QA.Tests.Comms.Email
{
	[TestFixture, AUT(AUT.Uk), Parallelizable(TestScope.All)]
	public class ExtensionLoanReminderEmailTests
	{
        private static readonly dynamic EmailTable = Drive.Data.QaData.Db.Email;

		private LoanExtensionEntity _extension;

		private Guid _accountId;
		private Guid _applicationId;
		private readonly Guid _clientId = Guid.NewGuid();
		private Salesforce _salesForce;
	    private string _email;

        [SetUp]
		private void Setup()
		{
			_salesForce = Drive.ThirdParties.Salesforce;

			var sfUsername = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.UserName");
			var sfPassword = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Password");
			var sfUrl = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Url");

			_salesForce.SalesforceUsername = sfUsername.Value;
			_salesForce.SalesforcePassword = sfPassword.Value;
			_salesForce.SalesforceUrl = sfUrl.Value;
			
			_extension = CreateLoanAndExtend();
		}

		[Test, AUT(AUT.Uk), Owner(Owner.AlexSloat)]
		public void SendLoanExtensionReminderEmailTest()
		{
			//Time out extension reminder saga in comms
			var saga = Do.With.Interval(1).Until(() => Drive.Data.OpsSagas.Db.ExtensionReminderEmailSagaEntity.FindByAccountId(_accountId));
			Drive.Msmq.Comms.Send(new TimeoutMessage { ClearTimeout = true, Expires = DateTime.UtcNow, SagaId = saga.Id, State = null });

			//We should see salesforce activity recording a extension reminder message.
			var sfContactId = Do.Until(() => (string)Drive.Data.Salesforce.Db.SalesforceAccounts.FindByAccountId(_accountId).SalesforceId);

			Assert.DoesNotThrow(() => Do.Until(() => _salesForce.GetTask(sfContactId, "WhatId", "Email", "Loan extension reminder.") != null),
								"Extension Reminder activity not found in salesforce for application id: {0} extension Id: {1}", _applicationId, _extension.ExternalId);

            AssertEmailIsSent(_email, 34753);
		}

        private void AssertEmailIsSent(string email, int template)
        {
            Assert.IsNotNull(
                Do.Until(() =>
                         EmailTable.Find(
                            EmailTable.EmailAddress == email &&
                            EmailTable.TemplateName == template)));
        }

		private LoanExtensionEntity CreateLoanAndExtend()
		{
			const decimal trustRating = 400.00M;
			_accountId = Guid.NewGuid();
			var bankAccountId = Guid.NewGuid();
			var paymentCardId = Guid.NewGuid();
			_applicationId = Guid.NewGuid();
			var extensionId = Guid.NewGuid();

			var setupData = new AccountSummarySetupFunctions();


			CreateCommsData(_clientId, _accountId);

			setupData.Scenario03Setup(_applicationId, paymentCardId, bankAccountId, _accountId, trustRating);

			var applicationTab = Drive.Data.Payments.Db.Applications;
			var app = Do.With.Interval(1).Until(() => applicationTab.FindAll(applicationTab.ExternalId == _applicationId).Single());
			var fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
			var fixedTermApp =
				Do.With.Interval(1).Until(
					() => fixedTermLoanAppTab.FindAll(fixedTermLoanAppTab.ApplicationId == app.ApplicationId).Single());

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

			var paymentCardsBaseTab = Drive.Data.Payments.Db.PaymentCardsBase;
			Do.With.Interval(1).Until(
				() => paymentCardsBaseTab.FindAll(paymentCardsBaseTab.ExternalId == paymentCardId && paymentCardsBaseTab.AuthorizedOn != null).Single());

			Drive.Api.Commands.Post(new CreateFixedTermLoanExtensionCommand
			{
				ApplicationId = _applicationId,
				ExtendDate = new Date(fixedTermApp.NextDueDate.Date.AddDays(2), DateFormat.Date),
				ExtensionId = extensionId,
				PartPaymentAmount = 20M,
				PaymentCardCv2 = "000",
				PaymentCardId = paymentCardId
			});

			var loanExtTab = Drive.Data.Payments.Db.LoanExtensions;
			var loanExtension =
				Do.With.Interval(1).Until(
					() =>
					loanExtTab.FindAll(loanExtTab.ExternalId == extensionId &&
									   loanExtTab.ApplicationId == app.ApplicationId &&
									   loanExtTab.PartPaymentTakenOn != null).Single());

			Assert.IsNotNull(loanExtension, "A loan extension should be created");

			return loanExtension;
		}

		private void CreateCommsData(Guid clientId, Guid accountId)
		{
			const string homePhone = "02071111111";

		    _email = Get.RandomEmail();

			Drive.Msmq.Comms.Send(new
			                      	SaveCustomerDetails
			                      	{
			                      		AccountId = accountId,
			                      		ClientId = clientId,
			                      		CreatedOn = DateTime.UtcNow,
			                      		DateOfBirth = Get.GetDoB(),
                                        Email = _email,
			                      		Forename = Get.GetName(),
			                      		Gender = GenderEnum.Male,
			                      		HomePhone = homePhone,
			                      		MiddleName = Get.GetMiddleName(),
			                      		MobilePhone = Get.GetMobilePhone(),
			                      		//string.Format("07{0}", DateTime.UtcNow.Ticks.ToString().Substring(0, 8)),
			                      		Surname = Get.GetName(),
			                      		Title = TitleEnum.Dr,
			                      		WorkPhone = homePhone,
			                      	});
			Drive.Msmq.Comms.Send(new IAccountCreated {AccountId = accountId});


			Assert.DoesNotThrow(() =>
			                    Do.With.Interval(1).Until(() => Drive.Data.Comms.Db.CustomerDetails.FindByAccountId(accountId))
			                    , "CustomerDetails was not created."
				);


			Drive.Msmq.Comms.Send(new
			                      	SaveCustomerAddress
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

			Assert.DoesNotThrow(() =>
			                    Do.With.Interval(1).Until(() => Drive.Data.Comms.Db.Addresses.FindByAccountId(accountId))
			                    , "Address was not created."
				);
		}
	}
}

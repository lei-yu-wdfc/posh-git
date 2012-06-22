using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.AddPaymentCardCommand;
using CreateRepaymentArrangementCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateRepaymentArrangementCommand;
using PaymentFrequencyEnum = Wonga.QA.Framework.Api.Enums.PaymentFrequencyEnum;

namespace Wonga.QA.Tests.Payments
{
	public class RepaymentArrangementTests
	{
		[Test, AUT(AUT.Uk), Parallelizable]
		public void CustomerServiceSetRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();
			
			application.PutIntoArrears();

			Drive.Msmq.Payments.Send(new CreateExtendedRepaymentArrangementCommand
			                          	{
			                          		AccountId = customer.Id,
											ApplicationId = application.Id,
											EffectiveBalance = 100,
											RepaymentAmount = 100,
											RepaymentDetails = new[]
											                   	{
											                   		new ArrangementDetail{Amount = 49, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today},
																	new ArrangementDetail{Amount = 51, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today.AddDays(7)}
											                   	}
			                          	});

			var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			Thread.Sleep(10000);
			var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-725"), Parallelizable]
		public void CreateRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			application.PutIntoArrears(4);

			var cmd = new CreateRepaymentArrangementCommand()
			           	{
			           		ApplicationId = application.Id,
			           		Frequency = PaymentFrequencyEnum.Every4Weeks,
			           		RepaymentDates = new[] {DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1)},
			           		NumberOfMonths = 2
			           	};
			Drive.Api.Commands.Post(cmd);
			
			var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
			var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
			Assert.IsNotNull(Drive.Db.Payments.Transactions.Where(x => x.ApplicationId == dbApplication.ApplicationId && x.Type == "SuspendInterestAccrual"));
		}

		[Test, AUT(AUT.Uk), JIRA("UK-726"), Parallelizable]
		public void CustomerMissesRepaymentArrangementInstallmentTest()
		{
			//Test written to support both mocked and non mocked environments
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			application.PutIntoArrears(4);

			application.CreateRepaymentArrangement();

			var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			
			//Remove card details, replace with bad card details to cause payment failure
			var db = new DbDriver();
			var accountPreferences = db.Payments.AccountPreferences.Single(x => x.AccountId == app.AccountId);
			accountPreferences.PrimaryPaymentCardId = null;
			db.Payments.SubmitChanges();
			var paymentCard = Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid);
			Drive.Db.Payments.PersonalPaymentCards.Single(x => x.PaymentCardId == paymentCard.PaymentCardId).Delete().Submit();
			Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid).Delete().Submit();
			Drive.Db.ColdStorage.PaymentCards.Single(x => x.ExternalId == app.PaymentCardGuid).Delete().Submit();
			Drive.Api.Commands.Post(new AddPaymentCardCommand
			                         	{
			                         		AccountId = app.AccountId,
			                         		PaymentCardId = app.PaymentCardGuid,
			                         		CardType = "Other",
                                            Number = "1111111111111111",
			                         		HolderName = "Test Holder",
			                         		StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
			                         		ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
			                         		IssueNo = "000",
			                         		SecurityCode = "666",
			                         		IsCreditCard = false,
			                         		IsPrimary = true,
			                         	});
			Do.Until(() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid));
			
			var newPaymentCard = db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid);
			//Set date to past for card payment mock
			newPaymentCard.ExpiryDate = DateTime.Today.AddYears(-1);
			accountPreferences.Refresh().PrimaryPaymentCardId = newPaymentCard.PaymentCardId;
			db.Payments.SubmitChanges();

			//Process Repayment Arrangement Installment, expecting failure
			var repaymentArrangement =
				Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == app.ApplicationId);
			var firstRepaymentArrangementDetail =
				db.Payments.RepaymentArrangementDetails.First(
					x => x.RepaymentArrangementId == repaymentArrangement.RepaymentArrangementId);

			Drive.Msmq.Payments.Send(new ProcessScheduledRepaymentCommand
			                          	{
			                          		RepaymentArrangementId = repaymentArrangement.RepaymentArrangementId,
			                          		RepaymentDetailId = firstRepaymentArrangementDetail.RepaymentArrangementDetailId
			                          	});
			
			var scheduledPayment = Do.Until(() => Drive.Db.Payments.ScheduledPayments.Single(x => x.ApplicationId == app.ApplicationId &&
                                                                                                  x.Amount == firstRepaymentArrangementDetail.Amount));
			Assert.IsFalse(scheduledPayment.Success.Value);
		}

		//Needed for serialization in CreateExtendedRepaymentArrangementCommand
		private class ArrangementDetail
		{
			public decimal Amount { get; set; }
			public CurrencyCodeIso4217Enum Currency { get; set; }
			public DateTime DueDate { get; set; }
		}
	}
}

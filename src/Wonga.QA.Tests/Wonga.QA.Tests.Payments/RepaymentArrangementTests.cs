using System;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.AddPaymentCardCommand;
using CreateRepaymentArrangementCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateRepaymentArrangementCommand;
using PaymentFrequencyEnum = Wonga.QA.Framework.Api.Enums.PaymentFrequencyEnum;

namespace Wonga.QA.Tests.Payments
{
	[Parallelizable(TestScope.All)]
	public class RepaymentArrangementTests
	{
		private bool _repaymentArrangementEnabled;
		private ArrangementDetail[] _arrangementDetails;
		private Application _application;

		#region setup#

		[FixtureSetUp]
		public void FixtureSetup()
		{
			_repaymentArrangementEnabled = ConfigurationFunctions.GetRepaymentArrangementEnabled();
			ConfigurationFunctions.SetRepaymentArrangementEnabled(true);
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			ConfigurationFunctions.SetRepaymentArrangementEnabled(_repaymentArrangementEnabled);
		}

		#endregion setup#

		[Test, AUT(AUT.Uk), Owner(Owner.AlexSloat)]
		public void CustomerServiceSetRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			_application = ApplicationBuilder.New(customer).Build();

			_arrangementDetails = ArrangementDetails();

			_application.PutIntoArrears();

			Drive.Msmq.Payments.Send(new CreateExtendedRepaymentArrangement
										{
											AccountId = customer.Id,
											ApplicationId = _application.Id,
											EffectiveBalance = 100.02m,
											RepaymentAmount = 100.02m,
											RepaymentDetails = _arrangementDetails
										});

			var repaymentArrangement = GetRepaymentArrangementEntity(_application);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
		}

		#region RA query Tests
		[Test, JIRA("UKOPS-79"), DependsOn("CustomerServiceSetRepaymentArrangementTest"), Pending("Bug 858-CSAPI query not returning Amount PAid "), Owner(Owner.AnilKrishnamaneni)]
		public void RepaymentArrangemetnDetailsQuery()
		{
			var repaymentArrangementDetails = Drive.Cs.Queries.Post(new GetRepaymentArrangementsQuery() { ApplicationId = _application.Id });
			var repayDate = repaymentArrangementDetails.Values["DueDate"].ToArray();
			var repayAmount = repaymentArrangementDetails.Values["Amount"].ToArray();
			var amountPaid = repaymentArrangementDetails.Values["AmountPaid"].ToArray();
			var currency = repaymentArrangementDetails.Values["Currency"].ToArray();
			var canceledOn = repaymentArrangementDetails.Values["CanceledOn"].SingleOrDefault();
			var closedOn = repaymentArrangementDetails.Values["ClosedOn"].SingleOrDefault();
			var isBroken = repaymentArrangementDetails.Values["IsBroken"].SingleOrDefault();
			for (int i = 0; i < _arrangementDetails.Length; i++)
			{
				Assert.AreEqual(DateTime.Parse(repayDate[i]).ToString("yyyy-MM-dd"), _arrangementDetails[i].DueDate.ToString("yyyy-MM-dd"));
				Assert.AreEqual(repayAmount[i], _arrangementDetails[i].Amount.ToString(CultureInfo.InvariantCulture));
				Assert.AreEqual(currency[i], _arrangementDetails[i].Currency.ToString());
			}
			Assert.AreEqual(amountPaid[0], _arrangementDetails[0].Amount.ToString(CultureInfo.InvariantCulture));
			Assert.IsNull(canceledOn);
			Assert.IsNull(closedOn);
			Assert.IsNull(isBroken);
		}
		
		[Test, AUT(AUT.Uk), JIRA("UKOPS-79"), DependsOn("CancelRepaymentArrangemntAfterRepaymentToday"), Owner(Owner.AnilKrishnamaneni)]
		public void RepaymentArrangemetnDetailsQueryForCanceledRA()
		{
			var repaymentArrangementDetails = Drive.Cs.Queries.Post(new GetRepaymentArrangementsQuery() { ApplicationId = _application.Id });
			var canceledOn = repaymentArrangementDetails.Values["CanceledOn"].SingleOrDefault();
			Assert.AreEqual(DateTime.Parse(canceledOn).ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"));
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-49"), Owner(Owner.AnilKrishnamaneni), Pending("Bug 858-CSAPI query not returning Amount PAid ")]
		public void RepaymentArrangementQueryClosedRA()
		{
		    Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();
			application.CreateRepaymentArrangement(); 
			var repaymentArrangement = GetRepaymentArrangementEntity(application);
			var repaymentArrangementSagaId = (Guid)Do.Until(() => Drive.Data.OpsSagas.Db.RepaymentArrangementSagaEntity.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).Id);
			var count = repaymentArrangement.RepaymentArrangementDetails.Count;
			for (int i = 0; i < count; i++)
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = repaymentArrangementSagaId });
			var repaymentArrangementDetails = Drive.Cs.Queries.Post(new GetRepaymentArrangementsQuery() { ApplicationId = application.Id });
			var closedOn = repaymentArrangementDetails.Values["ClosedOn"].SingleOrDefault();
			Assert.IsTrue(Convert.ToBoolean(closedOn));
		}

		[Test, JIRA("UKOPS-79"), DependsOn("CancelRepaymentArrangementWhenRepaymentArrangementIsBroken"), Pending("UKOPS-822 Ticket Not Implemeneted Yet"),Owner(Owner.AnilKrishnamaneni)]
		public void RepaymentArrangemetnDetailsQueryForBrokenRA()
		{
			var repaymentArrangementDetails = Drive.Cs.Queries.Post(new GetRepaymentArrangementsQuery() { ApplicationId = _application.Id });
			var isBroken = repaymentArrangementDetails.Values["IsBroken"].SingleOrDefault();
			Assert.IsTrue(Convert.ToBoolean(isBroken));
		}

		#endregion


		[Test, AUT(AUT.Uk), JIRA("UKOPS-49"), Owner(Owner.ShaneMcHugh), DependsOn("RepaymentArrangemetnDetailsQuery")]
		public void CancelRepaymentArrangemntAfterRepaymentToday()
		{
			var repaymentArrangement = GetRepaymentArrangementEntity(_application);
			var a = _arrangementDetails[0].Amount;
			var d = _arrangementDetails[0].DueDate;
			Assert.IsNotNull(repaymentArrangement.RepaymentArrangementDetails.First(ra => ra.Amount == a && ra.DueDate == d));
			_application.CancelRepaymentArrangement();
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-49"), Owner(Owner.ShaneMcHugh), Pending("UKOPS-822 Ticket Not Implemeneted Yet")]
		public void CancelRepaymentArrangementWhenRepaymentArrangementIsBroken()
		{

		}

		[Test, AUT(AUT.Uk), JIRA("UK-725"), Owner(Owner.AlexSloat)]
		public void CreateRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build();

			application.PutIntoArrears(4);

			var cmd = new CreateRepaymentArrangementCommand()
						{
							ApplicationId = application.Id,
							Frequency = PaymentFrequencyEnum.Every4Weeks,
							RepaymentDates = new[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1) },
							NumberOfMonths = 2
						};
			var errors = Drive.Api.Commands.Post(cmd).GetErrors().ToList();
			Assert.IsTrue(errors.Count == 0, "No validation errors expected");

			var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
			var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId);
			Assert.AreEqual(2, repaymentArrangement.RepaymentArrangementDetails.Count);
			Assert.IsNotNull(Drive.Db.Payments.Transactions.Where(x => x.ApplicationId == dbApplication.ApplicationId && x.Type == "SuspendInterestAccrual"));
		}
		
		[Test, AUT(AUT.Uk), JIRA("UK-726"), Owner(Owner.AlexSloat)]
		public void CustomerMissesRepaymentArrangementInstallmentTest()
		{
			//Test written to support both mocked and non mocked environments
			Customer customer = CustomerBuilder.New().Build();

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

			Drive.Msmq.Payments.Send(new ProcessScheduledRepaymentMessage
										{
											RepaymentArrangementId = repaymentArrangement.RepaymentArrangementId,
											RepaymentDetailId = firstRepaymentArrangementDetail.RepaymentArrangementDetailId
										});

			var scheduledPayment = Do.Until(() => Drive.Db.Payments.ScheduledPayments.Single(x => x.ApplicationId == app.ApplicationId &&
																									x.Amount == firstRepaymentArrangementDetail.Amount));
			Assert.IsFalse(scheduledPayment.Success.Value);
		}

		[Test, AUT(AUT.Uk), JIRA("UKOPS-49"), Owner(Owner.ShaneMcHugh)]
		public void CancelRepaymentArrangementTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application = ApplicationBuilder.New(customer).Build().PutIntoArrears(20);

			application.CreateRepaymentArrangement();
			application.CancelRepaymentArrangement();
		}

		#region Helpers#

		private static ArrangementDetail[] ArrangementDetails()
		{
			return new[]
			       	{
			       		new ArrangementDetail
			       			{Amount = 49.01m, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today},
			       		new ArrangementDetail
			       			{
			       				Amount = 51.01m,Currency = CurrencyCodeIso4217Enum.GBP,DueDate = DateTime.Today.AddDays(7)
			       			}
			       	};
		}

		//Needed for serialization in CreateExtendedRepaymentArrangementCommand
		private class ArrangementDetail
		{
			public decimal Amount { get; set; }
			public CurrencyCodeIso4217Enum Currency { get; set; }
			public DateTime DueDate { get; set; }
		}

		private RepaymentArrangementEntity GetRepaymentArrangementEntity(Application application)
		{
			var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			var repaymentArrangement =
					Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
			return repaymentArrangement;
		}

		#endregion helpers#
	}
}

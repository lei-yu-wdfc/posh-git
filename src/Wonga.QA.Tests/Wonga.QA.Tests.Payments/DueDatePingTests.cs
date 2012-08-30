using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture(Order = -1)]
    [Parallelizable(TestScope.All)] 
	[AUT(AUT.Uk)]
	class DueDatePingTests
	{

		private Customer _customer;
		private Application _application;
		private Application _secondApplication;
		private dynamic _sagaEntityFiveAm;
		private dynamic _sagaSecondEntityFail;
		private dynamic _sagaEntityFail;
		private static dynamic _primaryPaymentCardId;
		private int _appInternalId;
		private int _secondAppInternalId;
		private static decimal _amountduetoday = 0m;
    	private const string ServiceConfigKey = "Payments.DueTodayPingSchedule";

		private static readonly dynamic DueDatePingScheduleSagaEntities = Drive.Data.OpsSagas.Db.DueDatePingScheduleSagaEntity;

		[FixtureSetUp]
		public void SetUp()
		{
			SetPaymentsUtcNow();
		}

		[FixtureTearDown]
		public void TearDown()
		{
			var db = new DbDriver();
			var paymentsNowDb = db.Ops.ServiceConfigurations.Single(a => a.Key == ServiceConfigKey);
			db.Ops.ServiceConfigurations.DeleteOnSubmit(paymentsNowDb);
			db.Ops.SubmitChanges();
		}

		[Test, JIRA("UKOPS-425"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsShouldCreateNewTransactionWhenFiveAmPingSucceeds()
		{
			var customer = GetCustomer();
			var application = GetApplication(customer);
			var appInternalId = ApplicationOperations.GetAppInternalId(application);
			application.MakeDueToday();
			_sagaEntityFiveAm = GetSagaEntity(appInternalId);
			CheckPaymentTransactionForDuePing(application, appInternalId);
		}

		[Test, JIRA("UKOPS-425"), DependsOn("PaymentsShouldCreateNewTransactionWhenFiveAmPingSucceeds"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsShouldMarkSagaAsCompleteWhenFiveAmPingSucceeds()
		{
			CheckDueDateSagaIsComplete(_sagaEntityFiveAm.Id);
		}

		[Test, JIRA("UKOPS-425"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFiveAmPingFails()
		{
			_customer = GetCustomer();
			_application = GetApplication(_customer);
			_application.ExpireCard();
			_application.MakeDueToday();
			_appInternalId = ApplicationOperations.GetAppInternalId(_application);
			_sagaEntityFail = GetSagaEntity(_appInternalId);
			_primaryPaymentCardId =
				Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_application.AccountId).SingleOrDefault().
					PrimaryPaymentCardId;
			_customer.AddBadCard();
			CheckPaymentRequestDeclinedTransaction(_application, _appInternalId);
			CheckChaseForArrearSagaIsNotComplete(_sagaEntityFail.Id);
			UpdatePrimaryCard(_application.AccountId);
			SetCardExpiryDate(_customer.GetPaymentCard(), DateTime.Now.AddYears(1));
		}

		[Test, JIRA("UKOPS-425"), DependsOn("PaymentsShouldAddRecordToPaymentCardRepaymentRequestWhenFiveAmPingFails"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsShouldCreateNewTransactionWhenFivePingFailsEightAmPingSucceeds()
		{
			CheckPaymentTransactionForDuePing(_application, _appInternalId);
		}

		[Test, JIRA("UKOPS-425"), DependsOn("PaymentsShouldCreateNewTransactionWhenFivePingFailsEightAmPingSucceeds"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsShouldMarkSagaAsCompleteWhenEightAmCollectionSucceedsAfterFirstPingFail()
		{
			CheckDueDateSagaIsComplete(_sagaEntityFail.Id);
		}

		[Test, JIRA("UKOPS-425"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsFirstPingFailsToMakeSecondPingFail()
		{
			var customer = GetCustomer();
			_secondApplication = GetApplication(customer);
			_secondApplication.ExpireCard();
			_secondApplication.MakeDueToday();
			_secondAppInternalId = ApplicationOperations.GetAppInternalId(_secondApplication);
			customer.AddBadCard();
			_sagaSecondEntityFail = GetSagaEntity(_secondAppInternalId);
			CheckPaymentRequestDeclinedTransaction(_secondApplication, _secondAppInternalId);
			CheckChaseForArrearSagaIsNotComplete(_sagaSecondEntityFail.Id);
	    }

		[Test, JIRA("UKOPS-425"), DependsOn("PaymentsFirstPingFailsToMakeSecondPingFail"), Owner(Owner.AnilKrishnamaneni)]
		public void PaymentsRepaymentRequestSecondPingFailsAfterFiVeAmPingFailed()
		{
			int count = 2;
		    if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
		    	count = 3;
			Do.With.Timeout(2).Until<bool>(
				() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: _secondAppInternalId,
																					Amount: _amountduetoday,
																					StatusDescription: "Request Declined"
						  ).Count() == count);
			
			CheckDueDateSagaIsComplete(_sagaSecondEntityFail.Id);
		}

		
		#region Helpers

		private static void CheckPaymentRequestDeclinedTransaction(Application application, int appInternalId)
		{
			Do.With.Timeout(2).Until<bool>(
				() => Drive.Data.Payments.Db.PaymentCardRepaymentRequests.FindAllBy(ApplicationId: appInternalId,
																					Amount: _amountduetoday,
																					StatusDescription: "Request Declined"
						  ).Count() == 1);
		}

		public static void CheckPaymentTransactionForDuePing(Application application, int appInternalId)
		{
			Do.With.Timeout(2).Until<bool>(() => Drive.Data.Payments.Db.Transactions.FindAllBy(ApplicationId: appInternalId,
																							   Type:
																								   PaymentTransactionEnum.
																								   CardPayment.ToString(),
																							   Amount: _amountduetoday * -1,
																							   Scope:
																								   (int)
																								   PaymentTransactionScopeEnum.
																									   Credit,
																							   Reference:
																								   "Automatic Ping (Card)").
													 Count() == 1);
		}


    	private static void CheckDueDateSagaIsComplete(dynamic sagaId)
    	{
    		object id = null;
    		id = DueDatePingScheduleSagaEntities.FindById((Guid) sagaId);
			Do.With.Timeout(1).Until<bool>(() => id == null);
		}

		private static dynamic GetSagaEntity(int appInternalId)
		{
			return Do.With.Timeout(2).Until(() => DueDatePingScheduleSagaEntities.FindAll(DueDatePingScheduleSagaEntities.ApplicationId == appInternalId).Single());
		}


		private static void SetCardExpiryDate(Guid card, DateTime expiryDate)
		{
			Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: card, ExpiryDate: expiryDate, DeactivatedOn: null);
		}

		private static void UpdatePrimaryCard(Guid id)
		{
			Drive.Data.Payments.Db.AccountPreferences.UpdateByAccountId(AccountId: id,
																		PrimaryPaymentCardId: _primaryPaymentCardId);
		}

		private static void CheckChaseForArrearSagaIsNotComplete(dynamic sagaId)
		{
			Assert.IsNotNull(DueDatePingScheduleSagaEntities.FindById((Guid)sagaId));
		}

		private static Customer GetCustomer()
		{
			var customer = CustomerBuilder.New().Build();
			return customer;
		}

		private static Application GetApplication(Customer customer)
		{
			const decimal loanAmount = 100;
			var application = ApplicationBuilder.New(customer)
				.WithLoanAmount(loanAmount)
				.WithLoanTerm(7)
				.Build();
			_amountduetoday = application.GetDueDateBalance(); 
			return application;
		}

		private void SetPaymentsUtcNow()
		{
			Drive.Db.SetServiceConfiguration(ServiceConfigKey, GetThreeTimeoutsFromNow());
		}

		private string GetThreeTimeoutsFromNow()
		{
			var time = DateTime.Now.AddMinutes(2).ToString("HH:mm");
			time += ";" + DateTime.Now.AddMinutes(3).ToString("HH:mm");
			time += ";" + DateTime.Now.AddMinutes(4).ToString("HH:mm");
			return time;
		}

		#endregion
	}
}

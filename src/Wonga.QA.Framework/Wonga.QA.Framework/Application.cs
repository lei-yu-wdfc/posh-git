using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.SagaMessages;
using CreateRepaymentArrangementCommand = Wonga.QA.Framework.Api.Requests.Payments.Commands.CreateRepaymentArrangementCommand;
using PaymentTransactionEnum =  Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionEnum;
using PaymentTransactionScopeEnum = Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums.PaymentTransactionScopeEnum;

namespace Wonga.QA.Framework
{
	public class Application
	{
		public Guid Id { get; set; }
		public Guid BankAccountId { get; set; }
		public decimal LoanAmount { get; set; }
		public int LoanTerm { get; set; }
		public string FailedCheckpoint { get; private set; }
        public string BankAccountNumber { get; set; }

	    private readonly dynamic _applicationsTab = Drive.Data.Payments.Db.Applications;
	    private readonly dynamic _customerDetailsTab = Drive.Data.Comms.Db.CustomerDetails;
	    private readonly dynamic _accountPreferencesTab = Drive.Data.Payments.Db.AccountPreferences;
	    private readonly dynamic _scheduledPaymentSagaEntityTab = Drive.Data.OpsSagas.Db.ScheduledPaymentSagaEntity;
	    private readonly dynamic _fixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
	    private readonly dynamic _riskAppTab = Drive.Data.Risk.Db.RiskApplications;
        private readonly dynamic _transactionsTab = Drive.Data.Payments.Db.Transactions;
	    private readonly dynamic _loanDueDateNotifiSagaEntityTab = Drive.Data.OpsSagas.Db.LoanDueDateNotificationSagaEntity;
	    private readonly dynamic _fixedTermLoanSagaEntityTab = Drive.Data.OpsSagas.Db.FixedTermLoanSagaEntity;
	    private readonly dynamic _scheduledPostAccruedInterestSagaEntityTab = Drive.Data.OpsSagas.Db.ScheduledPostAccruedInterestSagaEntity;
	    private readonly dynamic _arrearsTab = Drive.Data.Payments.Db.Arrears;
	    private readonly dynamic _serviceConfigTab = Drive.Data.Ops.Db.ServiceConfigurations;
        private readonly dynamic _repaymentArrangementsTab = Drive.Data.Payments.Db.RepaymentArrangements;

        private static readonly dynamic ServiceConfigTab = Drive.Data.Ops.Db.ServiceConfigurations;

	    public Application()
		{
		}

		public Application(Guid id)
		{
			Id = id;
		}

		public Application(Guid id, string failedCheckpoint)
		{
			Id = id;
			FailedCheckpoint = failedCheckpoint;
		}

		public bool IsClosed
		{
			get { return _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single().ClosedOn == null ? false : true; }
		}
		 
		public Guid AccountId
		{
            get { return _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single().AccountId; }
		}

		public Customer GetCustomer()
		{
			//avoid going to the DB twice
			Guid currentAccountId = AccountId;
			return new Customer(
                _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single().AccountId,
                _customerDetailsTab.FindAll(_customerDetailsTab.AccountId == currentAccountId).Single().Email,
                _accountPreferencesTab.FindAll(_accountPreferencesTab.AccountId == currentAccountId).Single().BankAccountsBase.ExternalId);
		}

		public Application RepayOnDueDate()
		{
            var application = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();

			MakeDueToday(application);
			
		    if (!BankGatwayTakePaymentResponseIsMocked())
		    {
                var utcNow = DateTime.UtcNow;

                ScheduledPaymentSagaEntity sp = Do.Until(() => _scheduledPaymentSagaEntityTab.FindAll(_scheduledPaymentSagaEntityTab.ApplicationGuid == Id).Single());
                Drive.Msmq.Payments.Send(new PaymentTakenMessage { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow, ApplicationId = Id, TransactionAmount = GetBalance() });
                Do.While(sp.Refresh);
		    }

			var transaction = WaitForDirectBankPaymentCreditTransaction();

			TimeoutCloseApplicationSaga(transaction);

            Do.Until(() => _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single().ClosedOn);

			return this;
		}

        private static bool BankGatwayTakePaymentResponseIsMocked()
        {
            return BankGatwayInTestMode() || IsCaTestWithScotiaMocked();
        }

        private static bool BankGatwayInTestMode()
        {
            var testmode = ServiceConfigTab.FindAll(ServiceConfigTab.Key == "BankGateway.IsTestMode").SingleOrDefault();

            return testmode != null && Boolean.Parse(testmode.Value);
        }

        private static bool IsCaTestWithScotiaMocked()
        {
            var caScotiaMocksEnabled = ServiceConfigTab.FindAll(ServiceConfigTab.Key == "Mocks.ScotiaEnabled").SingleOrDefault();

            return caScotiaMocksEnabled != null && (Config.AUT == AUT.Ca && Boolean.Parse(caScotiaMocksEnabled.Value));
        }

		private static void TimeoutCloseApplicationSaga(dynamic transaction)
		{
			var closeAppSagaEntityTab = Drive.Data.OpsSagas.Db.CloseApplicationSagaEntity;
			CloseApplicationSagaEntity ca =
				Do.Until(
					() => closeAppSagaEntityTab.FindAll(closeAppSagaEntityTab.TransactionId == transaction.ExternalId).Single());
			Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ca.Id });
			Do.While(ca.Refresh);
		}

	    private TransactionEntity WaitForDirectBankPaymentCreditTransaction()
        {
            #region:find a way to fix this
            /*
	        var appId = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id);

            var transId = _transactionsTab.FindAll(_transactionsTab.ExternalId == appId.ExternalId);


	        transId.FindAll(_transactionsTab.Scope == (int) PaymentTransactionScopeEnum.Credit &&
	                        _transactionsTab.Type ==
	                        Get.EnumToString(Config.AUT == AUT.Uk
	                                             ? PaymentTransactionEnum.CardPayment
	                                             : PaymentTransactionEnum.DirectBankPayment).Single());
	        
                
                
                
	          

            var check2 = _transactionsTab.Type == Get.EnumToString(
                Config.AUT == AUT.Uk
                    ? PaymentTransactionEnum.CardPayment
                    : PaymentTransactionEnum.DirectBankPayment);

	        var test2 =
	            Do.Until(
	                () =>
	                _transactionsTab.FindAll(_transactionsTab.ExternalId == appId.ExternalId &&
	                                         _transactionsTab.Scope == (int) PaymentTransactionScopeEnum.Credit && check2).
	                    Single());
	        
	        

	        return Do.Until(() => _transactionsTab.FindAll(
	            _transactionsTab.ExternalId == Id &&
	            _transactionsTab.Scope == (int) PaymentTransactionScopeEnum.Credit &&
	            _transactionsTab.Type == Get.EnumToString(
	                Config.AUT == AUT.Uk
	                    ? PaymentTransactionEnum.CardPayment
	                    : PaymentTransactionEnum.DirectBankPayment)).Single());

*/
            #endregion
            return Do.Until(() => Drive.Db.Payments.Applications.Single(
	            a => a.ExternalId == Id).Transactions.Single(
	                t =>
	                (PaymentTransactionScopeEnum) t.Scope == PaymentTransactionScopeEnum.Credit && t.Type == Get.EnumToString(
	                    Config.AUT == AUT.Uk ? PaymentTransactionEnum.CardPayment : PaymentTransactionEnum.DirectBankPayment)));
	    }

	    public Application MakeDueToday()
		{
            var application = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();

			MakeDueToday(application);

			return this;
		}

        public Application UpdateNextDueDate(int days)
        {
            var application = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();
            var fixedApp = _fixedTermLoanAppTab.FindAll(_fixedTermLoanAppTab.ApplicationId == application.ApplicationId).Single();

            var span = new TimeSpan(days, 0, 0, 0);
            ApplicationOperations.UpdateNextDueDate(fixedApp, span);

            return this;
        }

        public Application UpdateAcceptedOnDate(int days)
        {
            var application = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();

            var span = new TimeSpan(days, 0, 0, 0);
            ApplicationOperations.MoveAcceptedOnDate(application, span);
            return this;
        }
        
		public Application RewindApplicationDates(TimeSpan rewindSpan)
		{
            var application = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();

            RewindAppDates(application, rewindSpan);

			return this;
		}

		public virtual Application RewindApplicationFurther(uint daysInArrears)
		{
			Drive.Db.Rewind(Id, (int)daysInArrears);

			return this;
		}

        private void RewindAppDates(dynamic application, TimeSpan rewindSpan)
        {
            var riskApplication = _riskAppTab.FindAll(_riskAppTab.ApplicationId == Id).Single();

            ApplicationOperations.RewindApplicationDates(application, riskApplication, rewindSpan);
        }
		
		public Application RewindApplicationDatesForDays(int days)
		{
			return RewindApplicationDates(TimeSpan.FromDays(days));
		}

        private void RewindAppDates(dynamic application)
        {
            TimeSpan span = _fixedTermLoanAppTab.FindByApplicationId(application.ApplicationId).NextDueDate - DateTime.Today;
            RewindAppDates(application, span);
        }

		public void MakeDueToday(dynamic application)
		{
			RewindAppDates(application);
            var ldd = _loanDueDateNotifiSagaEntityTab.FindAll(_loanDueDateNotifiSagaEntityTab.ApplicationId == Id).Single();
            if (Drive.Data.Ops.GetServiceConfiguration<bool>("Payments.FeatureSwitches.UseLoanDurationSaga") == false)
            {
                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ldd.Id });
            _loanDueDateNotifiSagaEntityTab.Update(ldd);

            var ftl = _fixedTermLoanSagaEntityTab.FindAll(_fixedTermLoanSagaEntityTab.ApplicationGuid == Id).Single();
                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
            _fixedTermLoanSagaEntityTab.Update(ftl);
            }
            else
            {
                //We should timeout the LoanDurationSaga...
                dynamic loanDurationSagaEntities = Drive.Data.OpsSagas.Db.LoanDurationSagaEntity;
                var loanDurationSaga = loanDurationSagaEntities.FindAllByAccountGuid(AccountGuid: application.AccountId).FirstOrDefault();

                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = loanDurationSaga.Id });
            }
		}


		#region Arrears

		public virtual Application PutIntoArrears(uint daysInArrears)
		{
			var totalDays = daysInArrears;

			if (!IsInArrears())
			{
				PutIntoArrears();
			}

			else
			{
				totalDays = totalDays - GetDaysInArrears();
			}

			Drive.Db.Rewind(Id, (int)totalDays);

			return this;
		}

        public virtual Application ExpireCard()
        {
            Customer customer = this.GetCustomer();
            Drive.Data.Payments.Db.PaymentCardsBase.UpdateByExternalId(ExternalId: customer.GetPaymentCard(),
                        ExpiryDate: new DateTime(DateTime.Now.Year - 1, 1, 31));
            return this;
        }

		public virtual Application PutIntoArrears()
		{
            var application = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();
		    var fixTermLoanApp =
		        _fixedTermLoanAppTab.FindAll(_fixedTermLoanAppTab.ApplicationId == application.ApplicationId).Single();

		    DateTime dueDate = fixTermLoanApp.NextDueDate ?? fixTermLoanApp.PromiseDate;

            var riskApplication = _riskAppTab.FindAll(_riskAppTab.ApplicationId == Id).Single();

			TimeSpan span = dueDate - DateTime.Today;

			ApplicationOperations.RewindApplicationDates(application, riskApplication, span);

            if (Config.AUT == AUT.Ca || Config.AUT == AUT.Za)
            {
                var entity = _scheduledPostAccruedInterestSagaEntityTab.FindAll(_scheduledPostAccruedInterestSagaEntityTab.ApplicationGuid == Id).Single();
                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });
            }

            var caScotiaMocksEnabled = _serviceConfigTab.FindAll(_serviceConfigTab.Key == "Mocks.ScotiaEnabled").SingleOrDefault();

            if (Config.AUT == AUT.Ca && Boolean.Parse(caScotiaMocksEnabled.Value))
            {
                ScotiaResponseBuilder.New().
                                ForBankAccountNumber(BankAccountNumber).
                                Reject();
            }

            var ftl = _fixedTermLoanSagaEntityTab.FindAll(_fixedTermLoanSagaEntityTab.ApplicationGuid == Id).Single();
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = ftl.Id });
            _fixedTermLoanSagaEntityTab.Update(ftl);

            if (Config.AUT != AUT.Ca || (Config.AUT == AUT.Ca && !Boolean.Parse(caScotiaMocksEnabled.Value)))
            {
                var sp = Do.Until(() => _scheduledPaymentSagaEntityTab.FindAll(_scheduledPaymentSagaEntityTab.ApplicationGuid == Id).Single());
                Drive.Msmq.Payments.Send(Config.AUT == AUT.Uk ? new TakePaymentFailedMessage { SagaId = sp.Id, CreatedOn = DateTime.UtcNow, ValueDate = DateTime.UtcNow, PaymentCardId = this.GetCustomer().GetPaymentCard() } :
                                                                new TakePaymentFailedMessage { SagaId = sp.Id, CreatedOn = DateTime.UtcNow, ValueDate = DateTime.UtcNow });

                Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = sp.Id }); 
            }

            Do.Until(() => _arrearsTab.FindAll(_arrearsTab.ApplicationId == application.ApplicationId).Single());

			var arrearsDate = (DateTime)(Drive.Data.Payments.Db.Arrears.FindByApplicationId(application.ApplicationId)).CreatedOn;
			Drive.Data.Payments.Db.Arrears.UpdateByApplicationId(ApplicationId: application.ApplicationId, CreatedOn: new DateTime(arrearsDate.Year, arrearsDate.Month, arrearsDate.Day));
			
			return this;
		}

		public virtual bool IsInArrears()
		{
			var appId = ((ApplicationEntity)Drive.Data.Payments.Db.Applications.FindByExternalId(Id)).ApplicationId;
			return (Drive.Data.Payments.Db.Arrears.FindByApplicationId(appId)) != null;
		}

		public uint GetDaysInArrears()
		{
			if (IsInArrears() == false)
			{
				return 0;
			}

			var appId = ((ApplicationEntity)Drive.Data.Payments.Db.Applications.FindByExternalId(Id)).ApplicationId;
			var arrearDate = (DateTime)(Drive.Data.Payments.Db.Arrears.FindByApplicationId(appId)).CreatedOn;

			return (uint)(DateTime.Today - arrearDate).Days;
		}

		#endregion


		public Application CreateRepaymentArrangement()
		{
			var cmd = new CreateRepaymentArrangementCommand
			{
				ApplicationId = Id,
				Frequency = PaymentFrequencyEnum.Every4Weeks,
				RepaymentDates = new[] { DateTime.Today.AddDays(1), DateTime.Today.AddMonths(1) },
				NumberOfMonths = 2
			};
			Drive.Api.Commands.Post(cmd);

            var dbApplication = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();
            Do.Until(() => _repaymentArrangementsTab.FindAll(_repaymentArrangementsTab.ApplicationId == dbApplication.ApplicationId).Single());

			return this;
		}

        public Application CancelRepaymentArrangement()
        {
            var dbApplication = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();
            var repaymentArrangement = Do.Until(() => _repaymentArrangementsTab.FindAll(_repaymentArrangementsTab.ApplicationId == dbApplication.ApplicationId).Single());
            Drive.Cs.Commands.Post(new CancelRepaymentArrangementCommand()
            {
                RepaymentArrangementId = repaymentArrangement.ExternalId
            });

            Do.Until(() => Drive.Data.Payments.Db.RepaymentArrangements.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).CanceledOn != null);
            return this;
        }

		public Decimal GetBalance()
		{
		    var appId = _applicationsTab.FindAll(_applicationsTab.ExternalId == Id).Single();
		    var transactions = _transactionsTab.FindAll(_transactionsTab.ApplicationId == appId.ApplicationId);
		    decimal total = 0;

		    foreach (var amount in transactions)
		    {
                if (amount.Scope != (decimal)PaymentTransactionScopeEnum.Other)
		        {
                    total += amount.Amount;    
		        }
		    }

		    return total;
		}

        public Decimal GetDueDateBalance()
        {
            var query = Config.AUT == AUT.Za ? (ApiRequest)
                new GetFixedTermLoanApplicationZaQuery { ApplicationId = Id } :
                new GetFixedTermLoanApplicationQuery { ApplicationId = Id };
            return Convert.ToDecimal(Drive.Api.Queries.Post(query).Values["BalanceNextDueDate"].Single());
        }

        public Decimal GetBalanceToday()
        {
            var query = Config.AUT == AUT.Za ? (ApiRequest)
                new GetFixedTermLoanApplicationZaQuery { ApplicationId = Id } :
                new GetFixedTermLoanApplicationQuery { ApplicationId = Id };
            return Convert.ToDecimal(Drive.Api.Queries.Post(query).Values["BalanceToday"].Single());
        }

		public Application RepayEarly(decimal amount, int dayOfLoanToMakeRepayment)
		{
            ApplicationOperations.RewindToDayOfLoanTerm(Id, dayOfLoanToMakeRepayment);

			Guid repaymentRequestId = Guid.NewGuid();

			Drive.Msmq.Payments.Send(new RepayLoanInternalViaBank
			{
				Amount = amount,
				ApplicationId = Id,
				CashEntityId = BankAccountId,
				RepaymentRequestId = repaymentRequestId
			});

            if (!BankGatwayTakePaymentResponseIsMocked())
			{
				var utcNow = DateTime.UtcNow;

				Int32 applicationid =
					Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).ApplicationId;

				RepaymentSagaEntity sp = Do.Until(() => Drive.Db.OpsSagas.RepaymentSagaEntities.Single(s => s.ApplicationId == applicationid));
                Drive.Msmq.Payments.Send(new PaymentTakenMessage { SagaId = sp.Id, ValueDate = utcNow, CreatedOn = utcNow, ApplicationId = Id });
				Do.While(sp.Refresh);
			}

            var transaction = WaitForDirectBankPaymentCreditTransaction();

            // Not sure this should be a part of this method 
            TimeoutCloseApplicationSaga(transaction);

			return this;
		}

	    public Application MoveToDebtCollectionAgency()
        {
            ServiceConfigurationEntity serviceConfiguration =  Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Payments.MoveToDcaDelayInMinutes");

            int daysToRewind = int.Parse(serviceConfiguration.Value)/60/24;

            Drive.Db.Rewind(Id, daysToRewind);

            var entity = Do.Until(() => Drive.Db.OpsSagasCa.ExternalDebtCollectionSagaEntities.Single(e => e.ApplicationId == Id));
            Drive.Msmq.Payments.Send(new TimeoutMessage { SagaId = entity.Id });

            Do.Until(() => Drive.Db.Payments.DebtCollections.Single(d => d.ApplicationEntity.ExternalId == Id && d.MovedToAgency));

            return this;
        }

        #region "Methods for testing my account scenarios"

        // Move NextDueDate before range of days where loan extension is permitted. Useful for testing GetAccountOptions.
        public void NextDueDateTooEarlyToExtendLoan()
        {
            var cfg = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Payments.ExtendLoanDaysBeforeDueDate");
            this.UpdateNextDueDate(int.Parse(cfg.Value) + 3);
        }

        /// Move NextDueDate into range of days where extensions are permitted. Useful for testing GetAccountOptions.
        public void NextDueNotTooEarlyToExtendLoan()
        {
            var cfg = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Payments.ExtendLoanDaysBeforeDueDate");
            this.UpdateNextDueDate(int.Parse(cfg.Value) - 3);
        }
	    
        #endregion

        public void MoveTransactionDates(int days)
        {
            var span = new TimeSpan(days, 0, 0, 0);
            ApplicationEntity application = Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id);
            Drive.Db.MoveApplicationTransactionDates(application, span);
        }
    }
}

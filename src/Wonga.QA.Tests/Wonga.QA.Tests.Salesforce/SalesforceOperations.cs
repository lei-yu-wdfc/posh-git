using System;
using System.Linq;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Old;


namespace Wonga.QA.Tests.Salesforce
{
    public class SalesforceOperations
    {
        private static ServiceConfigurationEntity _sfUsername;
        private static ServiceConfigurationEntity _sfPassword;
        private static ServiceConfigurationEntity _sfUrl;
        private static Framework.ThirdParties.Salesforce _sales;
        private static readonly dynamic ApplicationRepo = Drive.Data.Payments.Db.Applications;
        private static readonly dynamic LoanDueDateNotifiSagaEntityTab = Drive.Data.OpsSagas.Db.LoanDueDateNotificationSagaEntity;
        private static readonly dynamic FixedTermLoanAppTab = Drive.Data.Payments.Db.FixedTermLoanApplications;
        private static readonly dynamic RepaymentArrangementsTab = Drive.Data.Payments.Db.RepaymentArrangements;
        private static readonly dynamic CommsSuppressionsRepo = Drive.Data.Comms.Db.Suppressions;
        private static readonly dynamic PaymentsSuppressionsRepo = Drive.Data.Payments.Db.PaymentCollectionSuppressions;


        public static Framework.ThirdParties.Salesforce SalesforceSetup()
        {
            _sfUsername = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.UserName");
            _sfPassword = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Password");
            _sfUrl = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Url");
            _sales = Drive.ThirdParties.Salesforce;
            _sales.SalesforceUsername = _sfUsername.Value;
            _sales.SalesforcePassword = _sfPassword.Value;
            _sales.SalesforceUrl = _sfUrl.Value;
            return _sales;
        }

        public static Application CreateApplication(Customer customer)
        {

            var application = ApplicationBuilder.New(customer)
             .Build();

            //wait for the payment to customer to be sent out
            Do.Until(() => ApplicationRepo.FindAll(ApplicationRepo.ExternalId == application.Id &&
                                                   ApplicationRepo.Transaction.ApplicationId == ApplicationRepo.Id &&
                                                   ApplicationRepo.Type == "CashAdvance"));

            Do.With.Timeout(3).Until(() =>
            {
                var app = _sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null &&
                       app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
            });

            return application;
        }

        public static void CheckSalesApplicationStatus(Application application, double status)
        {
            Do.With.Timeout(3).Until(() =>
            {
                var app = _sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == status;
            });
        }

        public static void CheckPreviousStatus(Guid id,string previousStatus,string currentStatus)
        {

            Do.With.Timeout(1).Until(()=>  Drive.Data.BiCustomerManagement.Db.ApplicationStatusHistory.FindBy(ApplicationId: id,
                                                                                   PreviousStatus: previousStatus,
                                                                                   CurrentStatus: currentStatus) );
        }

        public static void MakeDueToday(dynamic application)
        {
            var ldd = LoanDueDateNotifiSagaEntityTab.FindAll(LoanDueDateNotifiSagaEntityTab.ApplicationId == application.Id).Single();
            Drive.Msmq.Payments.Send(new Framework.Msmq.TimeoutMessage { SagaId = ldd.Id });
            LoanDueDateNotifiSagaEntityTab.Update(ldd);
        }

        public static void RewindDatesToMakeDueToday(Application application)
        {
            int id = ApplicationOperations.GetAppInternalId(application);
            TimeSpan span = FixedTermLoanAppTab.FindByApplicationId(id).NextDueDate - DateTime.Today;
            application.RewindApplicationDates(span);
        }

        public static void CreateRepaymentArrangement(Customer customer,Application application)
        {
            var arrangementDetails = new[]
										{
											new ArrangementDetail
												{Amount = 49.01m, Currency = CurrencyCodeIso4217Enum.GBP, DueDate = DateTime.Today},
											new ArrangementDetail
												{
													Amount = 51.01m,
													Currency = CurrencyCodeIso4217Enum.GBP,
													DueDate = DateTime.Today.AddDays(7)
												}
										};
            Drive.Cs.Commands.Post(new CreateRepaymentArrangementCommand
            {
                AccountId = customer.Id,
                ApplicationId = application.Id,
                ArrangementDetails = arrangementDetails,
                EffectiveBalance = 100,
                RepaymentAmount = 100
            });
            var dbApplication = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
            Do.Until(() => Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == dbApplication.ApplicationId));
            CheckSupressionTable(application,1);
        }

        public static void CancelRepaymnetArrangement(Application application)
        {
            var dbApplication = ApplicationRepo.FindAll(ApplicationRepo.ExternalId == application.Id).Single();
            var repaymentArrangement = Do.Until(() => RepaymentArrangementsTab.FindAll(RepaymentArrangementsTab.ApplicationId == dbApplication.ApplicationId).Single());
            Drive.Cs.Commands.Post(new CancelRepaymentArrangementCommand()
            {
                RepaymentArrangementId = repaymentArrangement.ExternalId
            });

            Do.Until(() => Drive.Data.Payments.Db.RepaymentArrangements.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId).CanceledOn != null);
            CheckSupressionTable(application, 0);
        }

        private static void CheckSupressionTable(Application application,int value)
        {
            int appInternalId = ApplicationOperations.GetAppInternalId(application);
            Do.Until(() => CommsSuppressionsRepo.FindAll(
                           CommsSuppressionsRepo.AccountId == application.AccountId && CommsSuppressionsRepo.RepaymentArrangement == value).Single());
            Do.Until(() => PaymentsSuppressionsRepo.FindAll(
                PaymentsSuppressionsRepo.ApplicationId == appInternalId && PaymentsSuppressionsRepo.RepaymentArrangementSuppression == value).Single());
        }

        public class ArrangementDetail
        {
            public decimal Amount { get; set; }
            public CurrencyCodeIso4217Enum Currency { get; set; }
            public DateTime DueDate { get; set; }
        }

    }
}

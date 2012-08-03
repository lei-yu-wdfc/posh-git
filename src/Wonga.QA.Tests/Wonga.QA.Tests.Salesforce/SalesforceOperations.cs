using System;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;

namespace Wonga.QA.Tests.Salesforce
{
    class SalesforceOperations
    {
        private static ServiceConfigurationEntity _sfUsername;
        private static ServiceConfigurationEntity _sfPassword;
        private static ServiceConfigurationEntity _sfUrl;
        private static Framework.ThirdParties.Salesforce _sales;
        private static readonly dynamic _applicationRepo = Drive.Data.Payments.Db.Applications;

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
            Do.Until(() => _applicationRepo.FindAll(_applicationRepo.ExternalId == application.Id &&
                                                   _applicationRepo.Transaction.ApplicationId == _applicationRepo.Id &&
                                                   _applicationRepo.Type == "CashAdvance"));

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

    }
}

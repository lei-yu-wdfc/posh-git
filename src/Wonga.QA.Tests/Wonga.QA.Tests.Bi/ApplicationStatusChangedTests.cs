using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    [AUT(AUT.Uk)]
    public class ApplicationStatusChangedTests
    {
        [Test]
        [AUT(AUT.Uk), JIRA("UK-819")]
        [Description("Verifies that after funds have been transferred to the customer application status 'Live' will be set in salesforce")]
        public void FundsTransferred_SubmitsApplicactionStatusLive_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).Build();

            //wait for the payment to customer to be sent out
            Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id)
                               .Transactions.Where(
                                   t => t.Amount == loanAmount
                                        && t.Type == "CashAdvance"));
            ServiceConfigurationEntity sfUsername = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
            ServiceConfigurationEntity sfPassword = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
            ServiceConfigurationEntity sfUrl = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");
            var sales = Drive.ThirdParties.Salesforce;
            sales.SalesforceUsername = sfUsername.Value;
            sales.SalesforcePassword = sfPassword.Value;
            sales.SalesforceUrl = sfUrl.Value;
            Do.Until(() =>
                         {
                             var app = sales.GetApplicationById(application.Id);
                             return app.Status_ID__c != null && app.Status_ID__c == (double)Framework.ThirdParties.Salesforce.ApplicationStatus.Live;
                         });
        }
    }
}
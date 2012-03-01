using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    public class ApplicationStatusChangedTests
    {
        [Test, AUT(AUT.Uk)]
        public void FundsTransferred_SubmitsApplicactionStatusLive_ToSalesforce()
        {
            Customer customer = CustomerBuilder.New().Build();
            const decimal loanAmount = 222.22m;
            Application application = ApplicationBuilder.New(customer).WithLoanAmount(loanAmount).Build();

            //wait for the payment to customer to be sent out
            Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == application.Id)
                               .Transactions.Where(
                                   t => t.Amount == loanAmount
                                        && t.Type == "CashAdvance"));
            ServiceConfigurationEntity sfUsername = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.UserName");
            ServiceConfigurationEntity sfPassword = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Password");
            ServiceConfigurationEntity sfUrl = Driver.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == "Salesforce.Url");
            var sales = Driver.ThirdParties.Salesforce;
            sales.SalesforceUsername = sfUsername.Value;
            sales.SalesforcePassword = sfPassword.Value;
            sales.SalesforceUrl = sfUrl.Value;
            Do.Until(() =>
                         {
                             var app = sales.GetApplicationById(application.Id);
                             return app != null && app.Status__c == "Live";
                         });



        }
    }
}
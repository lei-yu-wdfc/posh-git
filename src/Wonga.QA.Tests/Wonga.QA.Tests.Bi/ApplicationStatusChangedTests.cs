using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Bi
{
    [TestFixture]
    public class ApplicationStatusChangedTests
    {
        [Test, AUT(AUT.Uk)]
        [Ignore]
        public void FundsTransferred_SubmitsApplicactionStatusChange_ToSalesforce()
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
            Driver.ThirdParties.Salesforce.SalesforceUsername = sfUsername.Value;
            Driver.ThirdParties.Salesforce.SalesforcePassword = sfPassword.Value;
            Driver.ThirdParties.Salesforce.SalesforceUrl = sfUrl.Value;
            Do.Until(() => Driver.ThirdParties.Salesforce.ApplicationExists(application.Id));

        }
    }
}
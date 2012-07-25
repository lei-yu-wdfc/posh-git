using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.ThirdParties;
using Wonga.QA.Tests.Core;


namespace Wonga.QA.Tests.Payments.Command
{
    [TestFixture]
    [Parallelizable(TestScope.All)]
    [AUT(AUT.Uk)]
    [Pending("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
    public class CsReportRefundRequestTests
    {
        [Test]
        [AUT(AUT.Uk), JIRA("UKOPS-163")]
        public void ReportRefundCommand_Changes_SalesforceApplicationStatus_To_Refund()
        {
            Guid caseId = Guid.NewGuid();
            Customer customer = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(customer).Build();
            var command = new CsReportRefundRequestCommand()
                                                       {
                                                           ApplicationId = application.Id,
                                                           CaseId = caseId,
                                                       };
            Drive.Cs.Commands.Post(command);

            var sfUsername = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.UserName");
            var sfPassword = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Password");
            var sfUrl = Drive.Data.Ops.Db.ServiceConfigurations.FindByKey("Salesforce.Url");
            var sales = Drive.ThirdParties.Salesforce;
            sales.SalesforceUsername = sfUsername.Value;
            sales.SalesforcePassword = sfPassword.Value;
            sales.SalesforceUrl = sfUrl.Value;


            // wait until sales force moves to Refund
            Do.Until(() =>
            {
                var app = sales.GetApplicationById(application.Id);
                return app.Status_ID__c != null && app.Status_ID__c == (double)Framework
                    .ThirdParties.Salesforce.ApplicationStatus.Refund;
            });
        }
    }
}
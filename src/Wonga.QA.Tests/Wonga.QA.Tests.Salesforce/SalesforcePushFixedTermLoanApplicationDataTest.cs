﻿using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Salesforce;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Salesforce
{
    [TestFixture, Parallelizable(TestScope.All), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
	public class SalesforcePushFixedTermLoanApplicationDataTest : SalesforceTestBase
	{
		[Test]
        [AUT(AUT.Uk), JIRA("UK-808"), Ignore("SF tests are failing because message congestion in SF TC queue, explicit until fixed")]
		[Description("Verify that the application data has been pushed to SF")]
		public void FixedTermLoanApplicationDataPushedToSalesforce()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			var app = ApplicationBuilder.New(customer).Build();

			Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() => Salesforce.ApplicationExists(app.Id));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2459"), Pending("ZA-2565")]
        public void PushTest_AccountCreateLaterThanApplicationMessage()
        {
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);

            //Remove salesforce account record to simulate account is not created yet.
            
            var db = new DbDriver();
            var salesForceAccount = Do.Until(() => db.Salesforce.SalesforceAccounts.Single(a => a.AccountId == customer.Id));
            db.Salesforce.SalesforceAccounts.DeleteOnSubmit(salesForceAccount);
            db.Salesforce.SubmitChanges();

            var app = ApplicationBuilder.New(customer).Build();
            var saga = Do.Until(() => db.OpsSagas.SaveFixedTermLoanApplicationEntities.Single(se => se.AccountId == customer.Id));

            db.Salesforce.SalesforceAccounts.InsertOnSubmit(new SalesforceAccountEntity()
                                                                {
                                                                    AccountId = customer.Id,
                                                                    SalesforceId = salesForceAccount.SalesforceId,
                                                                });
            db.Salesforce.SubmitChanges();
            Drive.Msmq.Salesforce.Send(new SaveCustomerDetailsToSalesforceZaCommand
                                           {
                                               AccountId = customer.Id
                                           });

            Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() => Salesforce.ApplicationExists(app.Id));          
        }

		[Test, AUT(AUT.Ca), JIRA("ZA-2459")]
		public void PushTest_AccountCreateLaterThanApplicationMessageCa()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);

			//Remove salesforce account record to simulate account is not created yet.

			var db = new DbDriver();
			var salesForceAccount = Do.Until(() => db.Salesforce.SalesforceAccounts.Single(a => a.AccountId == customer.Id));
			db.Salesforce.SalesforceAccounts.DeleteOnSubmit(salesForceAccount);
			db.Salesforce.SubmitChanges();

			var app = ApplicationBuilder.New(customer).Build();
			var saga = Do.Until(() => db.OpsSagas.SaveFixedTermLoanApplicationEntities.Single(se => se.AccountId == customer.Id));

			db.Salesforce.SalesforceAccounts.InsertOnSubmit(new SalesforceAccountEntity()
			{
				AccountId = customer.Id,
				SalesforceId = salesForceAccount.SalesforceId,
			});
			db.Salesforce.SubmitChanges();
			Drive.Msmq.Salesforce.Send(new SaveCustomerDetailsToSalesforceCaCommand
			{
				AccountId = customer.Id
			});

			Do.With.Timeout(TimeSpan.FromSeconds(5)).Until(() => Salesforce.ApplicationExists(app.Id));
		}
	}
}

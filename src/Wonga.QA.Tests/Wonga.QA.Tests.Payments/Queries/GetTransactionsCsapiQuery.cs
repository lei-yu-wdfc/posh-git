using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries;
using Wonga.QA.Framework.Helpers;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments.Queries
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class GetTransactionsCsapiQuery
    {
        private dynamic _transactions = Drive.Data.Payments.Db.Transactions;

		[Test, AUT(AUT.Za), JIRA("ZA-2227")]
		public void GetTransactions_ShouldOnlyShowServiceFeeTransactionsPostedTillPostingDate()
		{
			//Arrange
			var customer = CustomerBuilder.New().Build();
			var app = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

			Do.Until(() => _transactions.GetCount(_transactions.Applications.ExternalId == app.Id));

			var query = new GetTransactionsQuery
			{
				ApplicationGuid = app.Id
			};

			//Act
			var response = Drive.Cs.Queries.Post(query);

			Dictionary<PaymentTransactionType, int> retrievedTransactionTypes = new Dictionary<PaymentTransactionType, int>();

			XmlDocument _doc = new XmlDocument();
			_doc.LoadXml(response.Body.ToString());

			XmlNodeList xmlNodeList = _doc.GetElementsByTagName("Type");

			foreach (XmlElement element in xmlNodeList)
			{
				PaymentTransactionType transactionType;
				Enum.TryParse<PaymentTransactionType>(element.InnerText, true, out transactionType);

				if (retrievedTransactionTypes.ContainsKey(transactionType))
				{
					retrievedTransactionTypes[transactionType]++;
				}
				else
				{
					retrievedTransactionTypes[transactionType] = 1;
				}
			}

			//Assert
			Assert.AreEqual(1, retrievedTransactionTypes[PaymentTransactionType.ServiceFee]);
		}

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldReturnAllTransactionsWhenThereAreTransactionsForAGivenApplication()
        {
            var customer = CustomerBuilder.New().Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            Do.Until(() => Drive.Db.Payments.Transactions.Count(t => t.ApplicationEntity.ExternalId == app.Id));

            var query = new GetTransactionsQuery
                            {
                                ApplicationGuid = app.Id
                            };

            var response = Drive.Cs.Queries.Post(query);

            Assert.IsNotNull(response);
            Assert.Contains(response.Values["Type"], "Fee");
            Assert.Contains(response.Values["Type"], "CashAdvance");
            Assert.Contains(response.Values["Type"], "Interest");
        }

        [Test, AUT(AUT.Wb), JIRA("SME-375")]
        public void PaymentsShouldReturnNoTransactionsWhenThereAreNoTransactionsForAGivenApplication()
        {
            var customer = CustomerBuilder.New().WithMiddleName("FailingName").Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            var app = ApplicationBuilder.New(customer, organisation).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

            var query = new GetTransactionsQuery
            {
                ApplicationGuid = app.Id
            };

            var response = Drive.Cs.Queries.Post(query);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Root.Descendants().Count(d => d.Name.LocalName == "Transaction"));
        }
    }
}

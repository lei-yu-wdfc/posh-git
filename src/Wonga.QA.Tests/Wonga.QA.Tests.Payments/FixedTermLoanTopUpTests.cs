using System;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;

namespace Wonga.QA.Tests.Payments
{
	public class FixedTermLoanTopUpTests
	{
		[Test, AUT(AUT.Uk), JIRA("UK-789")]
		public void GetFixedTermLoanTopUpOfferTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			var response = Driver.Api.Queries.Post(new GetFixedTermLoanTopupOfferQuery {AccountId = customer.Id});
			Assert.GreaterThan(decimal.Parse(response.Values["AmountMax"].Single()), 0);
			Assert.GreaterThan(int.Parse(response.Values["DaysTillRepaymentDate"].Single()), 0);
		}

		[Test, AUT(AUT.Uk), JIRA("UK-789")]
		public void GetFixedTermLoanTopupCalculationTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			var response = Driver.Api.Queries.Post(new GetFixedTermLoanTopupCalculationQuery
			                                       	{
			                                       		ApplicationId = application.Id,
			                                       		TopupAmount = 100
			                                       	});

			Assert.AreEqual(115.91M, decimal.Parse(response.Values["TotalRepayable"].Single()));
			Assert.AreEqual(15.91M, decimal.Parse(response.Values["InterestAndFeesAmount"].Single()));
		}

		[Test, AUT(AUT.Uk), JIRA("UK-789")]
		public void CreateFixedTermLoanTopUpTest()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			Driver.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
			                         	{
			                         		AccountId = customer.Id,
			                         		ApplicationId = application.Id,
											FixedTermLoanTopupId = Guid.NewGuid(),
			                         		TopupAmount = 150
			                         	});
			var app = Driver.Db.Payments.Applications.Single(x => x.ExternalId == application.Id);
			Assert.IsNotNull(Do.Until(() => Driver.Db.Payments.Topups.Single(x => x.ApplicationId == app.ApplicationId)));
		}

		[Test, AUT(AUT.Uk), JIRA("UK-789")]
		public void SignFixedTermLoanTopup()
		{
			Customer customer = CustomerBuilder.New().Build();
			Do.Until(customer.GetBankAccount);
			Do.Until(customer.GetPaymentCard);
			Application application = ApplicationBuilder.New(customer).Build();

			Driver.Api.Commands.Post(new CreateFixedTermLoanTopupCommand
			                         	{
			                         		AccountId = customer.Id,
			                         		ApplicationId = application.Id,
			                         		FixedTermLoanTopupId = Guid.NewGuid(),
			                         		TopupAmount = 150
			                         	});

			var app = Driver.Db.Payments.Applications.Single(x => x.ExternalId == application.Id);
			var topUp = Driver.Db.Payments.Topups.Single(x => x.ApplicationId == app.ApplicationId);
			Assert.IsNotNull(topUp);

			Driver.Api.Commands.Post(new SignFixedTermLoanTopupCommand
			                         	{
			                         		AccountId = customer.Id,
			                         		FixedTermLoanTopupId = topUp.ExternalId
			                         	});
            Thread.Sleep(10000);
			Do.Until(topUp.Refresh);

			Assert.IsNotNull(topUp.SignedOn);
			
			Assert.AreEqual(2, Do.Until(() => Driver.Db.Payments.Transactions.Where(
					x => x.ApplicationId == app.ApplicationId && x.ComponentTransactionId == topUp.ExternalId)).Count());
		}
	}
}

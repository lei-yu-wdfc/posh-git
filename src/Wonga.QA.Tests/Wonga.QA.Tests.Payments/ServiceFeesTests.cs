using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture, Parallelizable(TestScope.Descendants)]
	public class ServiceFeesTests
	{
		private const string NowServiceConfigKey =
			"Wonga.Payments.Validators.Za.CreateFixedTermLoanApplicationValidator.DateTime.UtcNow";

		private Application _application;

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			SetUtcNow(NowServiceConfigKey, new DateTime(2012, 03, 15, 13, 14, 15));

			Customer customer = CustomerBuilder.New().Build();
			_application =
				ApplicationBuilder.New(customer)
					.WithLoanAmount(100m)
					.WithLoanTerm(40)
					.Build();
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			var db = new DbDriver();
			var paymentsNowDb = db.Ops.ServiceConfigurations.Single(a => a.Key == NowServiceConfigKey);
			db.Ops.ServiceConfigurations.DeleteOnSubmit(paymentsNowDb);
			db.Ops.SubmitChanges();
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1969")]
		public void AllServiceFeesArePostedUpfrontOnApplication()
		{
			//Customer customer = CustomerBuilder.New().Build();
			//Application application = ApplicationBuilder.New(customer).WithLoanTerm(10).Build();
			PaymentsDatabase paymentsDatabase = Drive.Db.Payments;

			var applicationEntity = paymentsDatabase.Applications.Single(a => a.ExternalId == _application.Id);
			var serviceFees =
				paymentsDatabase.Transactions
					.Where(t =>
					       t.ApplicationId == applicationEntity.ApplicationId &&
					       t.Type == "ServiceFee")
					.OrderBy(t => t.PostedOn)
					.ToList();
			var cashAdvance =
				paymentsDatabase.Transactions
					.Single(t =>
					       t.ApplicationId == applicationEntity.ApplicationId &&
					       t.Type == "CashAdvance");

			Assert.AreEqual(4, serviceFees.Count);
			Assert.AreApproximatelyEqual(
				cashAdvance.PostedOn, serviceFees[0].PostedOn, TimeSpan.FromMinutes(10));
			Assert.AreApproximatelyEqual(
			    cashAdvance.PostedOn.AddDays(30), serviceFees[1].PostedOn, TimeSpan.FromMinutes(10));
			Assert.AreApproximatelyEqual(
			    cashAdvance.PostedOn.AddDays(60), serviceFees[2].PostedOn, TimeSpan.FromMinutes(10));
			Assert.AreApproximatelyEqual(
			    cashAdvance.PostedOn.AddDays(90), serviceFees[3].PostedOn, TimeSpan.FromMinutes(10));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1969", "ZA-2193")]
		public void SmallLoanHasPositiveBalanceAfterApplication()
		{
			//Customer customer = CustomerBuilder.New().Build();
			//Application application =
			//    ApplicationBuilder.New(customer)
			//        .WithLoanAmount(100m)
			//        .WithLoanTerm(10)
			//        .Build();

			var query =
				new GetFixedTermLoanApplicationZaQuery {ApplicationId = _application.Id};
			var response = Drive.Api.Queries.Post(query);

			Assert.AreEqual("174.10", response.Values["BalanceToday"].Single());
			Assert.AreEqual("92.27", response.Values["Fees"].Single());
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1969", "ZA-2193")]
		public void DueAmountObeysInDuplumRule()
		{
			//Customer customer = CustomerBuilder.New().Build();
			//Application application =
			//    ApplicationBuilder.New(customer)
			//        .WithLoanAmount(100m)
			//        .WithLoanTerm(40)
			//        .Build();

			var query =
				new GetFixedTermLoanApplicationZaQuery {ApplicationId = _application.Id};
			var response = Drive.Api.Queries.Post(query);

			Assert.AreEqual("200.00", response.Values["BalanceNextDueDate"].Single());

			//17.1 + 57 + 18.17
			//18.17 calculated as follow: 57 - (getBal -loanCap)
			Assert.AreEqual("92.27", response.Values["Fees"].Single()); 
		}

		private void SetUtcNow(string nowServiceConfigKey, DateTime dateTime)
		{
			Drive.Db.SetServiceConfiguration(nowServiceConfigKey, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}
}

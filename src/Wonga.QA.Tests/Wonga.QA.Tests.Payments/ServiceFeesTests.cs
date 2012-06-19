﻿using System;
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
	public class ServiceFeesTests
	{
		private const string NowServiceConfigKey =
			"Wonga.Payments.Validators.Za.CreateFixedTermLoanApplicationValidator.DateTime.UtcNow";

		[FixtureSetUp]
		public void FixtureSetUp()
		{
			SetUtcNow(NowServiceConfigKey, new DateTime(2012, 03, 15, 13, 14, 15));
		}

		[FixtureTearDown]
		public void FixtureTearDown()
		{
			var db = new DbDriver();
			var paymentsNowDb = db.Ops.ServiceConfigurations.Single(a => a.Key == NowServiceConfigKey);
			db.Ops.ServiceConfigurations.DeleteOnSubmit(paymentsNowDb);
			db.Ops.SubmitChanges();
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1969", "ZA-2193"), Parallelizable]
		public void SmallLoanHasPositiveBalanceAfterApplication()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application =
				ApplicationBuilder.New(customer)
					.WithLoanAmount(100m)
					.WithLoanTerm(10)
					.Build();

			var query =
				new GetFixedTermLoanApplicationZaQuery { ApplicationId = application.Id };
			var response = Drive.Api.Queries.Post(query);

			Assert.AreEqual("174.10", response.Values["BalanceToday"].Single());
			Assert.AreEqual("74.10", response.Values["Fees"].Single());
		}

		[Test, AUT(AUT.Za), JIRA("ZA-1969", "ZA-2193"), Parallelizable]
		public void DueAmountObeysInDuplumRule()
		{
			Customer customer = CustomerBuilder.New().Build();
			Application application =
				ApplicationBuilder.New(customer)
					.WithLoanAmount(100m)
					.WithLoanTerm(40)
					.Build();

			var query =
				new GetFixedTermLoanApplicationZaQuery { ApplicationId = application.Id };
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
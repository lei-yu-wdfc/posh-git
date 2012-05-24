using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.TransUnion
{
	[TestFixture, Parallelizable(TestScope.All), Pending("ZA-2565")]
	class BureauResponseCachingTests
	{
		private const RiskMask TestMask = RiskMask.TESTCustomerNameIsCorrect;
		private const int ResponseDecayDays = 30;

		private Customer customer;

		[Test, AUT(AUT.Za), JIRA("ZA-2400"), Pending("ZA-2565")]
		public void BureauResponseSavedToDatabase()
		{
			customer = CustomerBuilder.New().WithEmployer(TestMask).Build();
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			dynamic transunionResponse = Drive.Data.TransUnion.Db.TransunionResponse.FindByAccountId(customer.Id);

			Assert.IsNotNull(transunionResponse);
			Assert.AreEqual(customer.Id, (Guid)transunionResponse.AccountId);
			Assert.AreEqual(GetBureauResponseCount(customer), 1);
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2400"), DependsOn("BureauResponseSavedToDatabase"), Pending("ZA-2565")]
		public void BureauResponseNotSavedBeforePreviousResponseDecays()
		{
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			Assert.AreEqual(1, GetBureauResponseCount(customer));
		}

		[Test, AUT(AUT.Za), JIRA("ZA-2400"), DependsOn("BureauResponseNotSavedBeforePreviousResponseDecays"), Pending("ZA-2565")]
		public void BureauResponseCreatedAfterPreviousResponseDecays()
		{
			RewindBureauResponseCreatedOn(customer, ResponseDecayDays + 1);

			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();

			Assert.AreEqual(2, GetBureauResponseCount(customer));
		}

		#region Helpers

		private void RewindBureauResponseCreatedOn(Customer customer, uint days)
		{
			var creationDate = GetBureauResponseCreationDate(customer).AddDays(-days);
			Drive.Data.TransUnion.Db.TransunionResponse.UpdateByAccountId(AccountId: customer.Id, CreationDate: creationDate);
		}

		private DateTime GetBureauResponseCreationDate(Customer customer)
		{
			return (DateTime)(Drive.Data.TransUnion.Db.TransunionResponse.FindByAccountId(customer.Id).CreationDate);
		}

		private int GetBureauResponseCount(Customer customer)
		{
			var responses = Drive.Data.TransUnion.Db.TransunionResponse.FindAllByAccountId(customer.Id).ToList<dynamic>();

			return responses.Count;
		}

		#endregion
	}
}

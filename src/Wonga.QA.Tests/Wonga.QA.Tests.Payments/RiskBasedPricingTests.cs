using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
	[TestFixture]
	public class RiskBasedPricingTests
	{
		[Test, AUT(AUT.Wb), JIRA("SME-889")]
		public void ShouldUpdateApplicationWithCorrectApplicationFeeAndInterestRateFromRiskTier()
		{
			var customer = CustomerBuilder.New().Build();
			var organisation = OrganisationBuilder.New(customer).Build();
			var application = ApplicationBuilder.New(customer, organisation).Build();

			var riskApplicationEntity = Do.Until(() => Drive.Db.Risk.RiskApplications.Single(a => a.RiskBusinessApplicationEntity.RiskApplicationEntity.ApplicationId == application.Id));

			var tierRateMappingEntity = Do.Until(() => Drive.Db.Payments.TierRateMappings.Single(a => a.Tier == riskApplicationEntity.PriceTier));

			Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id 
				&& a.BusinessFixedInstallmentLoanApplicationEntity.ApplicationFee == tierRateMappingEntity.ApplicationFeeRate
				&& a.BusinessFixedInstallmentLoanApplicationEntity.InterestRate == tierRateMappingEntity.WeeklyInterestRate));
		}
	}
}

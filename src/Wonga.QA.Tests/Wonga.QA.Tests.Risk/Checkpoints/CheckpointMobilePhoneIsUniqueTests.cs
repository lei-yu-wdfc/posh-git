using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture, Parallelizable(TestScope.All)]
	public class CheckpointMobilePhoneIsUniqueTests
	{
		private const RiskMask TestMask = RiskMask.TESTMobilePhoneIsUnique;
		private Customer _customer;
		private static readonly string _phoneNumber = Get.GetMobilePhone();

		[Test]
		[JIRA("UK-1563")]
		public void L0_MobilePhoneIsUnique_LoanIsAccepted()
		{
			_customer = CreateCustomerWithVerifiedMobileNumber(_phoneNumber);
			ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
		}

		[Test, DependsOn("L0_MobilePhoneIsUnique_LoanIsAccepted")]
		[JIRA("UK-1563")]
		public void L0_MobilePhoneIsNotUnique_LoanIsDeclined()
		{
			//Create and check new customer
			Customer customer = CreateCustomerWithVerifiedMobileNumber(_phoneNumber);
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

		private static Customer CreateCustomerWithVerifiedMobileNumber(string phoneNumber)
		{
			CustomerBuilder customerBuilder = CustomerBuilder.New().WithMobileNumber(phoneNumber).WithEmployer(TestMask);
			Customer customer = customerBuilder.Build();

			var mobileVerificationEntity = Do.Until(() => Drive.Data.Comms.Db.MobilePhoneVerifications.FindBy(MobilePhone: phoneNumber, AccountId: customerBuilder.Id));

			Assert.IsNotNull(mobileVerificationEntity);
			Assert.IsNotNull(mobileVerificationEntity.Pin);

			//Force the mobile phone number to be verified successfully..
			Assert.DoesNotThrow(() => Drive.Api.Commands.Post(new CompleteMobilePhoneVerificationCommand
			{
				Pin = mobileVerificationEntity.Pin,
				VerificationId = mobileVerificationEntity.VerificationId
			}));

			return customer;
		}
	}
}
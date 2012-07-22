using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture, Parallelizable(TestScope.All), Pending("ZA-2565")]
	public class CheckpointMobilePhoneIsUniqueTests
	{
		private const RiskMask TestMask = RiskMask.TESTMobilePhoneIsUnique;
		private static readonly string PhoneNumber = GetRiskUniqueMobilePhone();

		public static string GetRiskUniqueMobilePhone()
		{
            var phone = Get.GetMobilePhone();
			Drive.Data.Risk.Db.RiskAccountMobilePhones.Delete(MobilePhone: phone); //Dodgy
            return phone; 
        }

        public void CleanPhone(string phone)
        {
            Drive.Data.Risk.Db.RiskAccountMobilePhones.Delete(MobilePhone: phone);
        }

		[Test]
		[JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 1: Accepted"), Category(TestCategories.CoreTest)]
		public void L0_MobilePhoneIsUnique_LoanIsAccepted()
		{
			Customer customer = CreateCustomerWithVerifiedMobileNumber(PhoneNumber);
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
		}

		[Test(Order = 1)]
		[JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 2: Declined")]
		[Ignore("looks like this test is redundant and depends on the previous one")]
		public void L0_MobilePhoneIsUniqueSecondPhoneIsNotValidated_LoanIsDeclined()
		{
			//Create and check new customer
			Customer customer = CreateCustomerWithVerifiedMobileNumber(PhoneNumber);
			ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
		}

        [Test]
		[JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 1, Scenario 2: Declined")]
        public void L0_MobilePhoneIsNotUnique_LoanIsDeclined()
        {
            var phone = GetRiskUniqueMobilePhone();
            try
            {
                //Create previous customer record
                Customer customer1 = CreateCustomerWithVerifiedMobileNumber(phone);
                ApplicationBuilder.New(customer1).Build();

                //Create and check new customer
                Customer customer = CreateCustomerWithVerifiedMobileNumber(phone);
                ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            }
            finally
            {
                CleanPhone(phone);
            }
        }


		private static Customer CreateCustomerWithVerifiedMobileNumber(string phoneNumber)
		{
			CustomerBuilder customerBuilder = CustomerBuilder.New().WithMobileNumber(phoneNumber).WithEmployer(TestMask);
			return customerBuilder.Build();
		}
	}
}

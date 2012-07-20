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
		private Customer _customer;
		private static readonly string _phoneNumber = Get.GetMobilePhone();

		public string GetMobilePhone()
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
			_customer = CreateCustomerWithVerifiedMobileNumber(_phoneNumber);
			ApplicationBuilder.New(_customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
		}

        [Test]
		[JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 1, Scenario 2: Declined")]
        public void L0_MobilePhoneIsNotUnique_LoanIsDeclined()
        {
            var phone = GetMobilePhone();
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

		[Test]
        [JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 2: Accepted")]
		public void L0_MobilePhoneIsUniqueSecondPhoneIsNotValidated_LoanIsAccepted()
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

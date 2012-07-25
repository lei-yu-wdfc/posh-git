using System;
using MbUnit.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
	[TestFixture]
    [Parallelizable(TestScope.All)]
	public class CheckpointMobilePhoneIsUniqueTests
	{
		private const RiskMask TestMask = RiskMask.TESTMobilePhoneIsUnique;     

		[Test]
		[JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 1: Accepted")]//, Category(TestCategories.CoreTest)]
		public void L0_MobilePhoneIsUnique_LoanIsAccepted()
		{
            var phone = Get.GetMobilePhone();
		    try
		    {
                CleanPhone(phone);
                var customer = CustomerBuilder.New().WithMobileNumber(phone).WithEmployer(TestMask).Build();
                ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
		    }
            finally 
		    {
		        CleanPhone(phone);
		    }
		}


        [Test]
        [JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 1, Scenario 2: Declined")]
        public void L0_MobilePhoneIsNotUnique_LoanIsDeclined()
        {
            var phone = Get.GetMobilePhone();

            try
            {
                //Create previous customer record
                var firstCustomer = CustomerBuilder.New().WithMobileNumber(phone).Build();
                ApplicationBuilder.New(firstCustomer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

                //Create and check new customer
                var secendCustomer = CustomerBuilder.New().WithMobileNumber(phone).WithEmployer(TestMask).Build();
                ApplicationBuilder.New(secendCustomer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
            }
            finally
            {
                CleanPhone(phone);
            }
        }


        [Test]
        [JIRA("UK-1563"), AUT(AUT.Uk), Description("Scenario 2: Accepted")]
        public void L0_MobilePhoneIsNotValidatedSecondPhoneIsUnique_LoanIsAccepted()
        {
            var phone = Get.GetMobilePhone();

            try
            {
                var firstCustomer = CustomerBuilder.New().WithMobileNumber(phone).Build();
                ApplicationBuilder.New(firstCustomer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

                CleanVerification(firstCustomer);
                CleanPhone(phone);


                var secendCustomer = CustomerBuilder.New().WithMobileNumber(phone).WithEmployer(TestMask).Build();
                ApplicationBuilder.New(secendCustomer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            }
            finally
            {
                CleanPhone(phone);
            }
        }


        private void CleanPhone(String phone)
        {
            Drive.Data.Risk.Db.RiskAccountMobilePhones.Delete(MobilePhone: phone);
        }

        private void CleanVerification(Customer customer)
        {
            var customerDetailsTable = Drive.Data.Comms.Db.CustomerDetails;
            var mobilePhoneVerificationTable = Drive.Data.Comms.Db.MobilePhoneVerification;

            var customerDetailsEntity = customerDetailsTable.FindAllBy(AccountId: customer.Id).Single();
            customerDetailsEntity.MobilePhone = null;
            customerDetailsTable.Update(customerDetailsEntity);
           
            var mobilePhoneVerificationEntity = mobilePhoneVerificationTable.FindAllBy(AccountId: customer.Id).Single();
            mobilePhoneVerificationEntity.MobileVerifiedOn = null;
            mobilePhoneVerificationTable.Update(mobilePhoneVerificationEntity);
        }
	}
}

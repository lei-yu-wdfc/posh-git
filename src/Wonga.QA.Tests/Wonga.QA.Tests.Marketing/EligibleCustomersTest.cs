using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Marketing
{
    class EligibleCustomersTest

    {
        private Customer _eligibleCustomer = null;
        private Customer _nonEligibleCustomer = null;
        private Customer _nonEligibleCustomerInArrears = null;

        private static readonly dynamic _eligibleCustomersEntity = Drive.Data.Marketing.Db.MarketingEligibleCustomers;

        private static readonly String ELIGIBLE_RESPONSE_KEY = "IsEligible";
        private static readonly String ELIGIBLE_CUSTOMER_RESPONSE = "true";
        private static readonly String NON_ELIGIBLE_CUSTOMER_RESPONSE = "false";

        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().Build();
            _nonEligibleCustomer = CustomerBuilder.New().Build();
            _nonEligibleCustomerInArrears = CustomerBuilder.New().Build();

            Do.Until(()=> _eligibleCustomersEntity.Insert(EligibleCustomerId: _eligibleCustomer.Id, ShowAd: 1, CustomerInArrears: 0,
                                            CreateOn: Get.RandomDate(), HasStandardCard: 1, HasPremiumCard: 0));
            Do.Until(()=> _eligibleCustomersEntity.Insert(EligibleCustomerId: _nonEligibleCustomerInArrears.Id, ShowAd: 0, CustomerInArrears: 1,
                                            CreateOn: Get.RandomDate(), HasStandardCard: 1, HasPremiumCard: 0));

        }


        [Test,AUT(AUT.Uk),JIRA("PP-32")]
        public void ExecuteGetEligibleCustomersQuery()
        {

            GetCustomerPrePaidEligibilityQuery eligibleCustomerMessage = new GetCustomerPrePaidEligibilityQuery
                                                                             {
                                                                                 AccountId = _eligibleCustomer.Id
                                                                             };
            GetCustomerPrePaidEligibilityQuery nonEligibleCustomerMessage = new GetCustomerPrePaidEligibilityQuery
                                                                             {
                                                                                 AccountId = _nonEligibleCustomer.Id
                                                                             };
            GetCustomerPrePaidEligibilityQuery nonEligibleCustomerInArrearsMessage = new GetCustomerPrePaidEligibilityQuery
                                                                             {
                                                                                AccountId = _nonEligibleCustomerInArrears.Id
                                                                             };

            Assert.IsTrue(Drive.Api.Queries.Post(eligibleCustomerMessage).Values[ELIGIBLE_RESPONSE_KEY].First().Equals(ELIGIBLE_CUSTOMER_RESPONSE));
            Assert.IsTrue(Drive.Api.Queries.Post(nonEligibleCustomerInArrearsMessage).Values[ELIGIBLE_RESPONSE_KEY].First().Equals(NON_ELIGIBLE_CUSTOMER_RESPONSE));
            Assert.Throws<ValidatorException>(() => Drive.Api.Queries.Post(nonEligibleCustomerMessage));
            
        }

        [TearDown]
        public void Rollback()
        {
            Do.Until(()=> _eligibleCustomersEntity.Delete(EligibleCustomerId: _eligibleCustomer.Id));
            Do.Until(()=> _eligibleCustomersEntity.Delete(EligibleCustomerId: _nonEligibleCustomerInArrears.Id));
        }


    }
}

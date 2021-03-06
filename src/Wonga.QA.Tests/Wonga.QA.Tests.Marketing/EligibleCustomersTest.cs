﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Exceptions;
using Wonga.QA.Framework.Api.Requests.Marketing.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.OpsSagas;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Marketing
{
    class EligibleCustomersTest

    {
        private Customer _eligibleCustomer = null;
        private Customer _nonEligibleCustomer = null;
        private Customer _nonEligibleCustomerInArrears = null;

        private static readonly String ELIGIBLE_RESPONSE_KEY = "IsEligible";
        private static readonly String ELIGIBLE_CUSTOMER_RESPONSE = "true";
        private static readonly String NON_ELIGIBLE_CUSTOMER_RESPONSE = "false";

        [SetUp]
        public void Init()
        {
            _eligibleCustomer = CustomerBuilder.New().Build();
            _nonEligibleCustomer = CustomerBuilder.New().Build();
            _nonEligibleCustomerInArrears = CustomerBuilder.New().Build();

            CustomerOperations.CreateMarketingEligibility(_eligibleCustomer.Id,true);
            CustomerOperations.CreateMarketingEligibility(_nonEligibleCustomerInArrears.Id,false);
        }


        [Test, AUT(AUT.Uk), JIRA("PP-32"), Owner(Owner.SvyatoslavKravchenko)]
        public void ExecuteGetEligibleCustomersQuery()
        {
            GetCustomerPrepaidEligibilityQuery eligibleCustomerMessage = new GetCustomerPrepaidEligibilityQuery
                                                                             {
                                                                                 AccountId = _eligibleCustomer.Id
                                                                             };
            GetCustomerPrepaidEligibilityQuery nonEligibleCustomerMessage = new GetCustomerPrepaidEligibilityQuery
                                                                             {
                                                                                 AccountId = _nonEligibleCustomer.Id
                                                                             };
            GetCustomerPrepaidEligibilityQuery nonEligibleCustomerInArrearsMessage = new GetCustomerPrepaidEligibilityQuery
                                                                             {
                                                                                AccountId = _nonEligibleCustomerInArrears.Id
                                                                             };

            Assert.IsTrue(Drive.Api.Queries.Post(eligibleCustomerMessage).Values[ELIGIBLE_RESPONSE_KEY].First().Equals(ELIGIBLE_CUSTOMER_RESPONSE));
            Assert.IsTrue(Drive.Api.Queries.Post(nonEligibleCustomerInArrearsMessage).Values[ELIGIBLE_RESPONSE_KEY].First().Equals(NON_ELIGIBLE_CUSTOMER_RESPONSE));
            Assert.IsTrue(Drive.Api.Queries.Post(nonEligibleCustomerMessage).Values[ELIGIBLE_RESPONSE_KEY].First().Equals(NON_ELIGIBLE_CUSTOMER_RESPONSE));
            
        }

        [TearDown]
        public void Rollback()
        {
            CustomerOperations.DeleteMarketingEligibility(_eligibleCustomer.Id);
            CustomerOperations.DeleteMarketingEligibility(_nonEligibleCustomerInArrears.Id);
        }


    }
}

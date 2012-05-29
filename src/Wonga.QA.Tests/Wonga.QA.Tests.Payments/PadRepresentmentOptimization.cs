using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class PadRepresentmentOptimization
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenACustomerGoesThenAnAttemptToRetrieve33PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheFirstPadRepresentmentForACustomerInArrearsIsSuccessfulThenASecondAttemptToRetrieve50PercentOfTheBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheSecondPadRepresentmentForACustomerInArrearsIsSuccessfulThenAThirdAttemptToRetrieveTheRemainingBalanceShouldBeMadeOnTheNextPayDate()
        {

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1962"), Ignore()]
        public void WhenTheThirdPadRepresentmentForACustomerInArrearsFailsThenThereShouldBeNoMoreAttemptsToRetrieveMoneyFromTheCustomer()
        {

        }
    }
}

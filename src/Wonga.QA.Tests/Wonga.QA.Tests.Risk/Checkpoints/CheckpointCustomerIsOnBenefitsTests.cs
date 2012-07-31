using System;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [Parallelizable(TestScope.All)]
    class CheckpointCustomerIsOnBenefitsTests
    {

        private const RiskMask TESTMask = RiskMask.TESTCustomerIsNotOnBenefits;

        [Test, AUT(AUT.Ca), JIRA("CA-2438")]
        public void L0CustomerIsOnBenefitsThenApplicationDeclined()
        {
            var customer = CustomerBuilder.New().WithEmployer(TESTMask).WithEmployerStatus(EmploymentStatusEnum.OnBenefits.ToString()).Build();
             ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }
    }
}

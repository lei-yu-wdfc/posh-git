using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Experian.AuthenticatePlus
{
    [Parallelizable(TestScope.All)]
    public class AuthenticatePlusCheckpointTests
    {
        [Test]
        [AUT(AUT.Uk), Owner(Owner.RiskTeam)]
        [JIRA("UK-854")]
        [Pending("Not yet ready as it`s WIP")]
        public void TestAuthPlusReturnsHighRisk_LoanIsDeclined()
        {
            //var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTApplicantIsNotMinor).Build();
            //var application = ApplicationBuilder.New(customer).Build();
        }
    }
}

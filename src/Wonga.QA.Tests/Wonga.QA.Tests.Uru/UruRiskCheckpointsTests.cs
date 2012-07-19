using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data.Enums.Risk;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Uru
{
    [Parallelizable(TestScope.All)]
    public class UruRiskCheckpointsTests
    {

        #region Main Applicant

        /* Main applicant - IsNotMinor */

        [Test]
        [AUT(AUT.Uk)]
        [JIRA("UKRISK-92")]
        public void L0_MainApplicat_NotMinor_LoanIsAccepted()
        {
            var customer =
                CustomerBuilder.New().WithForename("Authenticated").WithSurname("IsNotMinor").WithEmployer(
                    RiskMask.TESTApplicantIsNotMinorUru).Build();
            var application =
                ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }

        [Test]
        [AUT(AUT.Uk)]
        [JIRA("UKRISK-92")]
        public void L0_MainApplicat_IsMinor_LoanIsDeclined()
        {
            var customer =
                CustomerBuilder.New().WithForename("Authenticated").WithSurname("IsMinor").WithEmployer(
                    RiskMask.TESTApplicantIsNotMinorUru).Build();
            var application =
                ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        #endregion  
      
    }
}

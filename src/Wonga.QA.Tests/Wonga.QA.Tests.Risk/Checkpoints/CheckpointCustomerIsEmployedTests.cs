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
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [TestFixture]
    [Parallelizable(TestScope.All), AUT(AUT.Uk,AUT.Za)]
    public class CheckpointCustomerIsEmployedTests
    {
        private const RiskMask TestMask = RiskMask.TESTCustomerIsEmployed;
        private Application _loanApplication;

        [Test]
        [JIRA("UKRISK-72"), Description("Scenario 1: Customer is uneploymend, application declined")]
        public void L0CustomerIsUnemployedThenApplicationDeclined()
        {
            var customer = CustomerBuilder.New().WithEmployer(TestMask).WithEmployerStatus(EmploymentStatusEnum.Unemployed.ToString()).WithForename("Forename").WithSurname("Surname").Build();
            ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        [Test]
        [JIRA("UKRISK-72"), Description("Scenario 2: Customer is eploymend, application accepted")]
        public void L0CustomerEmployedThenApplicationAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(TestMask).WithEmployerStatus(EmploymentStatusEnum.EmployedFullTime.ToString()).Build();
            _loanApplication = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }

        [Test]
        [JIRA("UKRISK-72"), DependsOn("L0CustomerEmployedThenApplicationAccepted")]
        public void LnCustomerEmployedThenApplicationAccepted()
        {
            _loanApplication.RepayOnDueDate();
            ApplicationBuilder.New(_loanApplication.GetCustomer()).WithExpectedDecision(
                ApplicationDecisionStatus.Accepted).Build();

        }

    }
}

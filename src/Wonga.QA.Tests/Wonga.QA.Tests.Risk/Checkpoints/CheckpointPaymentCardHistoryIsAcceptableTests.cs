using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    [TestFixture, Parallelizable(TestScope.All)]
    public class CheckpointPaymentCardHistoryIsAcceptableTests
    {
        [Test,AUT(AUT.Uk),Owner(Owner.RiskTeam)]
        [Description("If there are no live loans against this payment card -> Accept the application")]
        public void Ln_PaymentCardHistoryIsAcceptable_ApplicationIsAccepted()
        {
            var customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTPaymentCardHistoryIsAcceptable).Build();
            var l0Application = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            l0Application.RepayOnDueDate();
            var lnApplication = ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }
       
        [Test, AUT(AUT.Uk), Owner(Owner.RiskTeam)]
        [Description("If there are live loans against this payment card -> Decline the application")]
        public void Ln_PaymentCardHistory_LiveLoansAgainstPaymentCard_ApplicationIsDeclined()
        {
            
        }

        [Test, AUT(AUT.Uk), Owner(Owner.RiskTeam)]
        [Description("If there are written off loans against this payment card -> Decline the application")]
        public void Ln_PaymentCardHistory_WrittenOffLoansAgainstPaymentCard_ApplicationIsDeclined()
        {
            Guid paymentCardId = Guid.NewGuid();
        }
    }
}

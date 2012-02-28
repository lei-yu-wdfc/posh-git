using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Risk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk.Checkpoints
{
    public class EidCheckpointTests
    {
        [Test, AUT(AUT.Ca), JIRA("CA-1743")]
        public void LnShouldPassEidCheck()
        {
            Customer cust = CustomerBuilder.New().Build();

            Application l0App = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatusEnum.Accepted).Build();

            l0App.RepayOnDueDate();

            EmploymentDetailEntity employmentDetails = Driver.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == cust.Id);
            employmentDetails.EmployerName = "test:DirectFraud";
            employmentDetails.Submit();

            Application lNApp = ApplicationBuilder.New(cust).Build();

            //Assert.IsTrue(RiskApiCheckpointTests.SingleCheckPointVerification(lNApp, CheckpointStatus.Verified,CheckpointDefinitionEnum.UserAssistedFraudCheck));
            Assert.Contains(Application.GetExecutedCheckpointDefinitions(lNApp.Id, CheckpointStatus.Verified), Data.EnumToString(CheckpointDefinitionEnum.UserAssistedFraudCheck));
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Builders;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using ApplicationBuilder = Wonga.QA.Framework.Builders.ApplicationBuilder;

namespace Wonga.QA.Tests.PayLater.Checkpoints.RiskWorkflows
{
    [TestFixture]
    public class PayLaterRiskWorkflowTests
    {
        private List<string> _expectedPayLaterCheckpointsNames;

        [Test, AUT(AUT.Uk), JIRA("PAYLATER-592"), Ignore("blocked - checkpoints not loading")]
        public void VerifyExpectedCheckpointsAreLoadedForPayLaterL0()
        {
            _expectedPayLaterCheckpointsNames = ExpectedPayLaterCheckpointsForL0;

            var payLaterAccount = AccountBuilder.PayLater.New().Build();
            var payLaterApplication = ApplicationBuilder.PayLater.New(payLaterAccount).Build();

            var actualCheckpointNames = Drive.Db.GetCheckpointDefinitionsForApplication(payLaterApplication.Id).Select(a => a.Name);
            Assert.AreElementsEqualIgnoringOrder(_expectedPayLaterCheckpointsNames, actualCheckpointNames);
        }

        #region CheckpointLists

        private static readonly List<string> ExpectedPayLaterCheckpointsForL0 = new List<string>()
		                                                         	{
                                                                        "MobilePhoneIsUnique",
                                                                        "NoSuspiciousApplicationActivity",
                                                                        "ApplicationDeviceNotOnBlacklist",
                                                                        "PresentInWongalookBlacklist",
                                                                        "DoNotlendToCustomer",
                                                                        "MultipleUsersMappedtoCustomer",
                                                                        "CallCreditBureauDataIsAvailable",
                                                                        "ApplicationElementNotCIFASFlagged",
                                                                        "ApplicantIsNotDeceased",
                                                                        "ApplicantIsSolvent",
                                                                        "DOBEnteredinApplication",
                                                                        "MobilePINValidation",
                                                                        "CustomerIsEmployed", //(this needs to be configurable on/off - we may not decline based on this)
                                                                        "MonthlyIncomeEnoughForRepayment"
		                                                         	};
        #endregion
    }
}

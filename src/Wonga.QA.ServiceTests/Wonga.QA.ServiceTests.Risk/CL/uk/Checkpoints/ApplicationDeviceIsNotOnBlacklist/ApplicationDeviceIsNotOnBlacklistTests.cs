using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mocks.Service;
using Wonga.QA.Framework.Msmq.Enums.Integration.Iovation;
using Wonga.QA.Framework.Msmq.Messages.Risk.BlackList;
using Wonga.QA.Framework.Msmq.Messages.Risk.Iovation;
using Wonga.QA.ServiceTests.Risk.CL.uk;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.ServiceTests.Risk.CL.Uk.Checkpoints.ApplicationDeviceIsNotOnBlacklist
{
    [Parallelizable(TestScope.All), AUT(AUT.Uk)]
    public class ApplicationDeviceIsNotOnBlacklistTests : RiskServiceTestClUkBase
    {
        protected override void BeforeEachTest()
        {
            base.BeforeEachTest();
            Background(maskName: RiskMask.TESTApplicationDeviceNotOnBlacklist,
                         checkpointName: "ApplicationDeviceNotOnBlacklist",
                         responsibleVerification: "ApplicantIsNotOnBlackListVerification");
        }

        [Test]
        public void IfMainApplicantDeviceNotFoundOnBlacklist_ApplicationIsDeclined()
        {
            GivenThatIovationWillPassTheApplicantsDevice();
            WhenTheL0UserAppliesForALoan();
            ThenTheRiskServiceShouldApproveTheLoan();
        }

        private void GivenThatIovationWillPassTheApplicantsDevice()
        {
            
            EndpointMock
               .OnArrivalOf<IovationRequestMessage>()
               .Matching(x => x.AccountId == MainApplicantAccountId)
               .ThenDoThis((receivedMsg, bus) =>
                {
                    var message = new IovationResponseMessage
                                        {
                                            AccountId = MainApplicantAccountId,
                                            SagaId = receivedMsg.SagaId,
                                            Result = IovationAdviceEnum.Allow,
                                            //Details = "<NServiceBus.KeyValuePairOfStringAndString><Key>Hello</Key><Value>World</Value></NServiceBus.KeyValuePairOfStringAndString>",
                                            Details = new Dictionary<string, string>() 
                                            {
                                                {"Key", "Hello"},
                                                {"Value", "Value"}
                                            },
                                            Reason = "Some Reason",
                                            TrackingNumber = "3478236472364234"
                                        };

                    bus.Send("riskservice",message);
                    
                });
        }
    }
}

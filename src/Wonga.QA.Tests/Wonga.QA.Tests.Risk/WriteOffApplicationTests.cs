using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Risk
{
    [TestFixture]
    public class WriteOffApplicationTests
    {
        [Test, AUT(AUT.Uk), JIRA("UKRISK-229")]
        public void RiskApplicationShouldBeWrittenOff_WhenCSAgentWritesOffApplication()
        {
            var customer = CustomerBuilder.New().Build();
            var application =
                ApplicationBuilder.New(customer).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

            var writeOffApplicationCommand = new WriteOffApplicationCommand {ApplicationId = application.Id,DoNotRelend = true};

            Drive.Cs.Commands.Post(writeOffApplicationCommand);

            Do.Until(() => Drive.Data.Risk.Db.RiskApplications.FindAllBy(
                ApplicationId: application.Id).SingleOrDefault().IsWrittenOff);

        }
    }
}

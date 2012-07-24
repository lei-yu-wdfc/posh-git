using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms
{
    [Parallelizable(TestScope.All)]
    public class CommsServiceTests
    {
        [Test]
        public void CommsServiceIsRunning()
        {
            Assert.IsTrue(Drive.Svc.Comms.IsRunning());
        }

        [Test, 
            AUT(AUT.Uk), 
            JIRA("UK-598"), 
            Description("Check that emails are received, when extension is not completed."), 
            Ignore("Not implemented in version 3.16.0")]
        public void LoanExtensionNotCompleteEmailSent()
        {

        }

        [Test, AUT(AUT.Wb), JIRA("SME-1423")]
        public void CommsEmailRegexValidationShouldAcceptEmailWithHyphenInDomain()
        {
            String email = Get.GetId().ToString() + "@won-ga.com";

            CustomerBuilder.New()
                .WithEmailAddress(email)
                .Build();
        }
    }
}

using System;
using System.Diagnostics;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
    [TestFixture, Parallelizable(TestScope.All), AUT(AUT.Za, AUT.Uk)]
    public class AcceptedLoanEmailTests
    {
        private Application _application;

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            if (Config.AUT != AUT.Za) return;

            //Why does ZA require BG be enabled to test this??
            if (Drive.Data.Ops.GetServiceConfiguration<bool>("BankGateway.IsTestMode"))
                Assert.Inconclusive("Bankgateway is in test mode");
        }

        [Test, AUT(AUT.Za, AUT.Uk), JIRA("QA-204"), Owner(Owner.MichaelDoyle), Explicit]
        [AUTRow(AUT.Za, "18432")]
        [AUTRow(AUT.Uk, "9951")]
        public void AgreementEmailSentAfterApplicationAccepted(string expectedEmailTemplateName)
        {
            Debug.Print(expectedEmailTemplateName);
            Customer customer = CustomerBuilder.New().Build();
            _application = ApplicationBuilder.New(customer).Build();


            AssertDo.Until(
                () => Drive.Data.QaData.Db.Email.FindBy(EmailAddress: customer.Email, TemplateName: expectedEmailTemplateName),
                "Email of type:{0} to address{1} not found in QAData.Email table.", expectedEmailTemplateName, customer.Email);

            /*	If the test passes, but emails are still not being received - check the exact target admin system;
                Sometimes the "interaction" gests stuck.
                Logon and goto Interactions/Messages/Email/Trigggered - locate the interaction for the number above (hint: sorting by Modified-Date helps)
                Check the number of queued and if it's >0, pause and restart the interaction.
                */
        }

        [Test, AUT(AUT.Uk), JIRA("QA-204"), Owner(Owner.MichaelDoyle)]
        [AUTRow(AUT.Uk, "34718")]
        public void EmailSentAfterApplicationDecline(string expectedEmailTemplateName)
        {
            Debug.Print(expectedEmailTemplateName);
            Customer customer = CustomerBuilder.New().WithEmployer(RiskMask.TESTBankAccountMatchedToApplicant).Build();
            _application = ApplicationBuilder.New(customer).WithExpectedDecision(Wonga.QA.Framework.Api.ApplicationDecisionStatus.Declined).Build();


            Func<dynamic> query = () => Drive.Data.QaData.Db.Email.FindBy(EmailAddress: customer.Email, TemplateName: expectedEmailTemplateName);
            var row = AssertDo.Until(query,
                "Email of type:{0} to address{1} not found in QAData.Email table.", expectedEmailTemplateName, customer.Email);

            var emailId = (int)row.EmailId;

            var emailBody = Drive.Data.QaData.Db.EmailToken.FindBy(EmailId: emailId, Key: "Html_body");

            Assert.Contains(emailBody.Value, "This application did not meet our criteria");

            /*	If the test passes, but emails are still not being received - check the exact target admin system;
                Sometimes the "interaction" gests stuck.
                Logon and goto Interactions/Messages/Email/Trigggered - locate the interaction for the number above (hint: sorting by Modified-Date helps)
                Check the number of queued and if it's >0, pause and restart the interaction.
                */
        }



        [Test, AUT(AUT.Za), JIRA("QA-204"), DependsOn("AgreementEmailSentAfterApplicationAccepted"), Pending]
        public void PaymentConfirmationEmailSentAfterCustomerIsFunded()
        {
            //Need to timeout saga manually
            var sagaId = Drive.Data.OpsSagas.Db.HyphenBatchCashOutEntity.FindAll().Last();
        }
    }
}

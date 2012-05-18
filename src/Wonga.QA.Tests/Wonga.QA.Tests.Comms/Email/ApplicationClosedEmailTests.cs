using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
    [TestFixture, AUT(AUT.Wb)]
    public class ApplicationClosedEmailTests
    {
        private BusinessApplication applicationInfo;
        private string _emailAddress;

        [SetUp]
        public void Setup()
        {
            _emailAddress = Get.RandomEmail();
            var customer = CustomerBuilder
                .New()
                .WithEmailAddress(_emailAddress)
                .Build();
            var organisation = OrganisationBuilder.New(customer).Build();
            applicationInfo = ApplicationBuilder.New(customer, organisation).Build() as BusinessApplication;
        }

        [Test, AUT(AUT.Wb), JIRA("SME-1393")]
        public void ShouldSendEmailWhenLoanIsPaidInFull()
        {
            var templateName = "34381";
            applicationInfo.MoveBackInTime(7, false);

            var totalOutstandingAmount = applicationInfo.GetTotalOutstandingAmount();

            applicationInfo.CreateExtraPayment((decimal)totalOutstandingAmount);

            Do.With.Message("Should send email with info").Until(
                () => Drive.Data.QaData.Db.Emails.FindBy(EmailAddress: _emailAddress, TemplateName: templateName));
        }
    }
}

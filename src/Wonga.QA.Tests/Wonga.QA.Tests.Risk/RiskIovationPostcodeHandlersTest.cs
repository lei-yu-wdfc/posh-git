using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Enums;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Risk
{
    [Parallelizable(TestScope.All)]
    class RiskIovationPostcodeHandlersTest
    {

        [Test, AUT(AUT.Ca), JIRA("CA-2469")]
        public void UpdateApplicationClosedOnHandlerTest()
        {
            var customer = CustomerBuilder.New().Build();
            var app = ApplicationBuilder.New(customer).Build();
            app.MakeDueToday();

            Do.With.Timeout(3).Until(() => (Drive.Data.Risk.Db.RiskApplication.FindByApplicationId(app.Id).ClosedOn) is DateTime);
            var closedOnDecision = Drive.Data.Risk.Db.RiskApplication.FindByApplicationId(app.Id).ClosedOn;

            Assert.IsNotNull(closedOnDecision);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2469")]
        public void UpdateApplicationIsCanceledHandlerTest()
        {
            var customer = CustomerBuilder.New().Build();
            var app = ApplicationBuilder.New(customer).Build();
            Drive.Msmq.Risk.Send(new IApplicationCanceled { AccountId = customer.Id, ApplicationId = app.Id, CreatedOn = DateTime.Now });

            var signedIsCanceled = Do.With.Timeout(3).Until(
                () => Drive.Data.Risk.Db.RiskApplication.FindBy(ApplicationId: app.Id, IsCanceled: 1));

            Assert.IsTrue(signedIsCanceled.IsCanceled);

        }

        [Test, AUT(AUT.Ca), JIRA("CA-2469")]
        public void UpdateApplicationSignedOnHandlerTest()
        {
            var customer = CustomerBuilder.New().WithEmployerStatus(EmploymentStatusEnum.Student.ToString()).Build();
            var app = ApplicationBuilder.New(customer).Build();
            var signedOnDecision = Drive.Data.Risk.Db.RiskApplication.FindByApplicationId(app.Id).SignedOn;
            Assert.IsNotNull(signedOnDecision);
        }
    }
}

using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Comms.Email
{
    [TestFixture]
    class RepaymentArrangementEmailTests
    {
        [Test, AUT(AUT.Uk), JIRA("WIN-888"), Parallelizable]
        public void RepaymentArrangementSuccessSendsEmail()
        {
            //Test written to support both mocked and non mocked environments
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Do.Until(customer.GetPaymentCard);
            Application application = ApplicationBuilder.New(customer).Build();

            application.PutIntoArrears(4);

            application.CreateRepaymentArrangement();

            dynamic paymentsDb = Drive.Data.Payments.Db;

            var app = paymentsDb.Applications.FindByExternalId(application.Id);
			
            //Process Repayment Arrangement Installment, expecting success
            var repaymentArrangement = paymentsDb.RepaymentArrangements.FindByApplicationId(app.ApplicationId);
            var firstRepaymentArrangementDetail = paymentsDb.RepaymentArrangementDetails.FindByRepaymentArrangementId(repaymentArrangement.RepaymentArrangementId);

            Drive.Msmq.Payments.Send(new ProcessScheduledRepaymentMessage
            {
                RepaymentArrangementId = repaymentArrangement.RepaymentArrangementId,
                                                                            RepaymentDetailId = firstRepaymentArrangementDetail.RepaymentArrangementDetailId });
			
            dynamic emails = Drive.Data.QaData.Db.Email;
            Do.With.Timeout(1).Interval(5).Until(() => emails.FindBy(EmailAddress: customer.GetEmail(), TemplateName: "18489"));
        }
    }
}

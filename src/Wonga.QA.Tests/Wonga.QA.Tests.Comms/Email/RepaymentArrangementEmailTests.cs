using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using AddPaymentCardCommand = Wonga.QA.Framework.Api.AddPaymentCardCommand;

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

            application.PutApplicationIntoArrears(4);

            application.CreateRepaymentArrangement();

            var app = Drive.Db.Payments.Applications.Single(a => a.ExternalId == application.Id);
			
            //Remove card details, replace with bad card details to cause payment failure
            var db = new DbDriver();
            var accountPreferences = db.Payments.AccountPreferences.Single(x => x.AccountId == app.AccountId);
            accountPreferences.PrimaryPaymentCardId = null;
            db.Payments.SubmitChanges();
            var paymentCard = Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid);
            Drive.Db.Payments.PersonalPaymentCards.Single(x => x.PaymentCardId == paymentCard.PaymentCardId).Delete().Submit();
            Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid).Delete().Submit();
            Drive.Db.ColdStorage.PaymentCards.Single(x => x.ExternalId == app.PaymentCardGuid).Delete().Submit();
            Drive.Api.Commands.Post(new AddPaymentCardCommand { AccountId = app.AccountId,
                                                                PaymentCardId = app.PaymentCardGuid,
                                                                CardType = "Other",
                                                                Number = "4444333322221111", //This card will work if the cardpayment tc is in test mode.
                                                                HolderName = "Test Holder",
                                                                StartDate = DateTime.Today.AddYears(-1).ToDate(DateFormat.YearMonth),
                                                                ExpiryDate = DateTime.Today.AddMonths(6).ToDate(DateFormat.YearMonth),
                                                                IssueNo = "000",
                                                                SecurityCode = "666",
                                                                IsCreditCard = false,
                                                                IsPrimary = true, });

            Do.Until(() => Drive.Db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid));
			
            var newPaymentCard = db.Payments.PaymentCardsBases.Single(x => x.ExternalId == app.PaymentCardGuid);
            //Set date to past for card payment mock
            newPaymentCard.ExpiryDate = DateTime.Today.AddYears(-1);
            accountPreferences.Refresh().PrimaryPaymentCardId = newPaymentCard.PaymentCardId;
            db.Payments.SubmitChanges();

            //Process Repayment Arrangement Installment, expecting success
            var repaymentArrangement = Drive.Db.Payments.RepaymentArrangements.Single(x => x.ApplicationId == app.ApplicationId);
            var firstRepaymentArrangementDetail = db.Payments.RepaymentArrangementDetails
                                                             .First(x => x.RepaymentArrangementId == repaymentArrangement.RepaymentArrangementId);

            Drive.Msmq.Payments.Send(new ProcessScheduledRepaymentCommand { RepaymentArrangementId = repaymentArrangement.RepaymentArrangementId,
                                                                            RepaymentDetailId = firstRepaymentArrangementDetail.RepaymentArrangementDetailId });
			
            dynamic emails = Drive.Data.QaData.Db.Email;
            Do.With.Timeout(1).Interval(5).Until(() => emails.FindBy(EmailAddress: customer.GetEmail(), TemplateName: "18489"));
        }
    }
}

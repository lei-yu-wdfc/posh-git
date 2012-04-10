using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    [TestFixture]
    public class SchedulePaymentSagaTests
    {
        [Test]
        [AUT(AUT.Uk), JIRA("UK-876")]
        public void SubmitApplication_CreatesSagaEntity()
        {
            decimal loanAmount = 350m;
            int loanTerm = 12;
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Application application = ApplicationBuilder.New(customer)
                .WithLoanTerm(loanTerm)
                .WithLoanAmount(loanAmount).Build();

            Do. Until(() => Drive.Db.OpsSagas.FixedTermLoanSagaEntities
                               .Single(saga=>saga.AccountGuid == customer.Id 
                                             && saga.ApplicationGuid == application.Id 
                                             && saga.TermsAgreed == true
                                             && saga.ApplicationAccepted == true));
        }

        [Test]
        [AUT(AUT.Uk), JIRA("UK-876")]
        public void MakeDueToday_CompletesSagaAndCreatesScheduledPaymentSaga()
        {
            decimal loanAmount = 350m;
            int loanTerm = 12;
            Customer customer = CustomerBuilder.New().Build();
            Do.Until(customer.GetBankAccount);
            Application application = ApplicationBuilder.New(customer)
                .WithLoanTerm(loanTerm)
                .WithLoanAmount(loanAmount).Build();

            Do.Until(() => Drive.Db.OpsSagas.FixedTermLoanSagaEntities
                               .Single(saga => saga.AccountGuid == customer.Id
                                               && saga.ApplicationGuid == application.Id
                                               && saga.TermsAgreed == true
                                               && saga.ApplicationAccepted == true));

            application.MakeDueToday();

            Do.While(() => Drive.Db.OpsSagas.FixedTermLoanSagaEntities
                               .Any(saga => saga.AccountGuid == customer.Id
                                            && saga.ApplicationGuid == application.Id
                                            && saga.TermsAgreed == true
                                            && saga.ApplicationAccepted == true));
            Do.Until(() => Drive.Db.OpsSagas.ScheduledPaymentSagaEntities.Single(
                saga => saga.ApplicationGuid == application.Id 
                        && saga.AccountGuid == customer.Id));
        }


    }
}
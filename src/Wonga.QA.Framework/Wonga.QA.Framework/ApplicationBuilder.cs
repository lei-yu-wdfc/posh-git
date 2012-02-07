using System;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        private Guid _id;
        private Customer _customer;

        private Drivers _drivers;

        private ApplicationBuilder()
        {
            _id = Guid.NewGuid();
            _drivers = new Drivers();
        }

        public static ApplicationBuilder New(Customer customer)
        {
            return new ApplicationBuilder { _customer = customer };
        }

        public Application Build()
        {
            _drivers.Api.Commands.Post(new ApiRequest[]
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id; }),
                CreateFixedTermLoanApplicationCommand.New(r =>
                {
                    r.ApplicationId = _id;
                    r.AccountId = _customer.Id;
                    r.BankAccountId = _customer.GetBankAccount();
                }),
                VerifyFixedTermLoanCommand.New(r => { r.ApplicationId = _id; r.AccountId=_customer.Id; })
            });

            Do.Until(() => _drivers.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single() == "Accepted");

            _drivers.Api.Commands.Post(new SignApplicationCommand
            {
                AccountId = _customer.Id,
                ApplicationId = _id,
            });

            Do.Until(() => _drivers.Api.Queries.Post(new GetAccountSummaryQuery { AccountId = _customer.Id }).Values["HasCurrentLoan"].Single() == "true");

            return new Application(_id);
        }
    }
}

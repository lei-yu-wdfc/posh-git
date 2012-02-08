using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        private Guid _id;
        private Customer _customer;
        private Company _company;


        private Drivers _drivers;

        private ApplicationBuilder()
        {
            _id = Guid.NewGuid();
            _drivers = new Drivers();
        }

        public static ApplicationBuilder New(Customer customer)
        {
            return new ApplicationBuilder { _customer = customer};
        }

        public static ApplicationBuilder New(Customer customer, Company company)
        {
            return new ApplicationBuilder{_customer = customer,_company = company};
        }

        public Application Build()
        {
            List<ApiRequest> requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id; }),
                VerifyFixedTermLoanCommand.New(r => { r.ApplicationId = _id; r.AccountId=_customer.Id; })
            };

            switch (Config.AUT)
            {
                case AUT.Wb:
                    requests.Add(CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                    {
                        r.AccountId = _customer.Id; 
                        r.OrganisationId = _company.Id; 
                        r.BusinessPaymentCardId = _company.GetPaymentCard();
                        r.BusinessBankAccountId = _company.GetBankAccount();
                    }));
                    break;

                default:
                    requests.Add(CreateFixedTermLoanApplicationCommand.New(r =>
                    {
                        r.ApplicationId = _id;
                        r.AccountId = _customer.Id;
                        r.BankAccountId = _customer.GetBankAccount();
                    }));
                    break;
            }


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

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private ApplicationBuilder()
        {
            _id = Guid.NewGuid();
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
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id; })
            };

            switch (Config.AUT)
            {
                case AUT.Wb:
                    requests.AddRange(new ApiRequest[]{
                        CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                        {   
                            r.AccountId = _customer.Id; 
                            r.OrganisationId = _company.Id;
                            r.ApplicationId = _id;
                            r.BusinessPaymentCardId = _company.GetPaymentCard();
                            r.BusinessBankAccountId = _company.GetBankAccount();
                            r.MainApplicantBankAccountId = _customer.GetBankAccount();
                            r.MainApplicantPaymentCardId = _customer.GetPaymentCard();
                        }),
                        VerifyMainBusinessApplicantWbCommand.New(r => { r.AccountId = _customer.Id; r.ApplicationId = _id; })
                    });
                    
                    break;

                case AUT.Uk:
                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                            r.PaymentCardId = _customer.GetPaymentCard();
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = _customer.Id; 
                            r.ApplicationId = _id;
                        })
                    });
                    break;
			
                default:
                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = _customer.Id; 
                            r.ApplicationId = _id;
                        })
                    });
                    break;
            }
            
            Driver.Api.Commands.Post(requests);

            Do.Until(() => Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single() == "Accepted");
            
            Driver.Api.Commands.Post(Config.AUT == AUT.Wb
                                  ? (ApiRequest)
                                    new SignBusinessApplicationWbUkCommand { AccountId = _customer.Id, ApplicationId = _id }
                                  : new SignApplicationCommand { AccountId = _customer.Id, ApplicationId = _id });

            ApiRequest summary = Config.AUT == AUT.Za
                                     ? new GetAccountSummaryZaQuery {AccountId = _customer.Id}
                                     : Config.AUT == AUT.Wb
                                           ? (ApiRequest)
                                             new GetBusinessAccountSummaryWbUkQuery
                                                 {AccountId = _customer.Id, ApplicationId = _id}
                                           : new GetAccountSummaryQuery {AccountId = _customer.Id};

            Do.Until(() => Driver.Api.Queries.Post(summary).Values["HasCurrentLoan"].Single() == "true");

            Int32 previous = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            Do.While(() =>
            {
                Int32 current = Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == _id).Transactions.Count);
                if (previous != current)
                    stopwatch.Restart();
                previous = current;
                return stopwatch.Elapsed < TimeSpan.FromSeconds(5);
            });

            return new Application(_id);
        }
    }
}

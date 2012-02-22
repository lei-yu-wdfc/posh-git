using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Common.Enums.Risk;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Framework
{
    public class ApplicationBuilder
    {
        protected Guid _id;
        protected Customer _customer;
    	protected decimal _loanAmount;
    	protected Date _promiseDate;
        protected ApplicationDecisionStatusEnum _decision = ApplicationDecisionStatusEnum.Accepted;
        protected int _loanTerm;
        
        private Action _setPromiseDateAndLoanTerm;
        private Func<int> _getDaysUntilStartOfLoan;

        protected ApplicationBuilder()
        {
            _id = Guid.NewGuid();            
        	_loanAmount = Data.GetLoanAmount();

            _setPromiseDateAndLoanTerm = () =>
                                  {
                                      _promiseDate = Data.GetPromiseDate();
                                      _loanTerm = GetLoanTermFromPromiseDate();
                                  };

            _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoan;
        }

        private int GetLoanTermFromPromiseDate()
        {
            return (int)
                   (_promiseDate.DateTime.Subtract(DateTime.Today).TotalDays -
                    _getDaysUntilStartOfLoan());
        }

        public static ApplicationBuilder New(Customer customer)
        {
            return new ApplicationBuilder { _customer = customer};
        }

        public static ApplicationBuilder New(Customer customer, Organisation company)
        {
            return new BusineesAppicationBuilder(customer, company);
        }

		public ApplicationBuilder WithLoanAmount(decimal loanAmount)
		{
			_loanAmount = loanAmount;
			return this;
		}

		public ApplicationBuilder WithPromiseDate(Date promiseDate)
		{
		    _setPromiseDateAndLoanTerm = () =>
		                          {
		                              _promiseDate = promiseDate;
		                              _loanTerm = GetLoanTermFromPromiseDate();
		                          };

			return this;
		}

        public ApplicationBuilder WithLoanTerm(int loanTerm)
        {
            _setPromiseDateAndLoanTerm = () =>
                                             {
                                                 _loanTerm = loanTerm;
                                                 _promiseDate = GetPromiseDateFromLoanTerm(loanTerm);
                                             };

            return this;
        }

        private Date GetPromiseDateFromLoanTerm(int loanTerm)
        {
            return DateTime.Today.AddDays(loanTerm + _getDaysUntilStartOfLoan()).
                ToDate(DateFormat.Date);
        }

        public ApplicationBuilder WithExpectedDecision(ApplicationDecisionStatusEnum decision)
        {
            _decision = decision;
            return this;
        }
        
        public virtual Application Build()
        {
            if (Config.AUT == AUT.Wb)
            {
                throw new NotImplementedException(
                    "WB product should be using factory method with organization parameter");
            }

            _promiseDate.DateFormat = DateFormat.Date;
            _setPromiseDateAndLoanTerm();

            List<ApiRequest> requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id; })
            };

            switch (Config.AUT)
            {

                case AUT.Uk:

                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                            r.PaymentCardId = _customer.GetPaymentCard();
                        	r.LoanAmount = _loanAmount;
                        	r.PromiseDate = _promiseDate;
                        }),
                        VerifyFixedTermLoanCommand.New(r=>
                        {
                            r.AccountId = _customer.Id; 
                            r.ApplicationId = _id;
                        })
                    });
                    break;
			    case AUT.Ca:

                    // Start of Loan is different for CA
                    _getDaysUntilStartOfLoan = GetDaysUntilStartOfLoanForCa;
                    _setPromiseDateAndLoanTerm();

                    requests.AddRange(new ApiRequest[]{
                        CreateFixedTermLoanApplicationCommand.New(r =>
                        {
                            r.ApplicationId = _id;
                            r.AccountId = _customer.Id;
                            r.BankAccountId = _customer.GetBankAccount();
                            r.LoanAmount = _loanAmount;
                        	r.PromiseDate = _promiseDate;
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
                        	r.LoanAmount = _loanAmount;
                        	r.PromiseDate = _promiseDate;
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

            Do.Until(() => (ApplicationDecisionStatusEnum)
                Enum.Parse(typeof(ApplicationDecisionStatusEnum), Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single()) == _decision);

            if (_decision == ApplicationDecisionStatusEnum.Declined)
                return new Application(_id);

            if (_decision == ApplicationDecisionStatusEnum.Pending)
            {
                int i;
            }

            Driver.Api.Commands.Post(new SignApplicationCommand { AccountId = _customer.Id, ApplicationId = _id });

            ApiRequest summary = Config.AUT == AUT.Za
                                     ? new GetAccountSummaryZaQuery {AccountId = _customer.Id}
                                     :  (ApiRequest)new GetAccountSummaryQuery { AccountId = _customer.Id };


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

            return new Application {Id = _id, BankAccountId = _customer.BankAccountId, LoanAmount = _loanAmount, LoanTerm = _loanTerm};
        }

        private int GetDaysUntilStartOfLoan()
        {
            return 0;
        }

        private int GetDaysUntilStartOfLoanForCa()
        {
            return DateHelper.GetNumberOfDaysUntilStartOfLoanForCa();
        }
    }
}

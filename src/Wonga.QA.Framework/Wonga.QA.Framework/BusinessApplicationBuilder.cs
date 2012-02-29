using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.ContactManagement;

namespace Wonga.QA.Framework
{
    public class BusinessApplicationBuilder : ApplicationBuilder
    {
        protected Organisation _company;

        public BusinessApplicationBuilder(Customer customer, Organisation company)
        {
            _company = company;
            _customer = customer;
        }

        public override Application Build()
        {
            SignupSecondaryDirectors();

            var requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id; }),
                CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                        {   
                            r.AccountId = _customer.Id; 
                            r.OrganisationId = _company.Id;
                            r.ApplicationId = _id;
                            r.BusinessPaymentCardId = _company.GetPaymentCard();
                            r.BusinessBankAccountId = _company.GetBankAccount();
                            r.MainApplicantBankAccountId = _customer.GetBankAccount();
                            r.MainApplicantPaymentCardId = _customer.GetPaymentCard();
                        })
            };
            
            Driver.Api.Commands.Post(requests);

            Do.Until(
                () =>
                Driver.Db.Payments.BusinessFixedInstallmentLoanApplications.Any(
                    app => app.ApplicationEntity.ExternalId == _id));

            Driver.Api.Commands.Post(VerifyMainBusinessApplicantWbCommand.New(r =>
                                                                                  {
                                                                                      r.AccountId = _customer.Id;
                                                                                      r.ApplicationId = _id;
                                                                                  }));

            Do.Until(() => (ApplicationDecisionStatusEnum)Enum.Parse(typeof(ApplicationDecisionStatusEnum), Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single()) == _decision);

            if (_decision == ApplicationDecisionStatusEnum.Declined)
                return new Application(_id);

            Driver.Api.Commands.Post( new SignBusinessApplicationWbUkCommand { AccountId = _customer.Id, ApplicationId = _id });

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

        #region Helpers

        public void SignupSecondaryDirectors()
        {
            var guarantors = Driver.Db.ContactManagement.DirectorOrganisationMappings.Where(entity => entity.OrganisationId == _company.Id && entity.DirectorLevel > 0);
            foreach (DirectorOrganisationMappingEntity guarantor in guarantors)
            {
                CustomerBuilder sd = CustomerBuilder.New(guarantor.AccountId);
                sd.Build();
            }
        }

        #endregion
    }
}

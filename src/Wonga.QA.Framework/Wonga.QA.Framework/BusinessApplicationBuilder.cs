using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public class BusinessApplicationBuilder : ApplicationBuilder
    {
        protected Organisation Company;

        public BusinessApplicationBuilder(Customer customer, Organisation company)
        {
            Company = company;
            _customer = customer;
        }

        public override Application Build()
        {

            /* STEP 1
             * I create the initial common requests
             * If something is missing please let Alex P know */
            var requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=_id; r.AccountId = _customer.Id; }),
                CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                        {   
                            r.AccountId = _customer.Id; 
                            r.OrganisationId = Company.Id;
                            r.ApplicationId = _id;
                            r.BusinessPaymentCardId = Company.GetPaymentCard();
                            r.BusinessBankAccountId = Company.GetBankAccount();
                            r.MainApplicantBankAccountId = _customer.GetBankAccount();
                            r.MainApplicantPaymentCardId = _customer.GetPaymentCard();
                        }),
            };


            Driver.Api.Commands.Post(requests);

            /* STEP 2
             * And I submit my number of guarantors - 0 by default => 1 Risk Workflow will exist
             * IF more then 1 => more Workflows => Please remeber that they need to SIGN*/
            Driver.Api.Commands.Post(SubmitNumberOfGuarantorsCommand.New(r =>
                                                                             {
                                                                                 r.AccountId = _customer.Id;
                                                                                 r.ApplicationId = _id;
                                                                                 r.NumberOfGuarantors =Company.GetSecondaryDirectors().ToList().Count;
                                                                             }));

            /* STEP 3
             * I wait for Payments to do their job */
            Do.Until(() => Driver.Db.Payments.BusinessFixedInstallmentLoanApplications.Any(app => app.ApplicationEntity.ExternalId == _id));

            /* STEP 4
             * I verify the main applicant */
            Driver.Api.Commands.Post(VerifyMainBusinessApplicantWbCommand.New(r =>
                                                                                  {
                                                                                      r.AccountId = _customer.Id;
                                                                                      r.ApplicationId = _id;
                                                                                  }));
            /* Well - if its declined then the code below this IF statement wont work -> so as a temporary fix return the application 
             * Still need to investigate the wait for data / pending / PreAccepted */
            if (_decision == ApplicationDecisionStatusEnum.Declined)
            {
                Do.Until(() => (ApplicationDecisionStatusEnum)Enum.Parse(typeof(ApplicationDecisionStatusEnum), Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single()) == _decision);
                return new BusinessApplication(_id);
            }

            /* STEP 5
             * And the main applicant signs the application */
            Driver.Api.Commands.Post(new SignBusinessApplicationWbUkCommand { AccountId = _customer.Id, ApplicationId = _id });

            /* STEP 6 
             * And I wait for Payments to create the application */
            var previous = 0;
            var stopwatch = Stopwatch.StartNew();
            Do.While(() =>
            {
                Int32 current = Do.Until(() => Driver.Db.Payments.Applications.Single(a => a.ExternalId == _id).Transactions.Count);
                if (previous != current)
                    stopwatch.Restart();
                previous = current;
                return stopwatch.Elapsed < TimeSpan.FromSeconds(5);
            });

            /* STEP 7 
             * And I wait for the decision i want - PLEASE REMEBER THAT THE DEFAULT ONE IS ACCEPTED */
            Do.Until(() => (ApplicationDecisionStatusEnum)Enum.Parse(typeof(ApplicationDecisionStatusEnum), Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single()) == _decision);

            return new BusinessApplication(_id);
        }

        public void SignApplicationForSecondaryDirectors()
        {
            var guarantors = Driver.Db.ContactManagement.DirectorOrganisationMappings.Where(
                director => director.OrganisationId == Company.Id && director.DirectorLevel > 0);


            var requests = new List<ApiRequest>();

            foreach (var guarantor in guarantors)
            {
                requests.Add(new SignBusinessApplicationWbUkCommand { AccountId = guarantor.AccountId, ApplicationId = _id });
            }

            Driver.Api.Commands.Post(requests);
        }

        #region OLD BUILD METHOD

        private Application OLD_Build()
        {
            var requests = new List<ApiRequest>
                               {
                                   SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = _id),
                                   SubmitClientWatermarkCommand.New(r =>
                                                                        {
                                                                            r.ApplicationId = _id;
                                                                            r.AccountId = _customer.Id;
                                                                        }),
                                   CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                                                                                                    {
                                                                                                        r.AccountId =_customer.Id;
                                                                                                        r.OrganisationId=Company.Id;
                                                                                                        r.ApplicationId=_id;
                                                                                                        r.BusinessPaymentCardId=Company.GetPaymentCard();
                                                                                                        r.BusinessBankAccountId=Company.GetBankAccount();
                                                                                                        r.MainApplicantBankAccountId=_customer.GetBankAccount();
                                                                                                        r.MainApplicantPaymentCardId=_customer.GetPaymentCard();
                                                                                                    }),
                                   SubmitNumberOfGuarantorsCommand.New(r =>
                                                                           {
                                                                               r.AccountId = _customer.Id;
                                                                               r.ApplicationId = _id;
                                                                               r.NumberOfGuarantors =Company.GetSecondaryDirectors().ToList().Count;
                                                                           })
                               };

            Driver.Api.Commands.Post(requests);

            Do.Until(() =>Driver.Db.Payments.BusinessFixedInstallmentLoanApplications.Any(app => app.ApplicationEntity.ExternalId == _id));
            Driver.Api.Commands.Post(VerifyMainBusinessApplicantWbCommand.New(r =>
                                                                                  {
                                                                                      r.AccountId = _customer.Id;
                                                                                      r.ApplicationId = _id;
                                                                                  }));
            Do.Until(() => (ApplicationDecisionStatusEnum)Enum.Parse(typeof(ApplicationDecisionStatusEnum), Driver.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = _id }).Values["ApplicationDecisionStatus"].Single()) == _decision);

            if (_decision == ApplicationDecisionStatusEnum.Declined)
                return new BusinessApplication(_id);

            Driver.Api.Commands.Post(new SignBusinessApplicationWbUkCommand{AccountId = _customer.Id, ApplicationId = _id});

            Int32 previous = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            Do.While(() =>
                         {
                             Int32 current =Do.Until(() =>Driver.Db.Payments.Applications.Single(a => a.ExternalId == _id).Transactions.Count);
                             if (previous != current) stopwatch.Restart();
                             previous = current;
                             return stopwatch.Elapsed < TimeSpan.FromSeconds(5);
                         });

            return new BusinessApplication(_id);
        }

        #endregion
    }
}

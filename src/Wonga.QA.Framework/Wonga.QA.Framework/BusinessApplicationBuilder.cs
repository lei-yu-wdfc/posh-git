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
            Customer = customer;
            Guarantors = new List<Customer>();
        }

        public override Application Build()
        {

            if (Guarantors.Count > 0 && Decision != ApplicationDecisionStatus.Declined)
            {
                Decision = ApplicationDecisionStatus.PreAccepted;
            }

            /* STEP 1
             * I create the initial common requests
             * If something is missing please let Alex P know */
            var requests = new List<ApiRequest>
            {
                SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = Id),
                SubmitClientWatermarkCommand.New(r => { r.ApplicationId=Id; r.AccountId = Customer.Id;
                                                          r.BlackboxData = IovationBlackBox;
                }),
                CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                        {   
                            r.AccountId = Customer.Id; 
                            r.OrganisationId = Company.Id;
                            r.ApplicationId = Id;
                            r.BusinessPaymentCardId = Company.GetPaymentCard();
                            r.BusinessBankAccountId = Company.GetBankAccount();
                            r.MainApplicantBankAccountId = Customer.GetBankAccount();
                            r.MainApplicantPaymentCardId = Customer.GetPaymentCard();
                        }),
            };


            Drive.Api.Commands.Post(requests);

            /* STEP 2
             * And I submit my number of guarantors - 0 by default => 1 Risk Workflow will exist
             * IF more then 1 => more Workflows => Please remeber that they need to SIGN*/
            Drive.Api.Commands.Post(SubmitNumberOfGuarantorsCommand.New(r =>
                                                                            {
                                                                                r.AccountId = Customer.Id;
                                                                                r.ApplicationId = Id;
                                                                                //r.NumberOfGuarantors =Company.GetSecondaryDirectors().ToList().Count;
                                                                                r.NumberOfGuarantors =
                                                                                    Guarantors.Count;
                                                                            }));
                

            /* STEP 3
             * I wait for Payments to do their job */
            Do.Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.Any(app => app.ApplicationEntity.ExternalId == Id));

            /* STEP 3.1 
             * Send guarantors partial details
             * Added new stuff to work with new risk workflows */
            foreach (var guarantor in Guarantors)
            {
                var g = guarantor;
                Drive.Api.Commands.Post(AddSecondaryOrganisationDirectorCommand.New(r =>
                {
                    r.OrganisationId =Company.Id;
                    r.AccountId = g.Id;
                    r.Email = g.Email;
                    r.Forename = g.Forename;
                    r.Surname = g.Surname;
                }));
            }

            /* STEP 4
             * I verify the main applicant */
            Drive.Api.Commands.Post(VerifyMainBusinessApplicantWbCommand.New(r =>
                                                                                  {
                                                                                      r.AccountId = Customer.Id;
                                                                                      r.ApplicationId = Id;
                                                                                  }));
            /* Well - if its declined then the code below this IF statement wont work -> so as a temporary fix return the application 
             * Still need to investigate the wait for data / pending / PreAccepted */
            if (Decision == ApplicationDecisionStatus.Declined)
            {
                Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id }).Values["ApplicationDecisionStatus"].Single()) == Decision);
                return new BusinessApplication(Id);
            }

            /* STEP 5
             * now let send a payment comnand representing payment term sliders adjustment*/
            Drive.Api.Commands.Post(UpdateLoanTermWbUkCommand.New(r=>
                                                                      {
                                                                          r.ApplicationId = Id;                                                                          
                                                                      })); //use default for term - same as for create applicatiomn

            /* STEP 6
             * And the main applicant signs the application */
            Drive.Api.Commands.Post(new SignBusinessApplicationWbUkCommand { AccountId = Customer.Id, ApplicationId = Id });

            /* STEP 7 
             * And I wait for Payments to create the application */
            //var previous = 0;
            //var stopwatch = Stopwatch.StartNew();
            //Do.While(() =>
            //{
            //    Int32 current = Do.Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Count);
            //    if (previous != current)
            //        stopwatch.Restart();
            //    previous = current;
            //    return stopwatch.Elapsed < TimeSpan.FromSeconds(5);
            //});

            Do.While(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Count == 3);

            /* STEP 8
             * And I wait for the decision i want - PLEASE REMEBER THAT THE DEFAULT ONE IS ACCEPTED */
            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id }).Values["ApplicationDecisionStatus"].Single()) == Decision);

            return new BusinessApplication(Id);
        }

        #region Public Methods

        public void SignApplicationForSecondaryDirectors()
        {
            var guarantors = Drive.Db.ContactManagement.DirectorOrganisationMappings.Where(
                director => director.OrganisationId == Company.Id && director.DirectorLevel > 0);


            var requests = new List<ApiRequest>();

            foreach (var guarantor in guarantors)
            {
                requests.Add(new SignBusinessApplicationWbUkCommand { AccountId = guarantor.AccountId, ApplicationId = Id });
            }

            Drive.Api.Commands.Post(requests);
        }
        public void BuildGuarantors(Boolean scrubNames = true)
        {
            foreach (var guarantor in Guarantors)
            {
                var guarantorCustomerBuilder = CustomerBuilder.New(guarantor.Id);
                if (scrubNames)
                {
                    guarantorCustomerBuilder.ScrubForename(guarantor.Forename);
                    guarantorCustomerBuilder.ScrubSurname(guarantor.Surname);
                }

                //THIS IS A HACK! WE NEED TO EXTEND THE QAF TO PROPERLY BUILD AND CREATE SECONDARY DIRECTORS AND ALL THIS STUFF.
                guarantorCustomerBuilder.WithEmailAddress(guarantor.Email).WithForename(guarantor.Forename)
                    .WithSurname(guarantor.Surname).WithDateOfBirth(guarantor.DateOfBirth).WithMobileNumber(guarantor.MobilePhoneNumber).WithMiddleName(!String.IsNullOrEmpty(guarantor.MiddleName) ? guarantor.MiddleName : RiskMask.TESTNoCheck.ToString()).Build();
            }
        }

        #endregion

        #region "With" Helper Methods

        //TODO: Return BusinessApplicationBuilder not ApplicationBuilder
        public BusinessApplicationBuilder WithGuarantors(List<Customer> guarantors)
        {
            Guarantors = guarantors;
            return this;
        }

        #endregion

        #region OLD BUILD METHOD

        private Application OLD_Build()
        {
            var requests = new List<ApiRequest>
                               {
                                   SubmitApplicationBehaviourCommand.New(r => r.ApplicationId = Id),
                                   SubmitClientWatermarkCommand.New(r =>
                                                                        {
                                                                            r.ApplicationId = Id;
                                                                            r.AccountId = Customer.Id;
                                                                        }),
                                   CreateBusinessFixedInstallmentLoanApplicationWbUkCommand.New(r =>
                                                                                                    {
                                                                                                        r.AccountId =Customer.Id;
                                                                                                        r.OrganisationId=Company.Id;
                                                                                                        r.ApplicationId=Id;
                                                                                                        r.BusinessPaymentCardId=Company.GetPaymentCard();
                                                                                                        r.BusinessBankAccountId=Company.GetBankAccount();
                                                                                                        r.MainApplicantBankAccountId=Customer.GetBankAccount();
                                                                                                        r.MainApplicantPaymentCardId=Customer.GetPaymentCard();
                                                                                                    }),
                                   SubmitNumberOfGuarantorsCommand.New(r =>
                                                                           {
                                                                               r.AccountId = Customer.Id;
                                                                               r.ApplicationId = Id;
                                                                               r.NumberOfGuarantors =Company.GetSecondaryDirectors().ToList().Count;
                                                                           })
                               };

            Drive.Api.Commands.Post(requests);

            Do.Until(() =>Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.Any(app => app.ApplicationEntity.ExternalId == Id));
            Drive.Api.Commands.Post(VerifyMainBusinessApplicantWbCommand.New(r =>
                                                                                  {
                                                                                      r.AccountId = Customer.Id;
                                                                                      r.ApplicationId = Id;
                                                                                  }));

            Do.Until(() => (ApplicationDecisionStatus)Enum.Parse(typeof(ApplicationDecisionStatus), Drive.Api.Queries.Post(new GetApplicationDecisionQuery { ApplicationId = Id }).Values["ApplicationDecisionStatus"].Single()) == Decision);

            if (Decision == ApplicationDecisionStatus.Declined)
                return new BusinessApplication(Id);

            Drive.Api.Commands.Post(new SignBusinessApplicationWbUkCommand{AccountId = Customer.Id, ApplicationId = Id});

            Int32 previous = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            Do.While(() =>
                         {
                             Int32 current =Do.Until(() =>Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Count);
                             if (previous != current) stopwatch.Restart();
                             previous = current;
                             return stopwatch.Elapsed < TimeSpan.FromSeconds(5);
                         });

            return new BusinessApplication(Id);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
            _setPromiseDateAndLoanTerm();
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
                            if(LoanTerm > 0)
                            {
                                r.Term = LoanTerm;
                            }
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
            Do.With.Message("Payments did not create the application in time").Until(() => Drive.Db.Payments.BusinessFixedInstallmentLoanApplications.Any(app => app.ApplicationEntity.ExternalId == Id));

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
                WaitForRiskDecisionToBeMade();
                return new BusinessApplication(Id);
            }

            /* STEP 5
             * now let send a payment comnand representing payment term sliders adjustment*/
            Drive.Api.Commands.Post(UpdateLoanTermWbUkCommand.New(r=>
                                                                      {
                                                                          r.ApplicationId = Id;
                                                                          if (LoanTerm > 0)
                                                                          {
                                                                              r.Term = LoanTerm;
                                                                          }
                                                                      })); //use default for term - same as for create applicatiomn

            /* STEP 6
             * And the main applicant signs the application */
            Drive.Api.Commands.Post(new SignBusinessApplicationWbUkCommand { AccountId = Customer.Id, ApplicationId = Id });

            /* STEP 7 
            * And I wait for the decision i want - PLEASE REMEBER THAT THE DEFAULT ONE IS ACCEPTED 
            * Set timeout to two minutes to compensate for long risk decision */
            WaitForRiskDecisionToBeMade();

            /* STEP 8
             * And I wait for Payments to create the application, but only when the expected decision is Accepted */
            if (Decision == ApplicationDecisionStatus.Accepted)
            {
                Do.With.Message("The initial transactions have not been created").Until(() => Drive.Db.Payments.Applications.Single(a => a.ExternalId == Id).Transactions.Count == 3);
            }
            return new BusinessApplication(Id);
        }

        private void WaitForRiskDecisionToBeMade()
        {
            Do.With.Timeout(2).Message("Risk didn't return expected status \"{0}\"", Decision).Until(
                () =>
                (ApplicationDecisionStatus)
                Enum.Parse(typeof (ApplicationDecisionStatus),
                           Drive.Api.Queries.Post(new GetApplicationDecisionQuery {ApplicationId = Id}).Values[
                               "ApplicationDecisionStatus"].Single()) == Decision);
        }

        #region Public Methods

        public void SignApplicationForSecondaryDirectors()
        {
            var guarantors = Drive.Db.ContactManagement.DirectorOrganisationMappings.Where(
                director => director.OrganisationId == Company.Id && director.DirectorLevel > 0);


            var requests = guarantors.Select(guarantor => new SignBusinessApplicationWbUkCommand {AccountId = guarantor.AccountId, ApplicationId = Id});

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
    }
}

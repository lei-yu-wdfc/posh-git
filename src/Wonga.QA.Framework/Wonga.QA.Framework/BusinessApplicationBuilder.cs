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
            Guarantors = new List<CustomerBuilder>();
            SignGuarantors = true;
            CreateGuarantors = true;
        }

        public override Application Build()
        {
            _setPromiseDateAndLoanTerm();

            /* STEP 0 - If i have guarantors i need to send the preaccepted decision
             * The accepted one comes only after the guarantors did their part (created+signed)
             */

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
                            r.LoanAmount = LoanAmount;
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
                                                                                r.NumberOfGuarantors = Guarantors.Count;
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

            /* STEP 7.1
             * I create the guarantors 
             * I wait for them to be present into the comms db
             * For each -> I sign
             * TODO : Throw Exception for WithUnsignedGuarantors with WithPartialGuarantors
             * TODO : Think of the implication of ExpectedDecision(...) with WithPartialGuarantors() and WithUnsignedGuarantors()
             * TODO : Sign/Build only some of the guarantors
             */
            if (Guarantors.Count > 0 && CreateGuarantors)
            {
                foreach (var guarantorCustomerBuilder in Guarantors)
                {
                    guarantorCustomerBuilder.Build();
                }

                Do.With.Message("The guarantors and company mappings has not been created").Until(
                    () =>
                    Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(
                        p => p.OrganisationId == Company.Id && p.DirectorLevel > 0) == Guarantors.Count);

                if(SignGuarantors)
                    SignApplicationForSecondaryDirectors();
            }


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

                guarantorCustomerBuilder.Build();
            }
        }

        #endregion

        #region "With" Helper Methods

        public BusinessApplicationBuilder WithGuarantors(List<CustomerBuilder> guarantors)
        {
            Guarantors = guarantors;
            return this;
        }

        /// <summary>
        /// Choose this option if you want an incomplete L0 jurney when the guarantors didnt start their jurney yet
        /// </summary>
        /// <returns></returns>
        public BusinessApplicationBuilder WithPartialGuarantors()
        {
            CreateGuarantors = false;
            return this;
        }

        /// <summary>
        /// Choose this option if you want an incomplete L0 when the guarantors did start their jurney but they didnt sign yet
        /// </summary>
        /// <returns></returns>
        public BusinessApplicationBuilder WithUnsignedGuarantors()
        {
            SignGuarantors = false;
            return this;
        }

        #endregion
    }
}

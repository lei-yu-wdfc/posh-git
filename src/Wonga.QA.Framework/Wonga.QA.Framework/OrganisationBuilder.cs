using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using System.Linq;
using Wonga.QA.Framework.Db;

namespace Wonga.QA.Framework
{
    public class OrganisationBuilder
    {
        private readonly Guid _id;
        private Customer _primaryApplicant;
        private int _numberOfSecondaryDirector;
        private int _organisationNumber;
        
        private OrganisationBuilder(Customer customer)
        {
            _id = Guid.NewGuid();
            _numberOfSecondaryDirector = 1;
            _organisationNumber = Data.RandomInt(1, 99999999);
            _primaryApplicant = customer;
        }

        public static OrganisationBuilder New(Customer customer)
        {
            return new OrganisationBuilder(customer);
        }

        public OrganisationBuilder WithSoManySecondaryDirectors(int number)
        {
            _numberOfSecondaryDirector = number;
            return this;
        }

        public OrganisationBuilder WithOrganisationNumber(int orgNo)
        {
            _organisationNumber = orgNo;
            var Db = new DbDriver();
            var existingOrg = Db.ContactManagement.OrganisationDetails.SingleOrDefault(o => o.RegisteredNumber == orgNo.ToString()) ;
            if (existingOrg != null)
            {
                Db.ContactManagement.OrganisationDetails.DeleteOnSubmit(existingOrg);
                existingOrg.Submit();
            }
            
            return this;
        }

        public Organisation Build()
        {
            var requests = new List<ApiRequest>();

            requests.AddRange(new ApiRequest[]
                                  {
                                      SaveOrganisationDetailsCommand.New(r =>
                                                                             {
                                                                                 r.OrganisationId = _id;
                                                                                 r.RegisteredNumber =_organisationNumber;
                                                                             }),
                                      AddBusinessBankAccountWbUkCommand.New(r => r.OrganisationId = _id),
                                      AddBusinessPaymentCardWbUkCommand.New(r => r.OrganisationId = _id),
                                      AddPrimaryOrganisationDirectorCommand.New(r =>
                                                                                    {
                                                                                        r.OrganisationId = _id;
                                                                                        r.AccountId =_primaryApplicant.Id;
                                                                                        r.Email =_primaryApplicant.Email;
                                                                                    })
                                  });

           
            
            Driver.Api.Commands.Post(requests);            

            return new Organisation(_id);
        }    
  
        public void BuildSecondaryDirectors()
        {
            List<ApiRequest> requests = new List<ApiRequest>();

            for (int x = 0; x < _numberOfSecondaryDirector; x++)
            {
                Guid guarantorAccountId = Guid.NewGuid();
                CustomerBuilder.New(guarantorAccountId).Build();
                requests.Add(AddSecondaryOrganisationDirectorCommand.New(r =>
                                                                             {
                                                                                 r.OrganisationId = _id;
                                                                                 r.AccountId = guarantorAccountId;
                                                                             }));   
             
                
            }

            Driver.Api.Commands.Post(requests);

            Do.Until(
                () =>
                Driver.Db.ContactManagement.DirectorOrganisationMappings.Count(
                    director => director.OrganisationId == _id && director.DirectorLevel > 0) == _numberOfSecondaryDirector);

        }
    }
}

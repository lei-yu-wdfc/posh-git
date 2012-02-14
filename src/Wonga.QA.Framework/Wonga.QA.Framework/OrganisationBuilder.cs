using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;


namespace Wonga.QA.Framework
{
    public class OrganisationBuilder
    {
        private Guid _id;
        private Customer _primaryApplicant;
        private int _numberOfSecondaryDirector;
        
        private OrganisationBuilder()
        {
            _id = Guid.NewGuid();
            _numberOfSecondaryDirector = 1;
        }

        public static OrganisationBuilder New(Guid orgId)
        {
            return new OrganisationBuilder {_id = orgId};
        }

        public OrganisationBuilder WithPrimaryApplicant(Customer primaryApplicant)
        {
            _primaryApplicant = primaryApplicant;
            return this;
        }

        public OrganisationBuilder WithSoManySecondaryDirectors(int number)
        {
            _numberOfSecondaryDirector = number;
            return this;
        }

        public Organisation Build()
        {
            List<ApiRequest> requests = new List<ApiRequest>();

            requests.AddRange(new ApiRequest[]
                                           {
                                               SaveOrganisationDetailsCommand.New(r=>r.OrganisationId = _id),
                                               AddBusinessBankAccountWbUkCommand.New(r=>r.OrganisationId = _id),
                                               AddBusinessPaymentCardWbUkCommand.New(r=>r.OrganisationId = _id),
                                               AddPrimaryOrganisationDirectorCommand.New(r=> { r.OrganisationId = _id; r.AccountId = _primaryApplicant.Id; })                                               
                                           });

            for (int x = 0; x < _numberOfSecondaryDirector; x++ )
            {
                requests.Add(AddSecondaryOrganisationDirectorCommand.New(r => r.OrganisationId = _id));
            }
            
            Driver.Api.Commands.Post(requests);            

            return new Organisation(_id);
        }
    }
}

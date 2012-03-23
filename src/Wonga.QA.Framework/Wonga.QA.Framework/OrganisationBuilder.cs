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
        private readonly Customer _primaryApplicant;
        private int _numberOfSecondaryDirector;
        private String _organisationNumber;

        private OrganisationBuilder(Customer primaryApplicant)
        {
            _id = Guid.NewGuid();
            //_numberOfSecondaryDirector = 1;
            _organisationNumber = Get.RandomInt(1, 99999999).ToString();
            _primaryApplicant = primaryApplicant;
        }

        public static OrganisationBuilder New(Customer primaryApplicant)
        {
            return new OrganisationBuilder(primaryApplicant);
        }

        [Obsolete]
        public OrganisationBuilder WithSoManySecondaryDirectors(int number)
        {
            _numberOfSecondaryDirector = number;
            return this;
        }

        public OrganisationBuilder WithOrganisationNumber(String orgNo)
        {
            _organisationNumber = orgNo;
            var db = new DbDriver();
            var existingOrg = db.ContactManagement.OrganisationDetails.SingleOrDefault(o => o.RegisteredNumber == orgNo);
            if (existingOrg != null)
            {
                db.ContactManagement.OrganisationDetails.DeleteOnSubmit(existingOrg);
                existingOrg.Submit();
            }

            return this;
        }


        public Organisation Build()
        {
            var requests = new List<ApiRequest>();

            var addBusinessPaymentCardWbUkCommand = AddBusinessPaymentCardWbUkCommand.New(r => r.OrganisationId = _id);
            requests.AddRange(new ApiRequest[]
                                  {
                                      SaveOrganisationDetailsCommand.New(r =>
                                                                             {
                                                                                 r.OrganisationId = _id;
                                                                                 r.RegisteredNumber =_organisationNumber;
                                                                             }),
                                      AddBusinessBankAccountWbUkCommand.New(r => r.OrganisationId = _id),
                                      addBusinessPaymentCardWbUkCommand,
                                      AddPrimaryOrganisationDirectorCommand.New(r =>
                                                                                    {
                                                                                        r.OrganisationId = _id;
                                                                                        r.AccountId =_primaryApplicant.Id;
                                                                                        r.Email =_primaryApplicant.Email;
                                                                                    }),
                                        SavePaymentCardBillingAddressCommand.New(s =>
                                                                                     {
                                                                                         s.PaymentCardId =
                                                                                             addBusinessPaymentCardWbUkCommand
                                                                                                 .PaymentCardId;
                                                                                     })
                                  });



            Drive.Api.Commands.Post(requests);

            return new Organisation(_id);
        }

        [Obsolete]
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

            Drive.Api.Commands.Post(requests);

            Do.Until(
                () =>
                Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(
                    director => director.OrganisationId == _id && director.DirectorLevel > 0) == _numberOfSecondaryDirector);

        }

        [Obsolete]
        public void BuildSecondaryDirectors(List<Guid> secondaryDirectorsIds)
        {
            List<ApiRequest> requests = new List<ApiRequest>();

            for (var i = 0; i < secondaryDirectorsIds.Count; i++)
            {
                var guarantorId = i;
                requests.Add(AddSecondaryOrganisationDirectorCommand.New(r =>
                {
                    r.OrganisationId = _id;
                    r.AccountId = secondaryDirectorsIds[guarantorId];
                }));
            }

            Drive.Api.Commands.Post(requests);

            Do.Until(() =>Drive.Db.ContactManagement.DirectorOrganisationMappings.Count(director => director.OrganisationId == _id && director.DirectorLevel > 0) == _numberOfSecondaryDirector);
        }
    }
}

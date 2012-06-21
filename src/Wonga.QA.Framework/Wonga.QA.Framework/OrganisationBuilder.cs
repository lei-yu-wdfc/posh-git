using System;
using System.Collections.Generic;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Comms.ContactManagement.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk;
using Wonga.QA.Framework.Core;
using System.Linq;
using Wonga.QA.Framework.Db;
using System.Threading;

namespace Wonga.QA.Framework
{
    public class OrganisationBuilder
    {
        private readonly Guid _id;
        private readonly Customer _primaryApplicant;
        private String _organisationNumber;

        private OrganisationBuilder(Customer primaryApplicant)
        {
            _id = Guid.NewGuid();
            _organisationNumber = Get.RandomAlphaNumeric(5, 8).PadLeft(8, '0');
            _primaryApplicant = primaryApplicant;
        }

        public static OrganisationBuilder New(Customer primaryApplicant)
        {
            return new OrganisationBuilder(primaryApplicant);
        }

        public OrganisationBuilder WithOrganisationNumber(String orgNo)
        {
            _organisationNumber = orgNo;
            Drive.Data.ContactManagement.Db.OrganisationDetails.Delete(RegisteredNumber: orgNo);
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
    }
}

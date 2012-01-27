using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public class CustomerBuilder
    {
        private Guid _id;
        private Guid _verification;
        private String _employerName;

        private CustomerBuilder()
        {
            _id = Data.GetId();
            _verification = Data.GetId();
            _employerName = Data.GetEmployerName();
        }

        public static CustomerBuilder New()
        {
            return new CustomerBuilder();
        }

        public static CustomerBuilder New(Guid id)
        {
            return new CustomerBuilder { _id = id };
        }

        public CustomerBuilder WithEmployer(String name)
        {
            _employerName = name;
            return this;
        }

        public Customer Build()
        {
            List<ApiRequest> requests = new List<ApiRequest>
            {
                CreateAccountCommand.Random(r => r.AccountId = _id),
                SaveSocialDetailsCommand.Random(r => r.AccountId = _id),
                SavePasswordRecoveryDetailsCommand.Random(r => r.AccountId = _id),
                SaveContactPreferencesCommand.Random(r => r.AccountId = _id),
                CompleteMobilePhoneVerificationCommand.Random(r => r.VerificationId = _verification),
            };

            switch (Config.AUT)
            {
                case AUT.Za:
                    requests.AddRange(new ApiRequest[]
                    {
                        SaveCustomerDetailsZaCommand.Random(r =>
                        {
                            r.AccountId = _id;
                            r.NationalNumber = Data.GetNIN((Date) r.DateOfBirth, (GenderEnum) r.Gender == GenderEnum.Female);
                            if ((GenderEnum)r.Gender != GenderEnum.Female)
                                r.MaidenName = null;
                        }),
                        SaveCustomerAddressZaCommand.Random(r =>
                        {
                            r.AccountId = _id;
                            r.CountryCode = CountryCodeEnum.ZA.ToString().ToUpper();
                        }),
                        AddBankAccountZaCommand.Random(r => r.AccountId = _id),
                        SaveEmploymentDetailsZaCommand.Random(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        }),
                        VerifyMobilePhoneZaCommand.Random(r =>
                        {
                            r.AccountId = _id;
                            r.VerificationId = _verification;
                        }),
                    });
                    break;
                default:
                    throw new NotImplementedException();
            }

            Drivers.Api.Commands.Post(requests);
            return new Customer(_id);
        }
    }
}

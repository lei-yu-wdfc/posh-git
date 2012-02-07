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
        private Drivers _drivers;

        private CustomerBuilder()
        {
            _id = Data.GetId();
            _verification = Data.GetId();
            _employerName = Data.GetEmployerName();
            _drivers = new Drivers();
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
                CreateAccountCommand.New(r => r.AccountId = _id),
                SaveSocialDetailsCommand.New(r => r.AccountId = _id),
                SavePasswordRecoveryDetailsCommand.New(r => r.AccountId = _id),
                SaveContactPreferencesCommand.New(r => r.AccountId = _id),
                CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = _verification),
            };

            switch (Config.AUT)
            {
                case AUT.Za:
                    requests.AddRange(new ApiRequest[]
                    {
                        SaveCustomerDetailsZaCommand.New(r => r.AccountId = _id ),
                        SaveCustomerAddressZaCommand.New(r => r.AccountId = _id ),
                        AddBankAccountZaCommand.New(r => r.AccountId = _id),
                        SaveEmploymentDetailsZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        }),
                        VerifyMobilePhoneZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.VerificationId = _verification;
                        }),
                    });
                    break;

                case AUT.Ca:
                    requests.AddRange(new ApiRequest[]
                    {
                        SaveCustomerDetailsCaCommand.New(r => r.AccountId = _id ),
                        SaveCustomerAddressCaCommand.New(r => r.AccountId = _id ),
                        AddBankAccountCaCommand.New(r => r.AccountId = _id),
                        SaveEmploymentDetailsCaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        }),
                        VerifyMobilePhoneCaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.VerificationId = _verification;
                        })
                    });
                    break;

                default:
                    throw new NotImplementedException();
            }

            _drivers.Api.Commands.Post(requests);

            Do.Until(() => _drivers.Db.Payments.AccountPreferences.Single(a => a.AccountId == _id));
            Do.Until(() => _drivers.Db.Risk.RiskAccounts.Single(a => a.AccountId == _id));

            return new Customer(_id);
        }
    }
}

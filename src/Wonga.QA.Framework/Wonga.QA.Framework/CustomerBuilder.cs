using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
		private Decimal _netMonthlyIncome;
    	private GenderEnum _gender;
    	private String _nationalNumber;
        private String _foreName;
	    private Date _nextPayDate;
    	private Date _dateOfBirth;
        private String _middleName;
        private String _surname;
        private String _houseNumber;
        private ProvinceEnum _province;
        private String _email;
        private String _houseName;
        private String _postcode;
        private String _street;
        private String _flat;
        private String _district;
        private String _town;
        private String _county;
        private Guid _bankAccountId;
    	

        private CustomerBuilder()
        {
            _id = Data.GetId();
            _verification = Data.GetId();
            _employerName = Data.GetEmployerName();
        	_netMonthlyIncome = Data.RandomInt(1000, 2000);
			_dateOfBirth = Data.GetDoB();
			_gender = GenderEnum.Female;
			if(Config.AUT == AUT.Za) //TODO implement nationalNumber generators for other regions
        		_nationalNumber = Data.GetNIN(_dateOfBirth.DateTime, _gender == GenderEnum.Female);
            _surname = Data.GetName();
            _middleName = Data.GetMiddleName();
            _foreName = Data.GetName();
            _province = ProvinceEnum.ON;
            _houseNumber = Data.RandomInt(1, 100).ToString(CultureInfo.InvariantCulture);
            _houseName = Data.RandomString(8);
            if (Config.AUT == AUT.Wb || Config.AUT == AUT.Uk)
            {
                _postcode = "SW6 6PN";
            }
            else if (Config.AUT == AUT.Za)
            {
                _postcode = "0300";
            }
            else if (Config.AUT == AUT.Ca)
            {
                _postcode = "K0A0A0";
            	_province = ProvinceEnum.ON;
            }
            _street = Data.RandomString(15);
            _flat = Data.RandomString(4);
            _district = Data.RandomString(15);
            _town = Data.RandomString(15);
            _county = Data.RandomString(15);			
        	_nextPayDate = Data.GetNextPayDate();
			_email = Data.GetEmail();
            _bankAccountId = Data.GetId();
        	
            _province = ProvinceEnum.ON;
        }

        public static CustomerBuilder New()
        {
            return new CustomerBuilder();
        }

        public static CustomerBuilder New(Guid id)
        {
            return new CustomerBuilder { _id = id };
        }

        public CustomerBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public CustomerBuilder WithEmployer(string employerName)
        {
            _employerName = employerName;
            return this;
        }

		public CustomerBuilder WithNetMonthlyIncome(decimal netMonthlyIncome)
		{
			_netMonthlyIncome = netMonthlyIncome;
			return this;
		}
        
		public CustomerBuilder WithGender(GenderEnum gender)
		{
			_gender = gender;
			return this;
		}

		public CustomerBuilder WithNationalNumber(string nationalNumber)
		{
			_nationalNumber = nationalNumber;
			return this;
		}

		public CustomerBuilder WithNextPayDate(Date date)
		{
			_nextPayDate = date;
			return this;
		}

		public CustomerBuilder WithDateOfBirth(Date date)
		{
			_dateOfBirth = date;
			return this;
		}

        public CustomerBuilder WithForename(String foreName)
        {
            _foreName = foreName;
            return this;
        }

        public CustomerBuilder WithMiddleName(String middleName)
        {
            _middleName = middleName;
            return this;
        }

        public CustomerBuilder WithSurname(String surname)
        {
            _surname = surname;
            return this;
        }
        
        public CustomerBuilder ForProvince(ProvinceEnum province)
        {
            _province = province;
            return this;
        }

        public CustomerBuilder WithHouseNumberInAddress(String houseNumber)
        {
            _houseNumber = houseNumber;
            return this;
        }

        public CustomerBuilder WithHouseNameInAddress(String houseName)
        {
            _houseName = houseName;
            return this;
        }

        public CustomerBuilder WithPostcodeInAddress(String postcode)
        {
            _postcode = postcode;
            return this;
        }

        public CustomerBuilder WithStreetInAddress(String street)
        {
            _street = street;
            return this;
        }

        public CustomerBuilder WithFlatInAddress(String flat)
        {
            _flat = flat;
            return this;
        }

        public CustomerBuilder WithDistrictInAddress(String district)
        {
            _district = district;
            return this;
        }

        public CustomerBuilder WithTownInAddress(String town)
        {
            _town = town;
            return this;
        }

        public CustomerBuilder WithCountyInAddress(String county)
        {
            _county = county;
            return this;
        }

		public CustomerBuilder WithProvinceInAddress(ProvinceEnum province)
		{
			_province = province;
			return this;
		}

        public Customer Build()
        {
			_nextPayDate.DateFormat = DateFormat.Date;
			_dateOfBirth.DateFormat = DateFormat.Date;

            var requests = new List<ApiRequest>
            {
                CreateAccountCommand.New(r => { r.AccountId = _id;
                                                  r.Login = _email;
                }),
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
                        SaveCustomerDetailsZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.MiddleName = _middleName;
                            r.Email = _email;
                        	r.NationalNumber = _nationalNumber;
                        	r.DateOfBirth = _dateOfBirth;
                        	r.Gender = _gender;
                        }),
                        SaveCustomerAddressZaCommand.New(r =>
                                                             {
                                                                 r.AccountId = _id;
                                                                 r.HouseNumber = _houseNumber;
                                                                 r.HouseName = _houseName;
                                                                 r.Postcode = _postcode;
                                                                 r.Street = _street;
                                                                 r.Flat = _flat;
                                                                 r.District = _district;
                                                                 r.Town = _town;
                                                                 r.County = _county;
                                                             } ),
                        AddBankAccountZaCommand.New(r => { r.AccountId = _id;
                                                             r.BankAccountId = _bankAccountId;
                        }),
                        SaveEmploymentDetailsZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        	r.NextPayDate = _nextPayDate;
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
                        SaveCustomerDetailsCaCommand.New(r => 
                        { 
                            r.AccountId = _id;
                            r.Forename = _foreName;
                            r.MiddleName = _middleName;
                            r.Surname = _surname;
                            r.Email = _email;
                        	r.DateOfBirth = _dateOfBirth;
                        	r.Gender = _gender;
                        }),                       
                        SaveCustomerAddressCaCommand.New(r => {
                                                                 r.AccountId = _id;
                                                                 r.HouseNumber = _houseNumber;
                                                                 r.HouseName = _houseName;
                                                                 r.Postcode = _postcode;
                                                                 r.Street = _street;
                                                                 r.Flat = _flat;
                                                                 r.District = _district;
                                                                 r.Town = _town;
                                                                 r.County = _county;
																 r.Province = _province;
                                                                 r.Province = _province;
                        } ),
                        AddBankAccountCaCommand.New(r => { r.AccountId = _id;
                                                             r.BankAccountId = _bankAccountId;
                        }),
                        SaveEmploymentDetailsCaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        	r.NetMonthlyIncome = _netMonthlyIncome;
                        }),
                        VerifyMobilePhoneCaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.VerificationId = _verification;
                        })
                    });
                    break;

                case AUT.Wb:
                    requests.AddRange(new ApiRequest[]
                    {
                        SaveCustomerDetailsUkCommand.New(r=>
                                                             {
                                                                 r.AccountId = _id; 
                                                                 r.MiddleName = _middleName;
                                                                 r.Surname = _surname;
                                                                 r.Forename = _foreName;
                                                                 r.DateOfBirth = _dateOfBirth;
                                                                 r.Email = _email;
                                                             }),
                        SaveCustomerAddressUkCommand.New(r=>
                                                             {
                                                                 r.AccountId = _id;
                                                                 r.HouseNumber = _houseNumber;
                                                                 r.HouseName = _houseName;
                                                                 r.Postcode = _postcode;
                                                                 r.Street = _street;
                                                                 r.Flat = _flat;
                                                                 r.District = _district;
                                                                 r.Town = _town;
                                                                 r.County = _county;
                                                             }),
                        AddBankAccountUkCommand.New(r=>r.AccountId = _id),
                        AddPaymentCardCommand.New(r=>r.AccountId = _id)
                    });
                    break;

                case AUT.Uk:
                    requests.AddRange(new ApiRequest[]
					{
						SaveCustomerDetailsUkCommand.New(r=>
						                                     {
						                                         r.AccountId = _id;
						                                         r.Forename = _foreName;
						                                         r.Surname = _surname;
						                                         r.Email = _email;
						                                         r.DateOfBirth = _dateOfBirth;
						                                     }),
					    SaveCustomerAddressUkCommand.New(r =>
					                                         {
					                                             r.AccountId = _id;
                                                                 r.HouseNumber = _houseNumber;
                                                                 r.HouseName = _houseName;
                                                                 r.Postcode = _postcode;
                                                                 r.Street = _street;
                                                                 r.Flat = _flat;
                                                                 r.District = _district;
                                                                 r.Town = _town;
                                                                 r.County = _county;
					                                         }),
						AddBankAccountUkCommand.New(r => { r.AccountId = _id; }),
						AddPaymentCardCommand.New(r => { r.AccountId = _id; }),
						SaveEmploymentDetailsUkCommand.New(r =>
						{
							r.AccountId = _id;
							r.EmployerName = _employerName;
						}),
						VerifyMobilePhoneUkCommand.New(r =>
						{
						    r.AccountId = _id;
						    r.VerificationId = _verification;
						})
					});
                    break;

                default:
                    throw new NotImplementedException();
            }

            Driver.Api.Commands.Post(requests);

            Do.Until(() => Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == _id));
            Do.Until(() => Driver.Db.Risk.RiskAccounts.Single(a => a.AccountId == _id));
            
            switch (Config.AUT)
            {
                case AUT.Wb:
                    Do.Until(
                        () =>
                        Driver.Db.Payments.AccountPreferences.Single(ap => ap.AccountId == _id).PaymentCardsBaseEntity);
                    break;

				case AUT.Ca:
					Do.Until(
						() =>
						Driver.Db.Payments.BankAccountsBases.Single(bab => bab.ExternalId == _bankAccountId));
					break;
            }
            
            return new Customer(_id, _email, _bankAccountId);
        }
    }
}

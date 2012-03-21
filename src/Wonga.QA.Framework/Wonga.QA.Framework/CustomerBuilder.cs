﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Comms;

namespace Wonga.QA.Framework
{
    public class CustomerBuilder
    {
        private Guid _id;
        private Guid _verification;
        private object _employerName;
        private String _employerStatus;
		private Decimal _netMonthlyIncome;
    	private GenderEnum _gender;
    	private String _nationalNumber;
        private String _foreName;
	    private Date _nextPayDate;
    	private Date _dateOfBirth;
        private object _middleName;
        private String _surname;
    	private String _maidenName;
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
        private String _phoneNumber;
		private String _mobileNumber;
    	private Int64? _bankAccountNumber;
        private Int64 _paymentCardNumber;
        

        private CustomerBuilder()
        {
            _id = Data.GetId();
            _verification = Data.GetId();
            _employerName = Data.GetEmployerName();
            _employerStatus = Data.GetEmploymentStatus();
        	_netMonthlyIncome = Data.RandomInt(1000, 2000);
			_dateOfBirth = Data.GetDoB();
			_gender = GenderEnum.Female;
			if(Config.AUT == AUT.Za) //TODO implement nationalNumber generators for other regions
        		_nationalNumber = Data.GetNIN(_dateOfBirth.DateTime, _gender == GenderEnum.Female);
            _surname = Data.GetName();
            _middleName = Data.GetMiddleName();
        	_maidenName = Data.GetName();
            _foreName = Data.GetName();
            _province = ProvinceEnum.ON;
            _houseNumber = Data.RandomInt(1, 100).ToString(CultureInfo.InvariantCulture);
            _houseName = Data.RandomString(8);
            _phoneNumber = Data.GetPhone();
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
			_email = Data.RandomEmail();
            _bankAccountId = Data.GetId();
        	
            _province = ProvinceEnum.ON;
        	_bankAccountNumber = null;
            _paymentCardNumber = 4444333322221111;
            _mobileNumber = Data.GetMobilePhone();
        }

        public static CustomerBuilder New()
        {
            return new CustomerBuilder();
        }

        public static CustomerBuilder New(Guid id)
        {
            return new CustomerBuilder { _id = id };
        }

        //public CustomerBuilder WithEmail(string email)
        //{
        //    _email = email;
        //    return this;
        //}

        public CustomerBuilder WithEmployer(string employerName)
        {
            _employerName = employerName;
            return this;
        }

        public CustomerBuilder WithEmployer(RiskMask mask)
        {
            _employerName = mask;
            return this;
        }

        public CustomerBuilder WithEmployerStatus(string employerStatus)
        {
            _employerStatus = employerStatus;
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

        public CustomerBuilder WithEmailAddress(String email)
        {
            _email = email;
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

        public CustomerBuilder WithMiddleName(RiskMask mask)
        {
            _middleName = mask;
            return this;
        }

        public CustomerBuilder WithSurname(String surname)
        {
            _surname = surname;
            return this;
        }

		public CustomerBuilder WithMaidenName(String maidenName)
		{
			_maidenName = maidenName;
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

        public CustomerBuilder WithPhoneNumber(String phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public CustomerBuilder WithProvinceInAddress(ProvinceEnum province)
	    {
		    _province = province;
		    return this;
	    }

		public CustomerBuilder WithBankAccountNumber(Int64 bankAccountNumber)
		{
			_bankAccountNumber = bankAccountNumber;
			return this;
		}

        public CustomerBuilder WithPaymentCardNumber(Int64 cardNumber)
        {
            _paymentCardNumber = cardNumber;
            return this;
        }

        public CustomerBuilder WithMobileNumber(String mobileNumber)
        {
            _mobileNumber = mobileNumber;
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
            };

            switch (Config.AUT)
            {
                case AUT.Za:
                    requests.AddRange(new ApiRequest[]
                    {
                        SaveCustomerDetailsZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                        	r.Forename = _foreName;
                            r.MiddleName = _middleName;
                        	r.Surname = _surname;
                            r.Email = _email;
                        	r.NationalNumber = _nationalNumber;
                        	r.DateOfBirth = _dateOfBirth;
                        	r.Gender = _gender;
                        	r.MaidenName = _gender == GenderEnum.Female ? _maidenName : null;
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
                        AddBankAccountZaCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (_bankAccountNumber.HasValue)
                                                    		{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}
                                                    	}),
                        SaveEmploymentDetailsZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        	r.NextPayDate = _nextPayDate;
                        	r.Status = _employerStatus;
                        }),
                        VerifyMobilePhoneZaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.VerificationId = _verification;
                        	r.MobilePhone = _mobileNumber;
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
                            r.NationalNumber = _nationalNumber;
                            r.HomePhone = _phoneNumber;
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
                        AddBankAccountCaCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (_bankAccountNumber.HasValue)
                                                    		{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}
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
                        AddBankAccountUkCommand.New(r=>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		if (_bankAccountNumber.HasValue)
                                                    		{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}
                                                    	}),
                        AddPaymentCardCommand.New(r =>
						                              {
						                                  r.AccountId = _id;
						                                  r.Number = _paymentCardNumber;
						                              }),
                        VerifyMobilePhoneUkCommand.New(r=>
                                                           {
                                                               r.AccountId = _id;
                                                               r.Forename = _foreName;
                                                               r.VerificationId = _verification;
                                                               r.MobilePhone = _mobileNumber;
                                                           }),
                        CompleteMobilePhoneVerificationCommand.New(r=> r.VerificationId = _verification)
                    });
                    break;

                case AUT.Uk:
                    requests.AddRange(new ApiRequest[]
					{
						SaveCustomerDetailsUkCommand.New(r=>
						                                     {
						                                         r.AccountId = _id;
						                                         r.Forename = _foreName;
						                                         r.MiddleName = _middleName;
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
						AddBankAccountUkCommand.New(r =>
						                            	{
						                            		r.AccountId = _id;
						                            	    r.BankAccountId = _bankAccountId;
						                            		if (_bankAccountNumber.HasValue)
						                            		{
						                            			r.AccountNumber = _bankAccountNumber;
						                            		}
						                            	}),
						AddPaymentCardCommand.New(r =>
						                              {
						                                  r.AccountId = _id;
						                                  r.Number = _paymentCardNumber;
						                              }),
						SaveEmploymentDetailsUkCommand.New(r =>
						{
							r.AccountId = _id;
							r.EmployerName = _employerName;
						    r.Status = _employerStatus;
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

        	Do.With().Timeout(2).Until(() => Driver.Db.Ops.Accounts.Single(a => a.ExternalId == _id));
            Do.With().Timeout(2).Until(() => Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == _id));
            Do.With().Timeout(2).Until(() => Driver.Db.Risk.RiskAccounts.Single(a => a.AccountId == _id));
            
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

				case AUT.Za:
            		{
						var mobilePhoneVerification = Do.Until(() => Driver.Db.Comms.MobilePhoneVerifications.Single(a => a.AccountId == _id));

            			Driver.Api.Commands.Post(new CompleteMobilePhoneVerificationCommand
            			                         	{
            			                         		Pin = mobilePhoneVerification.Pin,
            			                         		VerificationId = mobilePhoneVerification.VerificationId
            			                         	});
            			Do.With().Timeout(2).Until(() => Driver.Db.Comms.CustomerDetails.Single(a => a.AccountId == _id).MobilePhone);
            		}
            		break;
            }
            
            return new Customer(_id, _email, _bankAccountId);
        }

        public void ScrubForename(String forename)
        {
            var db = new DbDriver();
            var customerDetailEntities = db.Comms.CustomerDetails.Where(cd => cd.Forename == forename).ToList();
            foreach (CustomerDetailEntity customerDetailEntity in customerDetailEntities)
            {
                customerDetailEntity.Forename = Data.GetName();
            }
            db.Comms.SubmitChanges();
        }

        public void ScrubSurname(String surname)
        {
            var db = new DbDriver();
            var customerDetailEntities = db.Comms.CustomerDetails.Where(cd => cd.Surname == surname).ToList();
            foreach (CustomerDetailEntity customerDetailEntity in customerDetailEntities)
            {
                customerDetailEntity.Surname = Data.GetName();
            }
            db.Comms.SubmitChanges();
        }
    }
}

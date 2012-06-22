using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Simple.Data;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Za;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Api.Requests.Ops.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Ops.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Ops.Commands.Za;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Payments.Commands.Za;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Risk.Commands.Za;
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
        private String _homePhoneNumber;
        private String _mobileNumber;
    	private String _bankAccountNumber;
        private Int64 _paymentCardNumber;
        private string _paymentCardSecurityCode;
        private string _paymentCardType;
        private String _institutionNumber;
        private int _numberOfDependants;
        private String _bankCode;
        private IncomeFrequencyEnum _incomeFrequency;

        private String _branchNumber;

        public Guid Id { get { return _id; } }
        public String Email { get { return _email; } }
        public String Forename { get { return _foreName; } }
        public String Surname { get { return _surname; } }

        public Date DateOfBirth { get { return _dateOfBirth; } }

        public string Town { get { return _town; } }

        private CustomerBuilder()
        {
            _id = Get.GetId();
            _verification = Get.GetId();
            _employerName = Get.GetEmployerName();
            _employerStatus = Get.GetEmploymentStatus();
            _netMonthlyIncome = 1500;
            _dateOfBirth = Get.GetDoB();
            _gender = GenderEnum.Female;
            if (Config.AUT == AUT.Za) //TODO implement nationalNumber generators for other regions
                _nationalNumber = Get.GetNationalNumber(_dateOfBirth.DateTime, _gender == GenderEnum.Female);
            _surname = Get.GetName();
            _middleName = Get.GetMiddleName();
            _maidenName = Get.GetName();
            _foreName = Get.GetName();
            _province = ProvinceEnum.ON;
            _houseNumber = Get.RandomInt(1, 100).ToString(CultureInfo.InvariantCulture);
            _houseName = Get.RandomString(8);
            _homePhoneNumber = Get.GetPhone();
            _street = Get.RandomString(15);
            _flat = Get.RandomString(4);
            _district = Get.RandomString(15);
            _town = Get.RandomString(15);
            _county = Get.RandomString(15);
            _nextPayDate = Get.GetNextPayDate();
            _email = Get.RandomEmail();
            _bankAccountId = Get.GetId();

            _province = ProvinceEnum.ON;
            _paymentCardNumber = 4444333322221111;
            _paymentCardSecurityCode = "777";
            _paymentCardType = "Visa";
            _mobileNumber = Get.GetMobilePhone();
            _institutionNumber = "002";
            _branchNumber = "00018";
            _bankAccountNumber = Get.GetBankAccountNumber();

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
        }

        public static CustomerBuilder New()
        {
            return new CustomerBuilder();
        }

        public static CustomerBuilder New(Guid id)
        {
            return new CustomerBuilder { _id = id };
        }

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

        public CustomerBuilder WithNumberOfDependants(int numberOfDependants)
        {
            _numberOfDependants = numberOfDependants;
            return this;
        }

        public CustomerBuilder WithPhoneNumber(String phoneNumber)
        {
            _homePhoneNumber = phoneNumber;
            return this;
        }

        public CustomerBuilder WithProvinceInAddress(ProvinceEnum province)
        {
            _province = province;
            return this;
        }

        public CustomerBuilder WithBankAccountNumber(String bankAccountNumber,String sortCode=null)
        {
            _bankAccountNumber = bankAccountNumber;
            _bankCode = sortCode;
            return this;
        }
       
        public CustomerBuilder WithPaymentCardNumber(Int64 cardNumber)
        {
            _paymentCardNumber = cardNumber;
            return this;
        }

        public CustomerBuilder WithPaymentCardSecurityCode(string securityCode)
        {
            _paymentCardSecurityCode = securityCode;
            return this;
        }

        public CustomerBuilder WithPaymentCardType(string cardType)
        {
            _paymentCardType = cardType;
            return this;
        }

        public CustomerBuilder WithMobileNumber(String mobileNumber)
        {
            _mobileNumber = mobileNumber;
            return this;
        }

        public CustomerBuilder WithSpecificAge(int age)
        {
            var rightNow = DateTime.Today;
            var then = rightNow.AddYears(-age);
            _dateOfBirth = new Date(then);

            return this;
        }

        public CustomerBuilder WithInstitutionNumber(String institutionNumber)
        {
            _institutionNumber = institutionNumber;
            return this;
        }

        public CustomerBuilder WithBranchNumber(String branchNumber)
        {
            _branchNumber = branchNumber;
            return this;
        }

        public CustomerBuilder WithIncomeFrequency(IncomeFrequencyEnum incomeFrequency)
        {
            _incomeFrequency = incomeFrequency;
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
                SaveContactPreferencesCommand.New(r => r.AccountId = _id),
            };

            switch (Config.AUT)
            {

                case AUT.Za:
                    requests.AddRange(new ApiRequest[]
                    {
						SaveSocialDetailsZaCommand.New(r =>
                                                 {
                                                     r.AccountId = _id;
                                                     r.Dependants = _numberOfDependants;
                                                 }),
                        SavePasswordRecoveryDetailsZaCommand.New(r => r.AccountId = _id),
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
						RiskSaveCustomerDetailsZaCommand.New(r =>
						{
                            r.AccountId = _id;
                        	r.Forename = _foreName;
                            r.MiddleName = _middleName;
                        	r.Surname = _surname;
                            r.Email = _email;
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
						RiskSaveCustomerAddressZaCommand.New(r =>
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
                        AddBankAccountZaCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (!string.IsNullOrEmpty(_bankAccountNumber))
                                                            r.AccountNumber = _bankAccountNumber;
                                                    	}),
						RiskAddBankAccountZaCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (!string.IsNullOrEmpty(_bankAccountNumber))
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
						SaveSocialDetailsCaCommand.New(r =>
                                                 {
                                                     r.AccountId = _id;
                                                     r.Dependants = _numberOfDependants;
                                                 }),
                        SavePasswordRecoveryDetailsCaCommand.New(r => r.AccountId = _id),
                        SaveCustomerDetailsCaCommand.New(r => 
                        { 
                            r.AccountId = _id;
                            r.Forename = _foreName;
                            r.MiddleName = _middleName;
                            r.Surname = _surname;
                            r.Email = _email;
                            r.DateOfBirth = _dateOfBirth;
                            r.NationalNumber = _nationalNumber;
                            r.HomePhone = _homePhoneNumber;
                            r.Gender = _gender;
                        }),              
         				RiskSaveCustomerDetailsCaCommand.New(r =>
						{
							r.AccountId = _id;
							r.Forename = _foreName;
                            r.MiddleName = _middleName;
                            r.Surname = _surname;
                            r.Email = _email;
                            r.DateOfBirth = _dateOfBirth;
                            r.HomePhone = _homePhoneNumber;
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
                        } ),
						RiskSaveCustomerAddressCaCommand.New(r =>
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
																 r.SubRegion = _province;
						}),
                        AddBankAccountCaCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                            if (!string.IsNullOrEmpty(_bankAccountNumber))
                                                            {
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}

                                                    	    r.BranchNumber = _branchNumber;
                                                    	    r.InstitutionNumber = _institutionNumber;

                                                    	}),
						RiskAddBankAccountCaCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (!string.IsNullOrEmpty(_bankAccountNumber))
                                                    		{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}
                                                    	}),
                        SaveEmploymentDetailsCaCommand.New(r =>
                        {
                            r.AccountId = _id;
                            r.EmployerName = _employerName;
                        	r.NetMonthlyIncome = _netMonthlyIncome;
                        	r.Status = _employerStatus;
                            if (!string.IsNullOrEmpty(_nextPayDate.ToString()))
                            r.NextPayDate = _nextPayDate;
                            if (!string.IsNullOrEmpty(_incomeFrequency.ToString()))
                                r.IncomeFrequency = _incomeFrequency;
                        })
					});
					requests.AddRange(GetPhoneCommandsPreAccountCreation());
                    break;

                case AUT.Wb:
                    requests.AddRange(new ApiRequest[]
                    {
						SaveSocialDetailsUkCommand.New(r =>
                                                 {
                                                     r.AccountId = _id;
                                                     r.Dependants = _numberOfDependants;
                                                 }),
                        SaveCustomerDetailsUkCommand.New(r=>
                                                             {
                                                                 r.AccountId = _id; 
                                                                 r.MiddleName = _middleName;
                                                                 r.Surname = _surname;
                                                                 r.Forename = _foreName;
                                                                 r.DateOfBirth = _dateOfBirth;
                                                                 r.Email = _email;
                                                                 r.HomePhone = _homePhoneNumber;
                                                                 r.Gender = _gender;
                                                             }),
						RiskSaveCustomerDetailsUkCommand.New(r =>
						{
																r.AccountId = _id; 
                                                                 r.MiddleName = _middleName;
                                                                 r.Surname = _surname;
                                                                 r.Forename = _foreName;
                                                                 r.DateOfBirth = _dateOfBirth;
                                                                 r.Email = _email;
                                                                 r.HomePhone = _homePhoneNumber;
                                                                 r.Gender = _gender;
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
						RiskSaveCustomerAddressUkCommand.New(r =>
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
                                                    	    r.BankAccountId = _bankAccountId;
                                                            if (!string.IsNullOrEmpty(_bankAccountNumber))
                     										{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}

                                                            if (!string.IsNullOrEmpty(_bankCode))
                                                                r.BankCode = _bankCode;
                                                    	}),
						RiskAddBankAccountUkCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (!string.IsNullOrEmpty(_bankAccountNumber))
                                                    		{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}
                                                    	}),

                        AddPaymentCardCommand.New(r =>
						                              {
						                                  r.AccountId = _id;
						                                  r.Number = _paymentCardNumber;
						                              }),
						RiskAddPaymentCardCommand.New(r =>
						                              {
						                                  r.AccountId = _id;
						                                  r.Number = _paymentCardNumber;
						                              })
                    });
					requests.AddRange(GetPhoneCommandsPreAccountCreation());
                    break;

                case AUT.Uk:
                    requests.AddRange(new ApiRequest[]
					{
						SaveSocialDetailsUkCommand.New(r =>
                                                 {
                                                     r.AccountId = _id;
                                                     r.Dependants = _numberOfDependants;
                                                 }),
                        
                        SavePasswordRecoveryDetailsUkCommand.New(r => r.AccountId = _id),
						SaveCustomerDetailsUkCommand.New(r=>
						                                     {
						                                         r.AccountId = _id;
						                                         r.Forename = _foreName;
						                                         r.MiddleName = _middleName;
						                                         r.Surname = _surname;
						                                         r.Email = _email;
						                                         r.DateOfBirth = _dateOfBirth;
						                                     }),
						RiskSaveCustomerDetailsUkCommand.New(r =>
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
						RiskSaveCustomerAddressUkCommand.New(r =>
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
                                                            if (!string.IsNullOrEmpty(_bankAccountNumber))
						                            		{
						                            			r.AccountNumber = _bankAccountNumber;
						                            		}

						                            	}),
						RiskAddBankAccountUkCommand.New(r =>
                                                    	{
                                                    		r.AccountId = _id;
                                                    		r.BankAccountId = _bankAccountId;
                                                    		if (!string.IsNullOrEmpty(_bankAccountNumber))
                                                    		{
                                                    			r.AccountNumber = _bankAccountNumber;
                                                    		}
                                                    	}),

						AddPaymentCardCommand.New(r =>
						                              {
						                                  r.AccountId = _id;
						                                  r.Number = _paymentCardNumber;
						                                  r.HolderName = String.Format("{0} {1}", _foreName, _surname);
                                                          r.IsPrimary = true;
						                                  r.ExpiryDate = DateTime.Today.AddYears(2).ToPaymentCardDate();
						                                  r.SecurityCode = _paymentCardSecurityCode;
						                                  r.CardType = _paymentCardType;
						                              }),
						RiskAddPaymentCardCommand.New(r =>
						                              {
						                                  r.AccountId = _id;
						                                  r.Number = _paymentCardNumber;
						                                  r.HolderName = String.Format("{0} {1}", _foreName, _surname);
														  r.ExpiryDate = DateTime.Today.AddYears(2).ToPaymentCardDate();
						                                  r.SecurityCode = _paymentCardSecurityCode;
						                                  r.CardType = _paymentCardType;
						                              }),
						SaveEmploymentDetailsUkCommand.New(r =>
						{
							r.AccountId = _id;
							r.EmployerName = _employerName;
						    r.Status = _employerStatus;
						    r.NetMonthlyIncome = _netMonthlyIncome;
						})
					});
					requests.AddRange(GetPhoneCommandsPreAccountCreation());
                    break;

                default:
                    throw new NotImplementedException();
            }

            Drive.Api.Commands.Post(requests);

            Do.With.Timeout(2).Until(() => Drive.Data.Ops.Db.Accounts.FindAllByExternalId(_id).Single());
            Do.With.Timeout(2).Until(() => Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_id).Single());
            Do.With.Timeout(2).Until(() => Drive.Data.Risk.Db.RiskAccounts.FindAllByAccountId(_id).Single());

            switch (Config.AUT)
            {
                case AUT.Wb:
                    Do.Until(
                        () =>
                        Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(_id).Single().PaymentCardsBase);
                    break;

                case AUT.Ca:
                    Do.Until(
                        () =>
                        Drive.Db.Payments.BankAccountsBases.Single(bab => bab.ExternalId == _bankAccountId));
                    break;

                case AUT.Za:
                    {
                        var mobilePhoneVerification = Do.Until(() => Drive.Db.Comms.MobilePhoneVerifications.Single(a => a.AccountId == _id));
						Drive.Api.Commands.Post(GetPhoneCommandsPostAccountCreation(mobilePhoneVerification.Pin));
						Do.With.Timeout(2).Until(() => Drive.Db.Comms.CustomerDetails.Single(a => a.AccountId == _id).MobilePhone);
                    }
                    break;
            }

            return new Customer(_id, _email, _bankAccountId, _bankAccountNumber){Province = _province};
        }

        public void ScrubForename(String forename)
        {
            var db = new DbDriver();
            var customerDetailEntities = db.Comms.CustomerDetails.Where(cd => cd.Forename == forename).ToList();
            foreach (CustomerDetailEntity customerDetailEntity in customerDetailEntities)
            {
                customerDetailEntity.Forename = Get.GetName();
            }
            db.Comms.SubmitChanges();
        }

        public void ScrubSurname(String surname)
        {
            var db = new DbDriver();
            var customerDetailEntities = db.Comms.CustomerDetails.Where(cd => cd.Surname == surname).ToList();
            foreach (CustomerDetailEntity customerDetailEntity in customerDetailEntities)
            {
                customerDetailEntity.Surname = Get.GetName();
            }
            db.Comms.SubmitChanges();
        }


		private IEnumerable<ApiRequest> GetPhoneCommandsPostAccountCreation(string pin)
		{
			switch (Config.AUT)
			{

				case AUT.Ca:
				case AUT.Wb:
				case AUT.Uk:
					throw new NotSupportedException(string.Format("{0} does not verify phone after account creation", Config.AUT));

				case AUT.Za:

					return GetZaPhoneCommandsPostAccountCreation(pin);

				default:

					throw new NotImplementedException(string.Format("Not Implemented for {0}", Config.AUT));
			}
		}

		private IEnumerable<ApiRequest> GetPhoneCommandsPreAccountCreation()
		{
			switch (Config.AUT)
			{

				case AUT.Ca:
					return _mobileNumber != null ? GetCaMobilePhoneCommandsPreAccountCreation() : GetCaHomePhoneCommandsPreAccountCreation();

				case AUT.Wb:
					return GetWbMobilePhoneCommandsPreAccountCreation();

				case AUT.Uk:
					return GetUkMobilePhoneCommandsPreAccountCreation();

				case AUT.Za:

					throw new NotSupportedException("ZA does mobile phone verification at a later stage");

				default:

					throw new NotImplementedException(string.Format("Not Implemented for {0}", Config.AUT));
			}
		}

		private IEnumerable<ApiRequest> GetUkMobilePhoneCommandsPreAccountCreation()
		{
			yield return VerifyMobilePhoneUkCommand.New(r =>
			{
				r.AccountId = _id;
				r.VerificationId = _verification;
				r.MobilePhone = _mobileNumber;
			});
			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = _verification);

			yield return RiskAddMobilePhoneUkCommand.New(r =>
			{
				r.AccountId = _id;
				r.MobilePhone = _mobileNumber;
			});
		}

		private IEnumerable<ApiRequest> GetWbMobilePhoneCommandsPreAccountCreation()
		{
			yield return VerifyMobilePhoneUkCommand.New(r =>
			{
				r.AccountId = _id;
				r.Forename = _foreName;
				r.VerificationId = _verification;
				r.MobilePhone = _mobileNumber;
			});

			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = _verification);

			yield return RiskAddMobilePhoneUkCommand.New(r =>
			{
				r.AccountId = _id;
				r.MobilePhone = _mobileNumber;
			});
		}

		private IEnumerable<ApiRequest> GetCaMobilePhoneCommandsPreAccountCreation()
		{
			yield return VerifyMobilePhoneCaCommand.New(r =>
			{
				r.AccountId = _id;
				r.VerificationId = _verification;
				r.MobilePhone = _mobileNumber;
			});

			yield return CompleteMobilePhoneVerificationCommand.New(r => r.VerificationId = _verification);

			yield return RiskAddMobilePhoneCaCommand.New(r =>
			{
				r.AccountId = _id;
				r.MobilePhone = _mobileNumber;
			});
		}

		private IEnumerable<ApiRequest> GetCaHomePhoneCommandsPreAccountCreation()
		{
			yield return VerifyHomePhoneCaCommand.New(r =>
			{
				r.AccountId = _id;
				r.VerificationId = _verification;
				r.HomePhone = _homePhoneNumber;
			});

			yield return CompleteHomePhoneVerificationCaCommand.New(r => r.VerificationId = _verification);

			yield return RiskAddHomePhoneCaCommand.New(r =>
			{
				r.AccountId = _id;
				r.HomePhone = _homePhoneNumber;
			});
		}
		
		private IEnumerable<ApiRequest> GetZaPhoneCommandsPostAccountCreation(string pin)
		{
			yield return new CompleteMobilePhoneVerificationCommand
			             	{
			             		Pin = pin,
			             		VerificationId = _verification
			             	};

			yield return new RiskAddMobilePhoneZaCommand
			             	{
			             		AccountId = _id,
			             		MobilePhone = _mobileNumber
			             	};
		}
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Payments.Commands;
using Wonga.QA.Framework.Api.Requests.Payments.Queries;
using Wonga.QA.Framework.Api.Requests.Payments.Queries.Za;
using Wonga.QA.Framework.Api.Requests.Risk.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Framework.Db.Payments;
using Wonga.QA.Framework.Helpers;

namespace Wonga.QA.Framework
{
    public class Customer
    {
        public Guid Id { get; set; }
        public String Email { get; set; }
        public Guid BankAccountId { get; set; }
        public String MiddleName { get; set; }
        public Int64 CardNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public ProvinceEnum Province { get; set; }

        public Customer(Guid id)
        {
            Id = id;
        }

        public String GetEmail()
        {
            return Drive.Db.Ops.Accounts.Single(c => c.ExternalId == Id).Login;
        }

        public Customer(Guid id, string email, Guid bankAccountId)
        {
            Id = id;
            Email = email;
            BankAccountId = bankAccountId;
        }

        public Customer(Guid id, string email, Guid bankAccountId, string bankAccountNumber)
        {
            Id = id;
            Email = email;
            BankAccountId = bankAccountId;
            BankAccountNumber = bankAccountNumber;
        }

        #region Public Methods

        public Application GetApplication()
        {
            return new Application(Guid.Parse(Drive.Api.Queries.Post(Config.AUT == AUT.Za ? (ApiRequest)new GetAccountSummaryZaQuery { AccountId = Id } : new GetAccountSummaryQuery { AccountId = Id }).Values["ApplicationId"].Single()));
        }

        public Application[] GetApplications()
        {
            return Drive.Db.Payments.Applications.Where(a => a.AccountId == Id).Select(a => new Application(a.ExternalId)).ToArray();
        }

        public Guid GetBankAccount()
        {
            return BankAccountId;
        }

        public PersonalPaymentCardEntity[] GetPersonalPaymentCards()
        {
            return Drive.Db.Payments.PersonalPaymentCards.Where(c => c.AccountId == this.Id).ToArray();
        }

        public void AddPaymentCard(string cardType, string cardNumber, string securityCode, DateTime expiryDate, bool isPrimary)
        {
            AddPaymentCardCommand cmd = AddPaymentCardCommand.New(r =>
            {
                r.AccountId = Id;
                r.Number = cardNumber;
                r.IsPrimary = isPrimary;
                r.SecurityCode = securityCode;
				r.ExpiryDate = expiryDate.ToPaymentCardDate();
                r.CardType = cardType;
            });
            Drive.Api.Commands.Post(cmd);

        	RiskAddPaymentCardCommand riskCommand = RiskAddPaymentCardCommand.New(r =>
        	                                                                      	{
        	                                                                      		r.AccountId = Id;
        	                                                                      		r.Number = cardNumber;
        	                                                                      		r.SecurityCode = securityCode;
																						r.ExpiryDate = expiryDate.ToPaymentCardDate();
        	                                                                      		r.CardType = cardType;
        	                                                                      	});
			Drive.Api.Commands.Post(riskCommand);

            Do.Until(() => Drive.Db.Payments.PersonalPaymentCards
                         .Single(c => c.AccountId == Id
                                      && c.PaymentCardsBaseEntity.Type == cardType
                                      && c.PaymentCardsBaseEntity.MaskedNumber == cardNumber.MaskedCardNumber()));
        }

        public Guid GetPaymentCard()
        {
            //1 - Wait for the AccountPreferences
            //2 - Wait for the PrimaryPaymentCardId to be inserted
            //3 - Return the corresponding PaymentCardsBase.ExternalId
            //4 - If anyone finds a better way to do this tell me ( Alex P )

            Int32? primaryPaymentCardId = null;
            Do.With.Timeout(2).Message("The AccountPreferences has not been created yet").Until(() => ((Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(Id).SingleOrDefault()) != null));
            Do.With.Timeout(2).Message("The PrimaryPaymentCardId has not been updated yet").Until(() => ((primaryPaymentCardId = Drive.Data.Payments.Db.AccountPreferences.FindAllByAccountId(Id).SingleOrDefault().PrimaryPaymentCardId) != null));
            return Drive.Data.Payments.Db.PaymentCardsBase.FindAllByPaymentCardId(primaryPaymentCardId).Single().ExternalId;
        }

        public void UpdateForename(String forename)
        {
            var db = new DbDriver();
            var customerDetailsRow = db.Comms.CustomerDetails.Single(cd => cd.AccountId == Id);
            customerDetailsRow.Forename = forename;
            db.Comms.SubmitChanges();
        }

        public void UpdateSurname(String surname)
        {
            var db = new DbDriver();
            var customerDetailsRow = db.Comms.CustomerDetails.Single(cd => cd.AccountId == Id);
            customerDetailsRow.Surname = surname;
            db.Comms.SubmitChanges();
        }

        public void UpdateEmployer(String employer)
        {
            var row = Drive.Db.Risk.EmploymentDetails.Single(cd => cd.AccountId == Id);
            row.EmployerName = employer;
            row.Submit();
        	Do.Until(row.Refresh);
        }

        public string GetCcin()
        {
            return Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id)).Ccin;
        }

        public string GetNextPayDate()
        {
            return Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id)).NextPayDate.ToString();
        }

        public string GetIncomeFrequency()
        {
            return Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id)).IncomeFrequency.ToString();
        }

        public string GetCustomerFullName()
        {
            var customerDetailsRow = Drive.Db.Comms.CustomerDetails.Single(cd => cd.AccountId == Id);
            return customerDetailsRow.Forename + " " + customerDetailsRow.Surname;
        }

        public string GetCustomerForename()
        {
            var customerDetailsRow = Drive.Db.Comms.CustomerDetails.Single(cd => cd.AccountId == Id);
        	return customerDetailsRow.Forename;
        }

        public void ScrubCcin()
        {
            var db = new DbDriver();
            AccountPreferenceEntity accountPreferenceEntity = db.Payments.AccountPreferences.Single(ap => ap.AccountId == Id);
            accountPreferenceEntity.Ccin = "ScrubbedCcin_" + Get.RandomInt(1000000, 9999999);
            db.Payments.SubmitChanges();
        }

        public string GetCustomerMobileNumber()
        {
            var customerDetailsRow = Drive.Db.Comms.CustomerDetails.Single(cd => cd.AccountId == Id);
            return customerDetailsRow.MobilePhone;
        }

        #endregion
        
    }
}
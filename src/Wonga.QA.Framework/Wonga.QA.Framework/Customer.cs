using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Api;
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
        public String Forename { get; private set; }
        public String Surname { get; private set; }
        public String MiddleName { get; set; }
        public Date DateOfBirth { get; private set; }
        public String MobilePhoneNumber { get; private set; }
        public Int64 CardNumber { get; set; }

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

        public Customer(Guid id,String email,String forename, String surname, Date dateOfBirth, String mobilePhoneNumber)
        {
            Id = id;
            Email = email;
            Forename = forename;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            MobilePhoneNumber = mobilePhoneNumber;
        }

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
            return Drive.Db.Payments.PersonalPaymentCards.Where(c=>c.AccountId == this.Id).ToArray();
        }

        public void AddPaymentCard(string cardType, string cardNumber, string securityCode, DateTime expiryDate, bool isPrimary)
        {
            AddPaymentCardCommand cmd = AddPaymentCardCommand.New(r =>
            {
                r.AccountId = this.Id;
                r.Number = cardNumber;
                r.IsPrimary = isPrimary;
                r.SecurityCode = securityCode;
                r.ExpiryDate = expiryDate.ToString("yyyy-MM");
                r.CardType = cardType;
            });
            Drive.Api.Commands.Post(cmd);
            Do.Until(() => Drive.Db.Payments.PersonalPaymentCards
                         .Single(c => c.AccountId == this.Id
                                      && c.PaymentCardsBaseEntity.Type == cardType
                                      && c.PaymentCardsBaseEntity.MaskedNumber == cardNumber.MaskedCardNumber()));
        }

        public Guid GetPaymentCard()
        {
            return Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id).PaymentCardsBaseEntity.ExternalId;
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
        }

        public string GetCcin()
        {
            return Do.Until(() => Drive.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id)).Ccin;
        }

        public string GetCustomerFullName()
        {
            var customerDetailsRow = Drive.Db.Comms.CustomerDetails.Single(cd => cd.AccountId == Id);
            return customerDetailsRow.Forename +" "+ customerDetailsRow.Surname;
        }

        public void ScrubCcin()
        {
            var db = new DbDriver();
            AccountPreferenceEntity accountPreferenceEntity = db.Payments.AccountPreferences.Single(ap => ap.AccountId == Id);
            accountPreferenceEntity.Ccin = "ScrubbedCcin_"+Get.RandomInt(10000, 99999);
            db.Payments.SubmitChanges();
        }
    }
}
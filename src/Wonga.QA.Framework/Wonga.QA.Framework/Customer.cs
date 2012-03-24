using System;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Framework.Db.Payments;

namespace Wonga.QA.Framework
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid BankAccountId { get; set; }
        public string Forename { get; private set; }
        public string Surname { get; private set; }
        public Date DateOfBirth { get; private set; }

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

        public Customer(Guid id,String email,String forename, String surname, Date dateOfBirth)
        {
            Id = id;
            Email = email;
            Forename = forename;
            Surname = surname;
            DateOfBirth = dateOfBirth;
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
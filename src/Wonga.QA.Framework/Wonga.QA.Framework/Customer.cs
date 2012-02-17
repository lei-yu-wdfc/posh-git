using System;
using System.Linq;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid BankAccountId { get; set; }

        public Customer(Guid id)
        {
            Id = id;
        }

        public Customer(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Customer(Guid id, string email, Guid bankAccountId)
        {
            Id = id;
            Email = email;
            BankAccountId = bankAccountId;
        }

        public Application GetApplication()
        {
            return new Application(Guid.Parse(Driver.Api.Queries.Post(Config.AUT == AUT.Za ? (ApiRequest)new GetAccountSummaryZaQuery { AccountId = Id } : new GetAccountSummaryQuery { AccountId = Id }).Values["ApplicationId"].Single()));
        }

        public Application[] GetApplications()
        {
            return Driver.Db.Payments.Applications.Where(a => a.AccountId == Id).Select(a => new Application(a.ExternalId)).ToArray();
        }

        public Guid GetBankAccount()
        {
			return Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id).BankAccountsBaseEntity.ExternalId;
        }

        public Guid GetPaymentCard()
        {
            return Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id).PaymentCardsBaseEntity.ExternalId;
        }
    }
}
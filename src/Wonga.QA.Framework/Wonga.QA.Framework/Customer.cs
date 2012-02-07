using System;
using System.Linq;

namespace Wonga.QA.Framework
{
    public class Customer
    {
        public Guid Id { get; set; }

        public Customer(Guid id)
        {
            Id = id;
        }

        public Application[] GetApplications()
        {
            return Driver.Db.Payments.Applications.Where(a => a.AccountId == Id).Select(a => new Application(a.ExternalId)).ToArray();
        }

        public Guid GetBankAccount()
        {
            return Driver.Db.Payments.AccountPreferences.Single(a => a.AccountId == Id).BankAccountsBaseEntity.ExternalId;
        }
    }
}
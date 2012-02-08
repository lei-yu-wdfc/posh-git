using System;
using System.Linq;

namespace Wonga.QA.Framework
{
    public class Company
    {
        public Guid Id { get; set; }

        public Company(Guid id)
        {
            Id = id;
        }

        public int GetPaymentCard()
        {
            return Driver.Db.Payments.BusinessPaymentCards.Single(a => a.OrganisationId == Id).PaymentCardId;
        }

        public int GetBankAccount()
        {
            return Driver.Db.Payments.BusinessBankAccounts.Single(a => a.OrganisationId == Id).BankAccountId;
        }
    }
}

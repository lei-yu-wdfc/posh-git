using System;
using System.Collections.Generic;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.ContactManagement;

namespace Wonga.QA.Framework
{
    public class Organisation
    {
        public Guid Id { get; set; }

        public Organisation(Guid id)
        {
            Id = id;
        }

        public Guid GetPaymentCard()
        {
            var paymentCardId = Do.Until(()=>Drive.Db.Payments.BusinessPaymentCards.Single(b => b.OrganisationId == Id).PaymentCardId);
            return Drive.Db.Payments.PaymentCardsBases.Single(a=>a.PaymentCardId == paymentCardId).ExternalId;
        }

        public Guid GetBankAccount()
        {
            var bankAccountId =Do.Until(()=>Drive.Db.Payments.BusinessBankAccounts.Single(b => b.OrganisationId == Id).BankAccountId);
            return Drive.Db.Payments.BankAccountsBases.Single(a=>a.BankAccountId == bankAccountId).ExternalId;
        }

        public IEnumerable<DirectorOrganisationMappingEntity> GetSecondaryDirectors()
        {
            return Drive.Db.ContactManagement.DirectorOrganisationMappings.Where(r => r.OrganisationId == Id && r.DirectorLevel == 1);
        }
    }
}

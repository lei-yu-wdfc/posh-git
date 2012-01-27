using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("BusinessFixedInstallmentApplicationAdded", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "Wonga.Payments.PublicMessages.IBusinessApplicationAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public class BusinessFixedInstallmentApplicationAddedCommand : MsmqMessage<BusinessFixedInstallmentApplicationAddedCommand>
    {
        public DateTime CreatedOn { get; set; }
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Int32 NumberOfWeeks { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ProductEnum ProductId { get; set; }
        public Decimal MonthlyInterestRate { get; set; }
        public Decimal ApplicationFee { get; set; }
    }
}

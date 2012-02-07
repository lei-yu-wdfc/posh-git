using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ManualPaymentTakenMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public partial class ManualPaymentTakenCommand : MsmqMessage<ManualPaymentTakenCommand>
    {
        public DateTime CreatedOn { get; set; }
        public Int64 PaymentReference { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Int32 BatchNumber { get; set; }
        public String BankAccountNumber { get; set; }
        public String BankCode { get; set; }
        public DateTime BatchSendTime { get; set; }
        public Guid ApplicationExternalId { get; set; }
        public Guid SenderReference { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.PublicMessages.BankGateway.Za.IEasypayPaymentTaken </summary>
    [XmlRoot("IEasypayPaymentTaken", Namespace = "Wonga.PublicMessages.BankGateway.Za", DataType = "")]
    public partial class IEasypayPaymentTakenZaEvent : MsmqMessage<IEasypayPaymentTakenZaEvent>
    {
        public DateTime CreatedOn { get; set; }
        public String RepaymentNumber { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
        public Int32 AcknowledgeId { get; set; }
    }
}

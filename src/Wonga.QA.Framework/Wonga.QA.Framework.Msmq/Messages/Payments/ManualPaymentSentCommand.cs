using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("ManualPaymentSentMessage", Namespace = "Wonga.Payments.InternalMessages.SagaMessages", DataType = "")]
    public class ManualPaymentSentCommand : MsmqMessage<ManualPaymentSentCommand>
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ValueDate { get; set; }
        public Decimal TransactionAmount { get; set; }
        public Guid ApplicationExternalId { get; set; }
        public Guid SenderReference { get; set; }
    }
}

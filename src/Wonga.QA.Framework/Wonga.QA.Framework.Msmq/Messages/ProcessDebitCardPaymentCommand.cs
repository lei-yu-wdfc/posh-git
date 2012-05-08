using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ProcessDebitCardPaymentMessage </summary>
    [XmlRoot("ProcessDebitCardPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ProcessDebitCardPaymentCommand : MsmqMessage<ProcessDebitCardPaymentCommand>
    {
        public Guid ApplicationId { get; set; }
        public Decimal CollectAmount { get; set; }
        public Guid SenderReference { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid PaymentExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public RepaymentRequestEnum RepaymentRequestType { get; set; }
    }
}

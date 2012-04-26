using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ResendPaymentMessage </summary>
    [XmlRoot("ResendPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ResendPaymentCommand : MsmqMessage<ResendPaymentCommand>
    {
        public Guid TransactionId { get; set; }
    }
}

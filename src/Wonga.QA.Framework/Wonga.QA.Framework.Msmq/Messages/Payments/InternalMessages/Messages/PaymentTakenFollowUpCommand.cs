using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PaymentTakenFollowUpMessage </summary>
    [XmlRoot("PaymentTakenFollowUpMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PaymentTakenFollowUpCommand : MsmqMessage<PaymentTakenFollowUpCommand>
    {
        public Guid ApplicationId { get; set; }
        public Decimal TransactionAmount { get; set; }
    }
}

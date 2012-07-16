using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Ping
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StartDCAPaymentSuppression </summary>
    [XmlRoot("StartDCAPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StartDCAPaymentSuppression : MsmqMessage<StartDCAPaymentSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

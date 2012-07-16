using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Ping
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StartFraudPaymentSuppression </summary>
    [XmlRoot("StartFraudPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StartFraudPaymentSuppression : MsmqMessage<StartFraudPaymentSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

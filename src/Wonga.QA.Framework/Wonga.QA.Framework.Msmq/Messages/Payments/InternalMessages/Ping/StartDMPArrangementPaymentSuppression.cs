using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Ping
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StartDMPArrangementPaymentSuppression </summary>
    [XmlRoot("StartDMPArrangementPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StartDMPArrangementPaymentSuppression : MsmqMessage<StartDMPArrangementPaymentSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StopHardshipPaymentSuppression </summary>
    [XmlRoot("StopHardshipPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StopHardshipPaymentSuppressionCommand : MsmqMessage<StopHardshipPaymentSuppressionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

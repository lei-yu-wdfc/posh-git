using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StopRepaymentArrangementPaymentSuppression </summary>
    [XmlRoot("StopRepaymentArrangementPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StopRepaymentArrangementPaymentSuppressionCommand : MsmqMessage<StopRepaymentArrangementPaymentSuppressionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

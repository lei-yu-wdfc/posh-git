using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StopDebtSoldPaymentSuppression </summary>
    [XmlRoot("StopDebtSoldPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StopDebtSoldPaymentSuppressionCommand : MsmqMessage<StopDebtSoldPaymentSuppressionCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Ping
{
    /// <summary> Wonga.Payments.InternalMessages.Ping.StartDebtSoldPaymentSuppression </summary>
    [XmlRoot("StartDebtSoldPaymentSuppression", Namespace = "Wonga.Payments.InternalMessages.Ping", DataType = "")]
    public partial class StartDebtSoldPaymentSuppression : MsmqMessage<StartDebtSoldPaymentSuppression>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

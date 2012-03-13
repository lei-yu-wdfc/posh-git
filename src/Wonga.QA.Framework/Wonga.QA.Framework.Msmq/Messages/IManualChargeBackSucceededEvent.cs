using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IManualChargeBackSucceeded </summary>
    [XmlRoot("IManualChargeBackSucceeded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IManualChargeBackSucceededEvent : MsmqMessage<IManualChargeBackSucceededEvent>
    {
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

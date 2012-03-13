using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IDueAmountExceededDoublePrinciple </summary>
    [XmlRoot("IDueAmountExceededDoublePrinciple", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IDueAmountExceededDoublePrincipleEvent : MsmqMessage<IDueAmountExceededDoublePrincipleEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

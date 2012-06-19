using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IHardshipDeactivated </summary>
    [XmlRoot("IHardshipDeactivated", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IHardshipDeactivatedEvent : MsmqMessage<IHardshipDeactivatedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

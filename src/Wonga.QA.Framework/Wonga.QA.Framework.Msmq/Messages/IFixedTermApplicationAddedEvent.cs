using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IFixedTermApplicationAdded </summary>
    [XmlRoot("IFixedTermApplicationAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IApplicationAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IFixedTermApplicationAddedEvent : MsmqMessage<IFixedTermApplicationAddedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IApplicationGuarantorAdded </summary>
    [XmlRoot("IApplicationGuarantorAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IApplicationGuarantorAddedEvent : MsmqMessage<IApplicationGuarantorAddedEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

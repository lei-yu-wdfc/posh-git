using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.PublicMessages.IApplicationCanceled </summary>
    [XmlRoot("IApplicationCanceled", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IApplicationCanceledEvent : MsmqMessage<IApplicationCanceledEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

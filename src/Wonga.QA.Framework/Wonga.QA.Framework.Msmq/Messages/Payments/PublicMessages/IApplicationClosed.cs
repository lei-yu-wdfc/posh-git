using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IApplicationClosed </summary>
    [XmlRoot("IApplicationClosed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IApplicationClosed : MsmqMessage<IApplicationClosed>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ClosedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

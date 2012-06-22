using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IBusinessApplicationClosed </summary>
    [XmlRoot("IBusinessApplicationClosed", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBusinessApplicationClosedEvent : MsmqMessage<IBusinessApplicationClosedEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime ClosedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages.events
{
    /// <summary> Wonga.Graydon.PublicMessages.events.IFoundNoCompany </summary>
    [XmlRoot("IFoundNoCompany", Namespace = "Wonga.Graydon.PublicMessages.events", DataType = "")]
    public partial class IFoundNoCompanyEvent : MsmqMessage<IFoundNoCompanyEvent>
    {
        public Guid OrganisationId { get; set; }
    }
}

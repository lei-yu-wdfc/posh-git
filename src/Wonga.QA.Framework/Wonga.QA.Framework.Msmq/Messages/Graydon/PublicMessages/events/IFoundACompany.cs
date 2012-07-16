using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages.events
{
    /// <summary> Wonga.Graydon.PublicMessages.events.IFoundACompany </summary>
    [XmlRoot("IFoundACompany", Namespace = "Wonga.Graydon.PublicMessages.events", DataType = "")]
    public partial class IFoundACompany : MsmqMessage<IFoundACompany>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

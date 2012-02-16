using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Graydon
{
    [XmlRoot("IFoundACompany", Namespace = "Wonga.Graydon.PublicMessages.events", DataType = "")]
    public partial class IFoundACompanyEvent : MsmqMessage<IFoundACompanyEvent>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupResponse </summary>
    [XmlRoot("CompanyLookupResponse", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public partial class CompanyLookupResponse : MsmqMessage<CompanyLookupResponse>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
        public Object Company { get; set; }
    }
}
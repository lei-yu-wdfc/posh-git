using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupRequest </summary>
    [XmlRoot("CompanyLookupRequest", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public partial class CompanyLookupRequest : MsmqMessage<CompanyLookupRequest>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}
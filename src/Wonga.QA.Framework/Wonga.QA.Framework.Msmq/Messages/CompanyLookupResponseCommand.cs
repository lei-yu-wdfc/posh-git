using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupResponse </summary>
    [XmlRoot("CompanyLookupResponse", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public partial class CompanyLookupResponseCommand : MsmqMessage<CompanyLookupResponseCommand>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
        public Object Company { get; set; }
    }
}

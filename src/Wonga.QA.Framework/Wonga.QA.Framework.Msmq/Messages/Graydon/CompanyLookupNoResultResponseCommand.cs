using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Graydon
{
    [XmlRoot("CompanyLookupNoResultResponse", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public class CompanyLookupNoResultResponseCommand : MsmqMessage<CompanyLookupNoResultResponseCommand>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Graydon
{
    [XmlRoot("CompanyLookupRequest", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public partial class CompanyLookupRequestCommand : MsmqMessage<CompanyLookupRequestCommand>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

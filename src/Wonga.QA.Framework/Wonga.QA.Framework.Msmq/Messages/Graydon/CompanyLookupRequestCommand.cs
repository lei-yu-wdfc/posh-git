using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Graydon
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupRequest </summary>
    [XmlRoot("CompanyLookupRequest", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public partial class CompanyLookupRequestCommand : MsmqMessage<CompanyLookupRequestCommand>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

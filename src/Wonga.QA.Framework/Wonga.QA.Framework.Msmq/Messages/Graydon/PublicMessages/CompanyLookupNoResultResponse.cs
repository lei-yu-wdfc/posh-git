using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupNoResultResponse </summary>
    [XmlRoot("CompanyLookupNoResultResponse", Namespace = "Wonga.Graydon.PublicMessages", DataType = "")]
    public partial class CompanyLookupNoResultResponse : MsmqMessage<CompanyLookupNoResultResponse>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

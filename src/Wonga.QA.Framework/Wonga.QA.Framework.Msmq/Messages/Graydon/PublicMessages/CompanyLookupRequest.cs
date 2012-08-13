using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupRequest </summary>
    [XmlRoot("CompanyLookupRequest", Namespace = "Wonga.Graydon.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Graydon.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompanyLookupRequest : MsmqMessage<CompanyLookupRequest>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
    }
}

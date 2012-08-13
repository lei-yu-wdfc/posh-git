using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages
{
    /// <summary> Wonga.Graydon.PublicMessages.CompanyLookupResponse </summary>
    [XmlRoot("CompanyLookupResponse", Namespace = "Wonga.Graydon.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Graydon.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CompanyLookupResponse : MsmqMessage<CompanyLookupResponse>
    {
        public Guid OrganisationId { get; set; }
        public String Identifier { get; set; }
        public Object Company { get; set; }
    }
}

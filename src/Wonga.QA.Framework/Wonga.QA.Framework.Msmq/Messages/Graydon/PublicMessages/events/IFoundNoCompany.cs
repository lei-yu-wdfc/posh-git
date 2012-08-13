using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Graydon.PublicMessages.events
{
    /// <summary> Wonga.Graydon.PublicMessages.events.IFoundNoCompany </summary>
    [XmlRoot("IFoundNoCompany", Namespace = "Wonga.Graydon.PublicMessages.events", DataType = "" )
    , SourceAssembly("Wonga.Graydon.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IFoundNoCompany : MsmqMessage<IFoundNoCompany>
    {
        public Guid OrganisationId { get; set; }
    }
}

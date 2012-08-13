using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.OrganisationUpserted </summary>
    [XmlRoot("OrganisationUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Salesforce.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class OrganisationUpserted : MsmqMessage<OrganisationUpserted>
    {
        public Guid OrganisationId { get; set; }
    }
}

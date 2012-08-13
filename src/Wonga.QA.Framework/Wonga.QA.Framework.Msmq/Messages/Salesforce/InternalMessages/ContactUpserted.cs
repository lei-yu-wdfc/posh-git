using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.ContactUpserted </summary>
    [XmlRoot("ContactUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "Wonga.Salesforce.InternalMessages.ISalesforceActivityParentAddedBase" )
    , SourceAssembly("Wonga.Salesforce.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ContactUpserted : MsmqMessage<ContactUpserted>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}

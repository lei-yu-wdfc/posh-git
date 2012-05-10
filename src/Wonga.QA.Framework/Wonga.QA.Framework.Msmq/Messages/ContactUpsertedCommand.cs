using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Salesforce.InternalMessages.ContactUpserted </summary>
    [XmlRoot("ContactUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "Wonga.Salesforce.InternalMessages.ISalesforceActivityParentAddedBase")]
    public partial class ContactUpsertedCommand : MsmqMessage<ContactUpsertedCommand>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}

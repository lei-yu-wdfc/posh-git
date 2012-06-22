using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalEvents
{
    /// <summary> Wonga.Salesforce.InternalEvents.ISalesforceAccountAdded </summary>
    [XmlRoot("ISalesforceAccountAdded", Namespace = "Wonga.Salesforce.InternalEvents", DataType = "")]
    public partial class ISalesforceAccountAddedEvent : MsmqMessage<ISalesforceAccountAddedEvent>
    {
        public Guid AccountId { get; set; }
    }
}

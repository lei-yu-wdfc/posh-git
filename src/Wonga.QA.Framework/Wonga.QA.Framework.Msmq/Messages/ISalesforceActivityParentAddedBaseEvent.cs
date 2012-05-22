using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Salesforce.InternalMessages.ISalesforceActivityParentAddedBase </summary>
    [XmlRoot("ISalesforceActivityParentAddedBase", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class ISalesforceActivityParentAddedBaseEvent : MsmqMessage<ISalesforceActivityParentAddedBaseEvent>
    {
        public Guid AccountId { get; set; }
    }
}

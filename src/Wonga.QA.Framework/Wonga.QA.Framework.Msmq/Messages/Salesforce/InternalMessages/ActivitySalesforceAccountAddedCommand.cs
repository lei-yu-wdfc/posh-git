using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.ActivitySalesforceAccountAddedMessage </summary>
    [XmlRoot("ActivitySalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "Wonga.Salesforce.InternalMessages.ISalesforceActivityParentAddedBase")]
    public partial class ActivitySalesforceAccountAddedCommand : MsmqMessage<ActivitySalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

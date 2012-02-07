using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Salesforce
{
    [XmlRoot("ActivitySalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class ActivitySalesforceAccountAddedCommand : MsmqMessage<ActivitySalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

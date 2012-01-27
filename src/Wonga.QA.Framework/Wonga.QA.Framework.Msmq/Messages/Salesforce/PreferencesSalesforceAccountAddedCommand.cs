using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Salesforce
{
    [XmlRoot("PreferencesSalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public class PreferencesSalesforceAccountAddedCommand : MsmqMessage<PreferencesSalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

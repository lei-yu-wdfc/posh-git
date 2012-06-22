using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.PreferencesSalesforceAccountAddedMessage </summary>
    [XmlRoot("PreferencesSalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class PreferencesSalesforceAccountAddedCommand : MsmqMessage<PreferencesSalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

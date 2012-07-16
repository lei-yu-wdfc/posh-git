using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages
{
    /// <summary> Wonga.Salesforce.InternalMessages.MobilePhoneSalesforceAccountAddedMessage </summary>
    [XmlRoot("MobilePhoneSalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class MobilePhoneSalesforceAccountAddedMessage : MsmqMessage<MobilePhoneSalesforceAccountAddedMessage>
    {
        public Guid AccountId { get; set; }
    }
}

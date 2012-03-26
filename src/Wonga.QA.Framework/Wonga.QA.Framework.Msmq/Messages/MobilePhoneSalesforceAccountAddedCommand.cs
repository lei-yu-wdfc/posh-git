using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Salesforce.InternalMessages.MobilePhoneSalesforceAccountAddedMessage </summary>
    [XmlRoot("MobilePhoneSalesforceAccountAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class MobilePhoneSalesforceAccountAddedCommand : MsmqMessage<MobilePhoneSalesforceAccountAddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

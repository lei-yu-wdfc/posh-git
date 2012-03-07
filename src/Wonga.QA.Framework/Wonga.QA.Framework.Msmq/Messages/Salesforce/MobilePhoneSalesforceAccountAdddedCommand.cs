using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Salesforce
{
    /// <summary> Wonga.Salesforce.InternalMessages.MobilePhoneSalesforceAccountAdddedMessage </summary>
    [XmlRoot("MobilePhoneSalesforceAccountAdddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class MobilePhoneSalesforceAccountAdddedCommand : MsmqMessage<MobilePhoneSalesforceAccountAdddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Salesforce
{
    /// <summary> Wonga.Salesforce.InternalMessages.HomePhoneSalesforceAccountAdddedMessage </summary>
    [XmlRoot("HomePhoneSalesforceAccountAdddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class HomePhoneSalesforceAccountAdddedCommand : MsmqMessage<HomePhoneSalesforceAccountAdddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

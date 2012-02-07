using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Salesforce
{
    [XmlRoot("HomePhoneSalesforceAccountAdddedMessage", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class HomePhoneSalesforceAccountAdddedCommand : MsmqMessage<HomePhoneSalesforceAccountAdddedCommand>
    {
        public Guid AccountId { get; set; }
    }
}

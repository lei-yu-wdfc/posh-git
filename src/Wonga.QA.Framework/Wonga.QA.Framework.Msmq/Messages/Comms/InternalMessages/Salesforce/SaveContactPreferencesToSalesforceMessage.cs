using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.SaveContactPreferencesToSalesforceMessage </summary>
    [XmlRoot("SaveContactPreferencesToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class SaveContactPreferencesToSalesforceMessage : MsmqMessage<SaveContactPreferencesToSalesforceMessage>
    {
        public Guid AccountId { get; set; }
        public Boolean AcceptMarketingContact { get; set; }
    }
}

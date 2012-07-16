using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce.Ca
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Ca.UpdateHomePhoneToSalesforceMessage </summary>
    [XmlRoot("UpdateHomePhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Ca", DataType = "")]
    public partial class UpdateHomePhoneToSalesforceMessage : MsmqMessage<UpdateHomePhoneToSalesforceMessage>
    {
        public String Pin { get; set; }
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
    }
}

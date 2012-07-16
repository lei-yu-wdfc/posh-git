using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.UpdateCustomerWorkPhoneToSalesforceMessage </summary>
    [XmlRoot("UpdateCustomerWorkPhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class UpdateCustomerWorkPhoneToSalesforceMessage : MsmqMessage<UpdateCustomerWorkPhoneToSalesforceMessage>
    {
        public Guid AccountId { get; set; }
        public String WorkPhone { get; set; }
    }
}

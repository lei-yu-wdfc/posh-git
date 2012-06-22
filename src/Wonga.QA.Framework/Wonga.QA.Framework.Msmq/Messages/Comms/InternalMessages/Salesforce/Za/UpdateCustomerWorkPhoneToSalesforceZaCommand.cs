using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Salesforce.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Za.UpdateCustomerWorkPhoneToSalesforceMessage </summary>
    [XmlRoot("UpdateCustomerWorkPhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Za", DataType = "")]
    public partial class UpdateCustomerWorkPhoneToSalesforceZaCommand : MsmqMessage<UpdateCustomerWorkPhoneToSalesforceZaCommand>
    {
        public Guid AccountId { get; set; }
        public String WorkPhone { get; set; }
    }
}

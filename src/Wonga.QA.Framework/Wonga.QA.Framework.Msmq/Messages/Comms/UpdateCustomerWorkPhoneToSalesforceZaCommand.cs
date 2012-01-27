using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("UpdateCustomerWorkPhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Za", DataType = "")]
    public class UpdateCustomerWorkPhoneToSalesforceZaCommand : MsmqMessage<UpdateCustomerWorkPhoneToSalesforceZaCommand>
    {
        public Guid AccountId { get; set; }
        public String WorkPhone { get; set; }
    }
}

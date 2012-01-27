using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("InsertMissingCustomerDetailsToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Za", DataType = "")]
    public class InsertMissingCustomerDetailsToSalesforceZaCommand : MsmqMessage<InsertMissingCustomerDetailsToSalesforceZaCommand>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
    }
}

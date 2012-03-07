using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Za.InsertMissingCustomerDetailsToSalesforceMessage </summary>
    [XmlRoot("InsertMissingCustomerDetailsToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Za", DataType = "")]
    public partial class InsertMissingCustomerDetailsToSalesforceZaCommand : MsmqMessage<InsertMissingCustomerDetailsToSalesforceZaCommand>
    {
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
    }
}

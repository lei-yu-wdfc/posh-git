using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SaveEmailToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public class SaveEmailToSalesforceCommand : MsmqMessage<SaveEmailToSalesforceCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}

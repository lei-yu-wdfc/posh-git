using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("UpdateHomePhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public class UpdateHomePhoneToSalesforceCommand : MsmqMessage<UpdateHomePhoneToSalesforceCommand>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
    }
}

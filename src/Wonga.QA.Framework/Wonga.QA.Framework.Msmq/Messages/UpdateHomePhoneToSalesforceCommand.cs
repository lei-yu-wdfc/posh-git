using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.UpdateHomePhoneToSalesforceMessage </summary>
    [XmlRoot("UpdateHomePhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class UpdateHomePhoneToSalesforceCommand : MsmqMessage<UpdateHomePhoneToSalesforceCommand>
    {
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
    }
}

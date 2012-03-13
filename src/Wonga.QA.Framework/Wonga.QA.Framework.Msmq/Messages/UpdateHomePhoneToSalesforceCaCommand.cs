using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Salesforce.Ca.UpdateHomePhoneToSalesforceMessage </summary>
    [XmlRoot("UpdateHomePhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce.Ca", DataType = "")]
    public partial class UpdateHomePhoneToSalesforceCaCommand : MsmqMessage<UpdateHomePhoneToSalesforceCaCommand>
    {
        public String Pin { get; set; }
        public Guid AccountId { get; set; }
        public String HomePhone { get; set; }
    }
}

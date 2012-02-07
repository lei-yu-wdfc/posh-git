using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SaveContactPreferencesToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class SaveContactPreferencesToSalesforceCommand : MsmqMessage<SaveContactPreferencesToSalesforceCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean AcceptMarketingContact { get; set; }
    }
}

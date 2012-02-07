using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("UpdateMobilePhoneToSalesforceMessage", Namespace = "Wonga.Comms.InternalMessages.Salesforce", DataType = "")]
    public partial class UpdateMobilePhoneToSalesforceCommand : MsmqMessage<UpdateMobilePhoneToSalesforceCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public Boolean MobileVerified { get; set; }
        public String Pin { get; set; }
    }
}

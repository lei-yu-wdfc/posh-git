using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IFraudNotificationReceived", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IFraudNotificationReceivedEvent : MsmqMessage<IFraudNotificationReceivedEvent>
    {
        public Guid AccountId { get; set; }
        public Boolean HasFraud { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

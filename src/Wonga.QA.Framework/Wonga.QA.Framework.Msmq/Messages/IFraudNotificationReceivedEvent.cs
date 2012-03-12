using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.IFraudNotificationReceived </summary>
    [XmlRoot("IFraudNotificationReceived", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IFraudNotificationReceivedEvent : MsmqMessage<IFraudNotificationReceivedEvent>
    {
        public Guid AccountId { get; set; }
        public Boolean HasFraud { get; set; }
        public String CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

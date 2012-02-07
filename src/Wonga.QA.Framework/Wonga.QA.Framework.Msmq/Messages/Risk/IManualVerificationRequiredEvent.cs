using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IManualVerificationRequired", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IManualVerificationRequiredEvent : MsmqMessage<IManualVerificationRequiredEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

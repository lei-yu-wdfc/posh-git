using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IHomePhoneUpdatedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IHomePhoneUpdated")]
    public class IHomePhoneUpdatedInternalEvent : MsmqMessage<IHomePhoneUpdatedInternalEvent>
    {
        public Guid VerificationId { get; set; }
        public String HomePhone { get; set; }
        public Guid AccountId { get; set; }
    }
}

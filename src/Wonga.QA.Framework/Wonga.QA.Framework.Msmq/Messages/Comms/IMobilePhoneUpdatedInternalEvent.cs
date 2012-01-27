using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("IMobilePhoneUpdatedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IMobilePhoneUpdated")]
    public class IMobilePhoneUpdatedInternalEvent : MsmqMessage<IMobilePhoneUpdatedInternalEvent>
    {
        public Guid VerificationId { get; set; }
        public String MobilePhone { get; set; }
        public Guid AccountId { get; set; }
    }
}

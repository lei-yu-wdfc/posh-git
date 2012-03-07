using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.IMobilePhoneUpdatedInternal </summary>
    [XmlRoot("IMobilePhoneUpdatedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IMobilePhoneUpdated")]
    public partial class IMobilePhoneUpdatedInternalEvent : MsmqMessage<IMobilePhoneUpdatedInternalEvent>
    {
        public Guid VerificationId { get; set; }
        public String MobilePhone { get; set; }
        public Guid AccountId { get; set; }
    }
}

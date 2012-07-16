using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.IMobilePhoneUpdatedInternal </summary>
    [XmlRoot("IMobilePhoneUpdatedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.IMobilePhoneUpdated")]
    public partial class IMobilePhoneUpdatedInternal : MsmqMessage<IMobilePhoneUpdatedInternal>
    {
        public Guid VerificationId { get; set; }
        public String MobilePhone { get; set; }
        public Guid AccountId { get; set; }
    }
}

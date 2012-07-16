using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IMobilePhoneUpdated </summary>
    [XmlRoot("IMobilePhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IMobilePhoneUpdated : MsmqMessage<IMobilePhoneUpdated>
    {
        public Guid AccountId { get; set; }
    }
}

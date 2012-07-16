using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IHomePhoneUpdated </summary>
    [XmlRoot("IHomePhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IHomePhoneUpdated : MsmqMessage<IHomePhoneUpdated>
    {
        public Guid AccountId { get; set; }
    }
}

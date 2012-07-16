using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.IWorkPhoneUpdated </summary>
    [XmlRoot("IWorkPhoneUpdated", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class IWorkPhoneUpdated : MsmqMessage<IWorkPhoneUpdated>
    {
        public Guid AccountId { get; set; }
    }
}

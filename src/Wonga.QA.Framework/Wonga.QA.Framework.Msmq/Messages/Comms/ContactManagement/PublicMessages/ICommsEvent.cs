using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.PublicMessages
{
    /// <summary> Wonga.Comms.ContactManagement.PublicMessages.ICommsEvent </summary>
    [XmlRoot("ICommsEvent", Namespace = "Wonga.Comms.ContactManagement.PublicMessages", DataType = "")]
    public partial class ICommsEvent : MsmqMessage<ICommsEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}

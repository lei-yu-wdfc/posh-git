using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages
{
    /// <summary> Wonga.Comms.PublicMessages.ICommsEvent </summary>
    [XmlRoot("ICommsEvent", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ICommsEvent : MsmqMessage<ICommsEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.ICommsEvent </summary>
    [XmlRoot("ICommsEvent", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ICommsCommsEvent : MsmqMessage<ICommsCommsEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    /// <summary> Wonga.Ops.PublicMessages.IPasswordChanged </summary>
    [XmlRoot("IPasswordChanged", Namespace = "Wonga.Ops.PublicMessages", DataType = "")]
    public partial class IPasswordChangedEvent : MsmqMessage<IPasswordChangedEvent>
    {
        public Guid AccountId { get; set; }
    }
}

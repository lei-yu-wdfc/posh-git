using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages
{
    /// <summary> Wonga.Ops.PublicMessages.IPasswordChanged </summary>
    [XmlRoot("IPasswordChanged", Namespace = "Wonga.Ops.PublicMessages", DataType = "")]
    public partial class IPasswordChanged : MsmqMessage<IPasswordChanged>
    {
        public Guid AccountId { get; set; }
    }
}

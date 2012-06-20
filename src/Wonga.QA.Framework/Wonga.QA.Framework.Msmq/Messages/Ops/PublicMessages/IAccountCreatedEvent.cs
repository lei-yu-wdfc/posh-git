using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages
{
    /// <summary> Wonga.Ops.PublicMessages.IAccountCreated </summary>
    [XmlRoot("IAccountCreated", Namespace = "Wonga.Ops.PublicMessages", DataType = "")]
    public partial class IAccountCreatedEvent : MsmqMessage<IAccountCreatedEvent>
    {
        public Guid AccountId { get; set; }
    }
}

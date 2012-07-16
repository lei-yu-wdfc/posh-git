using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelExtension </summary>
    [XmlRoot("CancelExtension", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CancelExtension : MsmqMessage<CancelExtension>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid AccountId { get; set; }
    }
}

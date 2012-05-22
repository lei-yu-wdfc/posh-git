using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CancelExtension </summary>
    [XmlRoot("CancelExtension", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CancelExtensionCommand : MsmqMessage<CancelExtensionCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid AccountId { get; set; }
    }
}

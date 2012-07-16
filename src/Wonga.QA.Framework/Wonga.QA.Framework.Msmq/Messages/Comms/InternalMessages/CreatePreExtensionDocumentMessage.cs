using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.CreatePreExtensionDocumentMessage </summary>
    [XmlRoot("CreatePreExtensionDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "")]
    public partial class CreatePreExtensionDocumentMessage : MsmqMessage<CreatePreExtensionDocumentMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
    }
}

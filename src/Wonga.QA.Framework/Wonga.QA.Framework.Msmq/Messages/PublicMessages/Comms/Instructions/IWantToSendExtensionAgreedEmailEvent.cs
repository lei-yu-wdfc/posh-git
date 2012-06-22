using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Instructions.IWantToSendExtensionAgreedEmail </summary>
    [XmlRoot("IWantToSendExtensionAgreedEmail", Namespace = "Wonga.PublicMessages.Comms.Instructions", DataType = "")]
    public partial class IWantToSendExtensionAgreedEmailEvent : MsmqMessage<IWantToSendExtensionAgreedEmailEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ExtensionId { get; set; }
        public Guid OriginatingSagaId { get; set; }
        public Guid HtmlFileId { get; set; }
    }
}

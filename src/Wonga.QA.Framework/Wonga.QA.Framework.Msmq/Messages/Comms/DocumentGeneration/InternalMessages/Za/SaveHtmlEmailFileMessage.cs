using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.DocumentGeneration.InternalMessages.Za
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Za.SaveHtmlEmailFileMessage </summary>
    [XmlRoot("SaveHtmlEmailFileMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Za", DataType = "")]
    public partial class SaveHtmlEmailFileMessage : MsmqMessage<SaveHtmlEmailFileMessage>
    {
        public Guid OriginatingSagaId { get; set; }
        public Byte[] Content { get; set; }
    }
}

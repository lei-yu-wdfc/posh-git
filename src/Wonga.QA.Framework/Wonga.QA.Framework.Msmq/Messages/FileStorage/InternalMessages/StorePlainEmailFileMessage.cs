using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.StorePlainEmailFileMessage </summary>
    [XmlRoot("StorePlainEmailFileMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class StorePlainEmailFileMessage : MsmqMessage<StorePlainEmailFileMessage>
    {
        public Guid OriginatingSagaId { get; set; }
        public String TextContent { get; set; }
    }
}
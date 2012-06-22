using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.StoreHtmlEmailFileMessage </summary>
    [XmlRoot("StoreHtmlEmailFileMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class StoreHtmlEmailFileCommand : MsmqMessage<StoreHtmlEmailFileCommand>
    {
        public Guid OriginatingSagaId { get; set; }
        public String TextContent { get; set; }
    }
}

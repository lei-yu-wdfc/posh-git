using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.FileStorage.InternalMessages.StorePlainEmailFileMessage </summary>
    [XmlRoot("StorePlainEmailFileMessage", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "")]
    public partial class StorePlainEmailFileCommand : MsmqMessage<StorePlainEmailFileCommand>
    {
        public Guid OriginatingSagaId { get; set; }
        public String TextContent { get; set; }
    }
}

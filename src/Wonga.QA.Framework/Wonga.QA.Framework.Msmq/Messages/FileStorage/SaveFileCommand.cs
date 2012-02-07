using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.FileStorage
{
    [XmlRoot("SaveFileMessage", Namespace = "Wonga.FileStorage.PublicMessages", DataType = "")]
    public partial class SaveFileCommand : MsmqMessage<SaveFileCommand>
    {
        public Guid FileId { get; set; }
        public Byte[] Content { get; set; }
    }
}

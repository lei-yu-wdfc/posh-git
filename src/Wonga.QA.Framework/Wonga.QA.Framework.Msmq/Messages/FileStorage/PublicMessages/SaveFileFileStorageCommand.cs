using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.PublicMessages
{
    /// <summary> Wonga.FileStorage.PublicMessages.SaveFileMessage </summary>
    [XmlRoot("SaveFileMessage", Namespace = "Wonga.FileStorage.PublicMessages", DataType = "")]
    public partial class SaveFileFileStorageCommand : MsmqMessage<SaveFileFileStorageCommand>
    {
        public Guid FileId { get; set; }
        public Byte[] Content { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.PublicMessages
{
    /// <summary> Wonga.FileStorage.PublicMessages.SaveFileMessage </summary>
    [XmlRoot("SaveFileMessage", Namespace = "Wonga.FileStorage.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.FileStorage.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveFileMessage : MsmqMessage<SaveFileMessage>
    {
        public Guid FileId { get; set; }
        public Byte[] Content { get; set; }
    }
}

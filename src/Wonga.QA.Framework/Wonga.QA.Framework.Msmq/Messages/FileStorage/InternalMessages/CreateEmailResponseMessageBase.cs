using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.FileStorage.InternalMessages
{
    /// <summary> Wonga.FileStorage.InternalMessages.CreateEmailResponseMessageBase </summary>
    [XmlRoot("CreateEmailResponseMessageBase", Namespace = "Wonga.FileStorage.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.FileStorage.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateEmailResponseMessageBase : MsmqMessage<CreateEmailResponseMessageBase>
    {
        public String HtmlContent { get; set; }
        public String PlainContent { get; set; }
    }
}

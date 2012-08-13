using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Api
{
    /// <summary> Wonga.Api.IEventMessage </summary>
    [XmlRoot("IEventMessage", Namespace = "Wonga.Api", DataType = "" )
    , SourceAssembly("Wonga.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IEventMessage : MsmqMessage<IEventMessage>
    {
        public DateTime CreatedOn { get; set; }
    }
}

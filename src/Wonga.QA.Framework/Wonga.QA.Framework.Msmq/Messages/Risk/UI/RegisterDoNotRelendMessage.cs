using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.UI
{
    /// <summary> Wonga.Risk.UI.RegisterDoNotRelendMessage </summary>
    [XmlRoot("RegisterDoNotRelendMessage", Namespace = "Wonga.Risk.UI", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RegisterDoNotRelendMessage : MsmqMessage<RegisterDoNotRelendMessage>
    {
        public Guid AccountId { get; set; }
        public Boolean DoNotRelend { get; set; }
    }
}

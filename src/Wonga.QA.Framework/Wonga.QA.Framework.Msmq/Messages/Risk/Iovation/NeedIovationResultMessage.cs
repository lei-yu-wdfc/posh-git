using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Iovation
{
    /// <summary> Wonga.Risk.Iovation.NeedIovationResultMessage </summary>
    [XmlRoot("NeedIovationResultMessage", Namespace = "Wonga.Risk.Iovation", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class NeedIovationResultMessage : MsmqMessage<NeedIovationResultMessage>
    {
    }
}

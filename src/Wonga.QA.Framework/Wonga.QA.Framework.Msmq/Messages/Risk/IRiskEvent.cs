using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IRiskEvent </summary>
    [XmlRoot("IRiskEvent", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskEvent : MsmqMessage<IRiskEvent>
    {
        public DateTime CreatedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages
{
    /// <summary> Wonga.Payments.InternalMessages.WriteOffApplicationBalance </summary>
    [XmlRoot("WriteOffApplicationBalance", Namespace = "Wonga.Payments.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class WriteOffApplicationBalance : MsmqMessage<WriteOffApplicationBalance>
    {
        public Guid ApplicationId { get; set; }
        public String Reference { get; set; }
    }
}

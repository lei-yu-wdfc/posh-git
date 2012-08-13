using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.PublicMessages
{
    /// <summary> Wonga.Ops.PublicMessages.IPwdResetKeyGenerated </summary>
    [XmlRoot("IPwdResetKeyGenerated", Namespace = "Wonga.Ops.PublicMessages", DataType = "" )
    , SourceAssembly("Wonga.Ops.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPwdResetKeyGenerated : MsmqMessage<IPwdResetKeyGenerated>
    {
        public Guid NotificationId { get; set; }
        public String PwdResetKey { get; set; }
    }
}

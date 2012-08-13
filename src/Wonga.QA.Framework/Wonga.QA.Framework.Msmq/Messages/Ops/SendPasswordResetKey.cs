using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.SendPwdResetKeyMessage </summary>
    [XmlRoot("SendPwdResetKeyMessage", Namespace = "Wonga.Ops", DataType = "" )
    , SourceAssembly("Wonga.Ops.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendPasswordResetKey : MsmqMessage<SendPasswordResetKey>
    {
        public Guid NotificationId { get; set; }
        public String PwdResetKey { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

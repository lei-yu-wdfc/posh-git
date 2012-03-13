using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Ops.SendPwdResetKeyMessage </summary>
    [XmlRoot("SendPwdResetKeyMessage", Namespace = "Wonga.Ops", DataType = "")]
    public partial class SendPasswordResetKeyCommand : MsmqMessage<SendPasswordResetKeyCommand>
    {
        public Guid NotificationId { get; set; }
        public String PwdResetKey { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

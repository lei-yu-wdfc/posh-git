using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Ops.ResetPasswordMessage </summary>
    [XmlRoot("ResetPasswordMessage", Namespace = "Wonga.Ops", DataType = "")]
    public partial class ResetPasswordCommand : MsmqMessage<ResetPasswordCommand>
    {
        public String PwdResetKey { get; set; }
        public String NewPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

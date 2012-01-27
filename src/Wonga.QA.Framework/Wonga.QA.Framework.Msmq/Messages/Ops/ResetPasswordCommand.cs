using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    [XmlRoot("ResetPasswordMessage", Namespace = "Wonga.Ops", DataType = "")]
    public class ResetPasswordCommand : MsmqMessage<ResetPasswordCommand>
    {
        public String PwdResetKey { get; set; }
        public String NewPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

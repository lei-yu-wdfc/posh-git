using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.ResetPasswordMessage </summary>
    [XmlRoot("ResetPasswordMessage", Namespace = "Wonga.Ops", DataType = "" )
    , SourceAssembly("Wonga.Ops.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ResetPassword : MsmqMessage<ResetPassword>
    {
        public String PwdResetKey { get; set; }
        public String NewPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

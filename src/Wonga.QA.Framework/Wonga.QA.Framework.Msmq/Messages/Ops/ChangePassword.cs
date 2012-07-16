using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.ChangePasswordMessage </summary>
    [XmlRoot("ChangePasswordMessage", Namespace = "Wonga.Ops", DataType = "")]
    public partial class ChangePassword : MsmqMessage<ChangePassword>
    {
        public Guid AccountId { get; set; }
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

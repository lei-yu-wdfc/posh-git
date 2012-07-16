using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Ops;

namespace Wonga.QA.Framework.Msmq.Messages.Ops
{
    /// <summary> Wonga.Ops.GeneratePwdResetKeyMessage </summary>
    [XmlRoot("GeneratePwdResetKeyMessage", Namespace = "Wonga.Ops", DataType = "")]
    public partial class GeneratePasswordResetKey : MsmqMessage<GeneratePasswordResetKey>
    {
        public Guid NotificationId { get; set; }
        public PwdResetKeyComplexityEnum Complexity { get; set; }
        public String Login { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    [XmlRoot("GeneratePwdResetKeyMessage", Namespace = "Wonga.Ops", DataType = "")]
    public class GeneratePasswordResetKeyCommand : MsmqMessage<GeneratePasswordResetKeyCommand>
    {
        public Guid NotificationId { get; set; }
        public PwdResetKeyComplexityEnum Complexity { get; set; }
        public String Login { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.CompleteHomePhoneVerificationMessage </summary>
    [XmlRoot("CompleteHomePhoneVerificationMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class CompleteHomePhoneVerification : MsmqMessage<CompleteHomePhoneVerification>
    {
        public Guid VerificationId { get; set; }
        public String Pin { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

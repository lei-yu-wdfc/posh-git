using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CompleteHomePhoneVerificationMessage", Namespace = "Wonga.Comms.Commands.Ca", DataType = "")]
    public partial class CompleteHomePhoneVerificationCaCommand : MsmqMessage<CompleteHomePhoneVerificationCaCommand>
    {
        public Guid VerificationId { get; set; }
        public String Pin { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

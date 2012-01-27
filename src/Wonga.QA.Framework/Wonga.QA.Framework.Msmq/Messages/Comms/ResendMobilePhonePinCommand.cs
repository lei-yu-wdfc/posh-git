using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ResendMobilePhonePinMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public class ResendMobilePhonePinCommand : MsmqMessage<ResendMobilePhonePinCommand>
    {
        public Guid VerificationId { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

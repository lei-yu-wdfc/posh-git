using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.ResendMobilePhonePinMessage </summary>
    [XmlRoot("ResendMobilePhonePinMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class ResendMobilePhonePinCommand : MsmqMessage<ResendMobilePhonePinCommand>
    {
        public Guid VerificationId { get; set; }
        public String Forename { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

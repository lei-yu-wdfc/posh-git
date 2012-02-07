using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendPasswordChangedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public partial class SendPasswordChangedEmailCommand : MsmqMessage<SendPasswordChangedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendAddressUpdatedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email", DataType = "")]
    public class SendAddressUpdatedEmailCommand : MsmqMessage<SendAddressUpdatedEmailCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
    }
}

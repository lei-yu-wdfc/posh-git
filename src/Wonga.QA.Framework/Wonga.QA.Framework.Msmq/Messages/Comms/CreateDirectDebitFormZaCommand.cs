using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateDirectDebitFormMessage", Namespace = "Wonga.Comms.InternalMessages.Za", DataType = "")]
    public class CreateDirectDebitFormZaCommand : MsmqMessage<CreateDirectDebitFormZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

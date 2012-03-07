using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Za.CreateDirectDebitFormMessage </summary>
    [XmlRoot("CreateDirectDebitFormMessage", Namespace = "Wonga.Comms.InternalMessages.Za", DataType = "")]
    public partial class CreateDirectDebitFormZaCommand : MsmqMessage<CreateDirectDebitFormZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

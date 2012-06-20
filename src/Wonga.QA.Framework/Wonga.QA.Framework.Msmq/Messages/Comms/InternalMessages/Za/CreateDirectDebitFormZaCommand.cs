using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Za.CreateDirectDebitFormMessage </summary>
    [XmlRoot("CreateDirectDebitFormMessage", Namespace = "Wonga.Comms.InternalMessages.Za", DataType = "")]
    public partial class CreateDirectDebitFormZaCommand : MsmqMessage<CreateDirectDebitFormZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

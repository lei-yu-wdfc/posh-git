using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Za
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Za.IDirectDebitProducedInternal </summary>
    [XmlRoot("IDirectDebitProducedInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Za", DataType = "Wonga.Comms.PublicMessages.Za.IDirectDebitProduced")]
    public partial class IDirectDebitProducedInternalZaEvent : MsmqMessage<IDirectDebitProducedInternalZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

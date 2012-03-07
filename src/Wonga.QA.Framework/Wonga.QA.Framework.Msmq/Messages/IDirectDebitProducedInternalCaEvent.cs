using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Ca.IDirectDebitProducedInternal </summary>
    [XmlRoot("IDirectDebitProducedInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Ca", DataType = "Wonga.Comms.PublicMessages.Ca.IDirectDebitProduced")]
    public partial class IDirectDebitProducedInternalCaEvent : MsmqMessage<IDirectDebitProducedInternalCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

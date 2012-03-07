using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Za.IDirectDebitProduced </summary>
    [XmlRoot("IDirectDebitProduced", Namespace = "Wonga.Comms.PublicMessages.Za", DataType = "")]
    public partial class IDirectDebitProducedZaEvent : MsmqMessage<IDirectDebitProducedZaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

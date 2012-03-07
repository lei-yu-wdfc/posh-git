using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Ca.IDirectDebitProduced </summary>
    [XmlRoot("IDirectDebitProduced", Namespace = "Wonga.Comms.PublicMessages.Ca", DataType = "")]
    public partial class IDirectDebitProducedCaEvent : MsmqMessage<IDirectDebitProducedCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
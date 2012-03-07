using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.ITopUpAgreementProduced </summary>
    [XmlRoot("ITopUpAgreementProduced", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ITopUpAgreementProducedEvent : MsmqMessage<ITopUpAgreementProducedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopUpId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

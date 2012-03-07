using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.ITopUpAgreementProducedInternal </summary>
    [XmlRoot("ITopUpAgreementProducedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.ITopUpAgreementProduced")]
    public partial class ITopUpAgreementProducedInternalEvent : MsmqMessage<ITopUpAgreementProducedInternalEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopUpId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

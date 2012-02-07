using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("ILoanAgreementProduced", Namespace = "Wonga.Comms.PublicMessages", DataType = "")]
    public partial class ILoanAgreementProducedEvent : MsmqMessage<ILoanAgreementProducedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

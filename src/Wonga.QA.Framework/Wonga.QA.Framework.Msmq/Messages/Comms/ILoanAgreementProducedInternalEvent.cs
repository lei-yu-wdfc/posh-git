using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.ILoanAgreementProducedInternal </summary>
    [XmlRoot("ILoanAgreementProducedInternal", Namespace = "Wonga.Comms.InternalMessages", DataType = "Wonga.Comms.PublicMessages.ILoanAgreementProduced")]
    public partial class ILoanAgreementProducedInternalEvent : MsmqMessage<ILoanAgreementProducedInternalEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

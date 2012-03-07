using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Ca.ILoanAgreementCancellationNoticeProducedInternal </summary>
    [XmlRoot("ILoanAgreementCancellationNoticeProducedInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Ca", DataType = "Wonga.Comms.PublicMessages.Ca.ILoanAgreementCancellationNoticeProduced")]
    public partial class ILoanAgreementCancellationNoticeProducedInternalCaEvent : MsmqMessage<ILoanAgreementCancellationNoticeProducedInternalCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}

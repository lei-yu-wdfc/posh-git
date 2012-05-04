using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Ca.ILoanAgreementCancellationNoticeProduced </summary>
    [XmlRoot("ILoanAgreementCancellationNoticeProduced", Namespace = "Wonga.Comms.PublicMessages.Ca", DataType = "")]
    public partial class ILoanAgreementCancellationNoticeProducedCaEvent : MsmqMessage<ILoanAgreementCancellationNoticeProducedCaEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
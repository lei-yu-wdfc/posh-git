using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.BritishColombia.CreateBcLoanAgreementCancellationNoticeMessage </summary>
    [XmlRoot("CreateBcLoanAgreementCancellationNoticeMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public partial class CreateBcLoanAgreementCancellationNoticeCaCommand : MsmqMessage<CreateBcLoanAgreementCancellationNoticeCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.CreateLoanAgreementCancellationNoticeMessage </summary>
    [XmlRoot("CreateLoanAgreementCancellationNoticeMessage", Namespace = "Wonga.Comms.InternalMessages.Ca", DataType = "")]
    public partial class CreateLoanAgreementCancellationNoticeCaCommand : MsmqMessage<CreateLoanAgreementCancellationNoticeCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

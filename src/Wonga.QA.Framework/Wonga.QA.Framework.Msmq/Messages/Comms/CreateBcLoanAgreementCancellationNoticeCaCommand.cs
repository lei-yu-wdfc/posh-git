using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateBcLoanAgreementCancellationNoticeMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public class CreateBcLoanAgreementCancellationNoticeCaCommand : MsmqMessage<CreateBcLoanAgreementCancellationNoticeCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

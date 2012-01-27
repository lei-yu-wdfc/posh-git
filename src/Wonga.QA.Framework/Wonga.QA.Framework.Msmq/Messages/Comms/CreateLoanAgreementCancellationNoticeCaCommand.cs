using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateLoanAgreementCancellationNoticeMessage", Namespace = "Wonga.Comms.InternalMessages.Ca", DataType = "")]
    public class CreateLoanAgreementCancellationNoticeCaCommand : MsmqMessage<CreateLoanAgreementCancellationNoticeCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

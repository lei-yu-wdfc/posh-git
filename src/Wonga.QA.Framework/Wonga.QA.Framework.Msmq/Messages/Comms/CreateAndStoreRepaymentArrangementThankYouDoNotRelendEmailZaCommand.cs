using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreRepaymentArrangementThankYouDoNotRelendEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementThankYouDoNotRelendEmailZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementThankYouDoNotRelendEmailZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

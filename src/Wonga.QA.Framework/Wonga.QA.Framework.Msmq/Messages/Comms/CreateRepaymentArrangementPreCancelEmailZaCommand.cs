using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateRepaymentArrangementPreCancelEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementPreCancelEmailZaCommand : MsmqMessage<CreateRepaymentArrangementPreCancelEmailZaCommand>
    {
        public Guid RepaymentArrangementDetailId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateRepaymentArrangementPaymentReminderEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementPaymentReminderEmailZaCommand : MsmqMessage<CreateRepaymentArrangementPaymentReminderEmailZaCommand>
    {
        public Guid RepaymentArrangementDetailId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

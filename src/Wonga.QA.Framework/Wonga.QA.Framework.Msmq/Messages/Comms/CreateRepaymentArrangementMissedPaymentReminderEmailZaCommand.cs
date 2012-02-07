using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateRepaymentArrangementMissedPaymentReminderEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementMissedPaymentReminderEmailZaCommand : MsmqMessage<CreateRepaymentArrangementMissedPaymentReminderEmailZaCommand>
    {
        public Guid RepaymentArrangementDetailId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

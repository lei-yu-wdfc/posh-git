using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateRepaymentArrangementPaymentReminderEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementPaymentReminderEmailCompleteZaCommand : MsmqMessage<CreateRepaymentArrangementPaymentReminderEmailCompleteZaCommand>
    {
        public Guid AccountId { get; set; }
        public Byte[] HtmlContent { get; set; }
        public Byte[] PlainContent { get; set; }
    }
}

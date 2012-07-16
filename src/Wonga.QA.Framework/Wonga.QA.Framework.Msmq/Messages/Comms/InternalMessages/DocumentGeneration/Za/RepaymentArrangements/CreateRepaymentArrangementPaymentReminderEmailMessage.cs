using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateRepaymentArrangementPaymentReminderEmailMessage </summary>
    [XmlRoot("CreateRepaymentArrangementPaymentReminderEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementPaymentReminderEmailMessage : MsmqMessage<CreateRepaymentArrangementPaymentReminderEmailMessage>
    {
        public Guid RepaymentArrangementDetailId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

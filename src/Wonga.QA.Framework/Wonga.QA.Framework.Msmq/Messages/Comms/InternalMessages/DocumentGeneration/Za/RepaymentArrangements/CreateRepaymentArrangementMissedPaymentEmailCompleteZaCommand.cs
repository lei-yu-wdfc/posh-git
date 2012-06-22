using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateRepaymentArrangementMissedPaymentEmailCompleteMessage </summary>
    [XmlRoot("CreateRepaymentArrangementMissedPaymentEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementMissedPaymentEmailCompleteZaCommand : MsmqMessage<CreateRepaymentArrangementMissedPaymentEmailCompleteZaCommand>
    {
        public Guid AccountId { get; set; }
        public Byte[] HtmlContent { get; set; }
        public Byte[] PlainContent { get; set; }
    }
}

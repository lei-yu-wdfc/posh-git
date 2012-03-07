using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage </summary>
    [XmlRoot("CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteZaCommand : MsmqMessage<CreateRepaymentArrangementPartiallyRepaidEarlyEmailCompleteZaCommand>
    {
        public Guid AccountId { get; set; }
        public Byte[] HtmlContent { get; set; }
        public Byte[] PlainContent { get; set; }
    }
}

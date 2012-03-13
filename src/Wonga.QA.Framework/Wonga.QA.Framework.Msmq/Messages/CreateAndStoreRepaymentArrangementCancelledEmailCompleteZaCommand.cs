using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateAndStoreRepaymentArrangementCancelledEmailCompleteMessage </summary>
    [XmlRoot("CreateAndStoreRepaymentArrangementCancelledEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementCancelledEmailCompleteZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementCancelledEmailCompleteZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}

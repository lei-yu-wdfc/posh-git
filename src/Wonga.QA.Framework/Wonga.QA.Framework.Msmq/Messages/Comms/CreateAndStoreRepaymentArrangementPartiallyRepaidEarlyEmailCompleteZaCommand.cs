using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateAndStoreRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage </summary>
    [XmlRoot("CreateAndStoreRepaymentArrangementPartiallyRepaidEarlyEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementPartiallyRepaidEarlyEmailCompleteZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementPartiallyRepaidEarlyEmailCompleteZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}

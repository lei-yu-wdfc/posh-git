using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreRepaymentArrangementThankYouDoNotRelendEmailCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementThankYouDoNotRelendEmailCompleteZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementThankYouDoNotRelendEmailCompleteZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid HtmlFileId { get; set; }
        public Guid PlainFileId { get; set; }
    }
}

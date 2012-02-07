using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreRepaymentArrangementCancelledEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementCancelledEmailZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementCancelledEmailZaCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("CreateAndStoreRepaymentArrangementCreatedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementCreatedEmailZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementCreatedEmailZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

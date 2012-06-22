using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements.CreateAndStoreRepaymentArrangementThankYouEmailMessage </summary>
    [XmlRoot("CreateAndStoreRepaymentArrangementThankYouEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Za.RepaymentArrangements", DataType = "")]
    public partial class CreateAndStoreRepaymentArrangementThankYouEmailZaCommand : MsmqMessage<CreateAndStoreRepaymentArrangementThankYouEmailZaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid RepaymentArrangementId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("ICashOutInstructionSent", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "")]
    public class ICashOutInstructionSentEvent : MsmqMessage<ICashOutInstructionSentEvent>
    {
        public DateTime CreatedOn { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}

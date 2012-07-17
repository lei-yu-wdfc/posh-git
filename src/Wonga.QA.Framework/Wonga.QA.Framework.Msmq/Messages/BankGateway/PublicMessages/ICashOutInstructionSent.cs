using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.PublicMessages
{
    /// <summary> Wonga.BankGateway.PublicMessages.ICashOutInstructionSent </summary>
    [XmlRoot("ICashOutInstructionSent", Namespace = "Wonga.BankGateway.PublicMessages", DataType = "")]
    public partial class ICashOutInstructionSent : MsmqMessage<ICashOutInstructionSent>
    {
        public DateTime CreatedOn { get; set; }
        public Guid AccountId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
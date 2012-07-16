using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.UI
{
    /// <summary> Wonga.Risk.UI.ManualVerificationDecisionMessage </summary>
    [XmlRoot("ManualVerificationDecisionMessage", Namespace = "Wonga.Risk.UI", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ManualVerificationDecisionMessage : MsmqMessage<ManualVerificationDecisionMessage>
    {
        public Guid AccountId { get; set; }
        public Int32 Probability { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

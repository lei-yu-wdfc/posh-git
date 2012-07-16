using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Transunion.InternalMessages
{
    /// <summary> Wonga.Transunion.InternalMessages.LoanClosedOrCanceledMessage </summary>
    [XmlRoot("LoanClosedOrCanceledMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class LoanClosedOrCanceledMessage : MsmqMessage<LoanClosedOrCanceledMessage>
    {
        public Object BureauEnquiry { get; set; }
        public Guid SagaId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Transunion.InternalMessages.LoanClosedOrCanceledMessage </summary>
    [XmlRoot("LoanClosedOrCanceledMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class LoanClosedOrCanceledCommand : MsmqMessage<LoanClosedOrCanceledCommand>
    {
        public Object BureauEnquiry { get; set; }
        public Guid SagaId { get; set; }
    }
}

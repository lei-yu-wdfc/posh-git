using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.TransUnion
{
    [XmlRoot("LoanClosedOrCanceledMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public class LoanClosedOrCanceledCommand : MsmqMessage<LoanClosedOrCanceledCommand>
    {
        public Object BureauEnquiry { get; set; }
        public Guid SagaId { get; set; }
    }
}

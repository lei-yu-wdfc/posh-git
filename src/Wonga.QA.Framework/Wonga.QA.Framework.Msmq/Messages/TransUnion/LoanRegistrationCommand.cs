using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.TransUnion
{
    /// <summary> Wonga.Transunion.InternalMessages.LoanRegistrationMessage </summary>
    [XmlRoot("LoanRegistrationMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class LoanRegistrationCommand : MsmqMessage<LoanRegistrationCommand>
    {
        public Object BureauEnquiry { get; set; }
        public Guid SagaId { get; set; }
    }
}

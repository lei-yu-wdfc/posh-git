using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.CallValidateBusinessRequestMessage </summary>
    [XmlRoot("CallValidateBusinessRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class CallValidateBusinessRequestWbUkCommand : MsmqMessage<CallValidateBusinessRequestWbUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

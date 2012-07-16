using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.CallValidateBusinessRequestMessage </summary>
    [XmlRoot("CallValidateBusinessRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class CallValidateBusinessRequestMessage : MsmqMessage<CallValidateBusinessRequestMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Decimal LoanAmount { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

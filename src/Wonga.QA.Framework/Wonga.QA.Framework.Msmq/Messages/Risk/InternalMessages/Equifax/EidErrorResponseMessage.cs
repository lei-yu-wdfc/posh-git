using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Equifax
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EidErrorResponseMessage </summary>
    [XmlRoot("EidErrorResponseMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.InternalMessages.Equifax.EidBaseResponseMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class EidErrorResponseMessage : MsmqMessage<EidErrorResponseMessage>
    {
        public String[] ReasonCodes { get; set; }
        public String Text { get; set; }
        public String TransactionKey { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

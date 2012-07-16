using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToPerformBalanceTransferFailedMessageResponse </summary>
    [XmlRoot("IMakeRequestToPerformBalanceTransferFailedMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToPerformBalanceTransferFailedMessageResponse : MsmqMessage<IMakeRequestToPerformBalanceTransferFailedMessageResponse>
    {
        public String PpsLocalTimeStamp { get; set; }
        public String ErrorMessage { get; set; }
        public Guid SagaId { get; set; }
    }
}

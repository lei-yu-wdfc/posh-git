using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToPerformBalanceTransferMessageResponse </summary>
    [XmlRoot("IMakeRequestToPerformBalanceTransferMessageResponse", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToPerformBalanceTransferResponseEvent : MsmqMessage<IMakeRequestToPerformBalanceTransferResponseEvent>
    {
        public DateTime? ProviderTransactionTimeStamp { get; set; }
        public DateTime? TransactionTimeStamp { get; set; }
        public String ErrorMessage { get; set; }
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;
using Wonga.QA.Framework.Msmq.Enums.PpsProvider.InternalMessages.SagaMessages;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToPerformBalanceTransferMessage </summary>
    [XmlRoot("IMakeRequestToPerformBalanceTransferMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToPerformBalanceTransferEvent : MsmqMessage<IMakeRequestToPerformBalanceTransferEvent>
    {
        public Decimal Amount { get; set; }
        public String CardSerialNumber { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public TransferEnum WongaTransferType { get; set; }
        public PpsTransferRequestEnum PpsTransferRequestType { get; set; }
        public Guid SagaId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;
using Wonga.QA.Framework.Msmq.Enums.PublicMessages.Payments.PrepaidCard.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToPerformBalanceTransferMessage </summary>
    [XmlRoot("IMakeRequestToPerformBalanceTransferMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.PpsProvider.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMakeRequestToPerformBalanceTransferMessage : MsmqMessage<IMakeRequestToPerformBalanceTransferMessage>
    {
        public Decimal Amount { get; set; }
        public String CardSerialNumber { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public TransferEnum WongaTransferType { get; set; }
        public PpsTransferRequestEnum PpsTransferRequestType { get; set; }
        public DateTime RequestedOn { get; set; }
        public Guid SagaId { get; set; }
    }
}

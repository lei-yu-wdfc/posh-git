using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.Ca;
using Wonga.QA.Framework.Msmq.Enums.PpsProvider.InternalMessages.SagaMessages;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer.IWantToMakeAccountBalanceTransfer </summary>
    [XmlRoot("IWantToMakeAccountBalanceTransfer", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer", DataType = "")]
    public partial class IWantToMakeAccountBalanceTransferEvent : MsmqMessage<IWantToMakeAccountBalanceTransferEvent>
    {
        public Guid SagaId { get; set; }
        public Decimal Amount { get; set; }
        public Guid? ApplicationId { get; set; }
        public CurrencyCodeIso4217Enum CurrencyCode { get; set; }
        public Guid CustomerExternalId { get; set; }
        public Guid CardDetailsExternalId { get; set; }
        public PpsTransferRequestEnum PpsTransferRequestType { get; set; }
        public TransferEnum WongaTransferType { get; set; }
        public String OriginalMessageId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

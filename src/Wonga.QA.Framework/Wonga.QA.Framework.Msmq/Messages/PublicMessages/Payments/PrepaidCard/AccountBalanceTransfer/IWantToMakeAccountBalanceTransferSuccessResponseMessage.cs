using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer.IWantToMakeAccountBalanceTransferSuccessResponseMessage </summary>
    [XmlRoot("IWantToMakeAccountBalanceTransferSuccessResponseMessage", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer", DataType = "")]
    public partial class IWantToMakeAccountBalanceTransferSuccessResponseMessage : MsmqMessage<IWantToMakeAccountBalanceTransferSuccessResponseMessage>
    {
        public Guid SagaId { get; set; }
        public Guid TransactionId { get; set; }
        public String PpsLocalTimeStamp { get; set; }
        public DateTime SucceededOn { get; set; }
        public String AditionalResponseInformation { get; set; }
    }
}

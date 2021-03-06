using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer.IWantToMakeAccountBalanceTransferFailedResponseMessage </summary>
    [XmlRoot("IWantToMakeAccountBalanceTransferFailedResponseMessage", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.AccountBalanceTransfer", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToMakeAccountBalanceTransferFailedResponseMessage : MsmqMessage<IWantToMakeAccountBalanceTransferFailedResponseMessage>
    {
        public Guid SagaId { get; set; }
        public Guid? TransactionId { get; set; }
        public String PpsLocalTimeStamp { get; set; }
        public DateTime FailedOn { get; set; }
        public String AditionalResponseInformation { get; set; }
    }
}

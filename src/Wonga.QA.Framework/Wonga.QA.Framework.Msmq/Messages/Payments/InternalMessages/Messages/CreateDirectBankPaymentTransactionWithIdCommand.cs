using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreateDirectBankPaymentTransactionWithIdMessage </summary>
    [XmlRoot("CreateDirectBankPaymentTransactionWithIdMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class CreateDirectBankPaymentTransactionWithIdCommand : MsmqMessage<CreateDirectBankPaymentTransactionWithIdCommand>
    {
        public Guid TransactionId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal TransactionAmount { get; set; }
        public String Reference { get; set; }
    }
}

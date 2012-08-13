using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.CreateDirectBankPaymentTransactionWithIdMessage </summary>
    [XmlRoot("CreateDirectBankPaymentTransactionWithIdMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateDirectBankPaymentTransactionWithIdMessage : MsmqMessage<CreateDirectBankPaymentTransactionWithIdMessage>
    {
        public Guid TransactionId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal TransactionAmount { get; set; }
        public String Reference { get; set; }
    }
}

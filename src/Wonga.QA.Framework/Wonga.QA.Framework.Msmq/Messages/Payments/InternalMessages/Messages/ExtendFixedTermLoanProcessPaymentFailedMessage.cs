using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendFixedTermLoanProcessPaymentFailedMessage </summary>
    [XmlRoot("ExtendFixedTermLoanProcessPaymentFailedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "" )
    , SourceAssembly("Wonga.Payments.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExtendFixedTermLoanProcessPaymentFailedMessage : MsmqMessage<ExtendFixedTermLoanProcessPaymentFailedMessage>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Int32 LoanExtensionInternalId { get; set; }
        public CardPaymentStatusCodeEnum FailureCode { get; set; }
        public Int64 PaymentReference { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime ValueDate { get; set; }
    }
}

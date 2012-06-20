using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Payments.InternalMessages.Messages;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.ExtendFixedTermLoanProcessPaymentFailedMessage </summary>
    [XmlRoot("ExtendFixedTermLoanProcessPaymentFailedMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class ExtendFixedTermLoanProcessPaymentFailedCommand : MsmqMessage<ExtendFixedTermLoanProcessPaymentFailedCommand>
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

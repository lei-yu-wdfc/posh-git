using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages
{
    /// <summary> Wonga.Payments.InternalMessages.SendPaymentMessage </summary>
    [XmlRoot("SendPaymentMessage", Namespace = "Wonga.Payments.InternalMessages", DataType = "")]
    public partial class SendPaymentCommand : MsmqMessage<SendPaymentCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public String BankAccount { get; set; }
        public String BankCode { get; set; }
        public String BankAccountType { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SenderReference { get; set; }
    }
}

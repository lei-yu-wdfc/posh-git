using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Iso;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IWongaPaymentToPrepaidCard </summary>
    [XmlRoot("IWongaPaymentToPrepaidCard", Namespace = "Wonga.Payments.PublicMessages", DataType = "")]
    public partial class IWongaPaymentToPrepaidCard : MsmqMessage<IWongaPaymentToPrepaidCard>
    {
        public Guid AccountId { get; set; }
        public Decimal Amount { get; set; }
        public Guid ApplicationId { get; set; }
        public String BankAccount { get; set; }
        public String BankAccountType { get; set; }
        public String BankCode { get; set; }
        public CurrencyCodeIso4217Enum Currency { get; set; }
        public Guid SenderReference { get; set; }
        public Boolean Succeeded { get; set; }
    }
}

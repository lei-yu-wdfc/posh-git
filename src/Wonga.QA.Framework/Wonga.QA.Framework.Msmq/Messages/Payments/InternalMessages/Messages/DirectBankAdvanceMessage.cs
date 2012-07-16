using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Payments.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.DirectBankAdvanceMessage </summary>
    [XmlRoot("DirectBankAdvanceMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class DirectBankAdvanceMessage : MsmqMessage<DirectBankAdvanceMessage>
    {
        public Int32 ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public PaymentTransactionEnum TransactionType { get; set; }
        public String Reference { get; set; }
    }
}

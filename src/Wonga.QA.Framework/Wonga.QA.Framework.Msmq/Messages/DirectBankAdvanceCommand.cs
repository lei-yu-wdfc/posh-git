using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.DirectBankAdvanceMessage </summary>
    [XmlRoot("DirectBankAdvanceMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class DirectBankAdvanceCommand : MsmqMessage<DirectBankAdvanceCommand>
    {
        public Int32 ApplicationId { get; set; }
        public Decimal Amount { get; set; }
        public PaymentTransactionEnum TransactionType { get; set; }
        public String Reference { get; set; }
    }
}

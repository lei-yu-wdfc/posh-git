using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.RejectedTransactionsReportMessage </summary>
    [XmlRoot("RejectedTransactionsReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "")]
    public partial class RejectedTransactionsReportCaCommand : MsmqMessage<RejectedTransactionsReportCaCommand>
    {
        public Decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public String CustomerBankCode { get; set; }
        public String CustomerAccountNumber { get; set; }
        public String CustomerName { get; set; }
        public Int32 TransactionId { get; set; }
        public String FailureReason { get; set; }
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
    }
}

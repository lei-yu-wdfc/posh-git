using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bmo.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.SettlementReportMessage </summary>
    [XmlRoot("SettlementReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "")]
    public partial class SettlementReportCaCommand : MsmqMessage<SettlementReportCaCommand>
    {
        public Decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public String CustomerBankCode { get; set; }
        public String CustomerAccountNumber { get; set; }
        public String CustomerName { get; set; }
        public Int32? TransactionId { get; set; }
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
    }
}

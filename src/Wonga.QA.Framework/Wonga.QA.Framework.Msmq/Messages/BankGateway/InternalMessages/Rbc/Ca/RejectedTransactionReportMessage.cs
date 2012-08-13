using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Rbc.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Rbc.Ca.RejectedTransactionReportMessage </summary>
    [XmlRoot("RejectedTransactionReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Rbc.Ca", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Rbc.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RejectedTransactionReportMessage : MsmqMessage<RejectedTransactionReportMessage>
    {
        public Decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public String CustomerBankCode { get; set; }
        public String CustomerAccountNumber { get; set; }
        public String CustomerName { get; set; }
        public Int32? TransactionId { get; set; }
        public String FailureReason { get; set; }
        public String FileName { get; set; }
        public Byte[] FileContent { get; set; }
    }
}

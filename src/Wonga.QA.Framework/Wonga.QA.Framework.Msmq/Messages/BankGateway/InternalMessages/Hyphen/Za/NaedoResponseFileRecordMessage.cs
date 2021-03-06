using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.NaedoResponseFileRecordMessage </summary>
    [XmlRoot("NaedoResponseFileRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Hyphen.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class NaedoResponseFileRecordMessage : MsmqMessage<NaedoResponseFileRecordMessage>
    {
        public String AccountName { get; set; }
        public Int64 AccountNumber { get; set; }
        public Int32 AccountType { get; set; }
        public DateTime ActionDate { get; set; }
        public Decimal Amount { get; set; }
        public Int32 BatchNumber { get; set; }
        public Int32 BranchNumber { get; set; }
        public String Code1 { get; set; }
        public String DebtorCreditorCode { get; set; }
        public String DocumentType { get; set; }
        public String ErrorCode { get; set; }
        public Int32 FileSequenceNumber { get; set; }
        public Int32 MessageType { get; set; }
        public Char ProcessingOption1 { get; set; }
        public Char ProcessingOption2 { get; set; }
        public String TransactionType { get; set; }
        public Int32 AcknowledgeTypeId { get; set; }
        public String FileName { get; set; }
        public String UserReference2 { get; set; }
        public String RawContent { get; set; }
    }
}

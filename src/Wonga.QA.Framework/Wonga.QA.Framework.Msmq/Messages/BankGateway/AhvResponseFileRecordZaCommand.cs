using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("AHVResponseFileRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public class AhvResponseFileRecordZaCommand : MsmqMessage<AhvResponseFileRecordZaCommand>
    {
        public String RawContent { get; set; }
        public String FileName { get; set; }
        public String MessageType { get; set; }
        public String AccountName { get; set; }
        public Int64 AccountNumber { get; set; }
        public Int32 AccountType { get; set; }
        public String BankUserReference { get; set; }
        public Int32 BranchNumber { get; set; }
        public String TransactionId { get; set; }
        public String RegistrationNumber { get; set; }
        public Boolean OutIsValidAccountNumber { get; set; }
        public Boolean OutIsMatchNationalNumber { get; set; }
        public Boolean? OutIsMatchLastName { get; set; }
        public Boolean? OutIsActiveAccountNumber { get; set; }
        public String OutErrorMessage { get; set; }
        public Boolean? OutIsActiveMoreThanThreeMonths { get; set; }
        public Boolean? OutAcceptsDebits { get; set; }
        public Boolean? OutAcceptsCredits { get; set; }
    }
}

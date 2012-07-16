using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Za
{
    /// <summary> Wonga.Payments.Za.AddBankAccountZa </summary>
    [XmlRoot("AddBankAccountZa", Namespace = "Wonga.Payments.Za", DataType = "")]
    public partial class AddBankAccountZa : MsmqMessage<AddBankAccountZa>
    {
        public String AccountType { get; set; }
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public String BankName { get; set; }
        public String AccountNumber { get; set; }
        public String HolderName { get; set; }
        public DateTime? AccountOpenDate { get; set; }
        public String CountryCode { get; set; }
        public Boolean IsPrimary { get; set; }
        public String IBAN { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

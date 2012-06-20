using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.Uk
{
    /// <summary> Wonga.Payments.Uk.AddBankAccount </summary>
    [XmlRoot("AddBankAccount", Namespace = "Wonga.Payments.Uk", DataType = "")]
    public partial class AddBankAccountUkCommand : MsmqMessage<AddBankAccountUkCommand>
    {
        public String BankCode { get; set; }
        public Boolean IsValid { get; set; }
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public String BankName { get; set; }
        public String AccountNumber { get; set; }
        public String HolderName { get; set; }
        public DateTime AccountOpenDate { get; set; }
        public String CountryCode { get; set; }
        public Boolean IsPrimary { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

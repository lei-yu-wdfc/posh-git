using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Ca
{
    /// <summary> Wonga.Payments.Ca.AddBankAccount </summary>
    [XmlRoot("AddBankAccount", Namespace = "Wonga.Payments.Ca", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddBankAccount : MsmqMessage<AddBankAccount>
    {
        public String BranchNumber { get; set; }
        public String InstitutionNumber { get; set; }
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

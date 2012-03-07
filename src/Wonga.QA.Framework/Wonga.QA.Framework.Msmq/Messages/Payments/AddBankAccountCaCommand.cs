using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.Ca.AddBankAccount </summary>
    [XmlRoot("AddBankAccount", Namespace = "Wonga.Payments.Ca", DataType = "")]
    public partial class AddBankAccountCaCommand : MsmqMessage<AddBankAccountCaCommand>
    {
        public String BranchNumber { get; set; }
        public String InstitutionNumber { get; set; }
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

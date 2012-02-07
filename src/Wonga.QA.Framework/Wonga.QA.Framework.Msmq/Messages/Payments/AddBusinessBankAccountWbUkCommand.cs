using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("AddBusinessBankAccount", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "")]
    public partial class AddBusinessBankAccountWbUkCommand : MsmqMessage<AddBusinessBankAccountWbUkCommand>
    {
        public Guid OrganisationId { get; set; }
        public Guid BankAccountId { get; set; }
        public String BankName { get; set; }
        public String AccountNumber { get; set; }
        public String HolderName { get; set; }
        public DateTime AccountOpenDate { get; set; }
        public String CountryCode { get; set; }
        public String BankCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("AddBankAccount")]
    public partial class AddBankAccountCaCommand : ApiRequest<AddBankAccountCaCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object BranchNumber { get; set; }
        public Object InstitutionNumber { get; set; }
        public Object AccountNumber { get; set; }
        public Object HolderName { get; set; }
        public Object AccountOpenDate { get; set; }
        public Object CountryCode { get; set; }
        public Object IsPrimary { get; set; }
    }
}

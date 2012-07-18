using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Pl
{
    /// <summary> Wonga.Payments.Commands.Pl.AddBankAccount </summary>
    [XmlRoot("AddBankAccount")]
    public partial class AddBankAccountPlCommand : ApiRequest<AddBankAccountPlCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object BankCode { get; set; }
        public Object AccountNumber { get; set; }
        public Object HolderName { get; set; }
        public Object AccountOpenDate { get; set; }
        public Object CountryCode { get; set; }
        public Object IsPrimary { get; set; }
        public Object IBAN { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Za
{
    /// <summary> Wonga.Payments.Commands.Za.AddBankAccountZa </summary>
    [XmlRoot("AddBankAccountZa")]
    public partial class AddBankAccountZaCommand : ApiRequest<AddBankAccountZaCommand>
    {
        public Object AccountId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object AccountType { get; set; }
        public Object AccountNumber { get; set; }
        public Object HolderName { get; set; }
        public Object AccountOpenDate { get; set; }
        public Object CountryCode { get; set; }
        public Object IsPrimary { get; set; }
    }
}

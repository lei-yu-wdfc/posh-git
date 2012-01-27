using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("AddBusinessBankAccount")]
    public class AddBusinessBankAccountWbUkCommand : ApiRequest<AddBusinessBankAccountWbUkCommand>
    {
        public Object OrganisationId { get; set; }
        public Object BankAccountId { get; set; }
        public Object BankName { get; set; }
        public Object BankCode { get; set; }
        public Object AccountNumber { get; set; }
        public Object HolderName { get; set; }
        public Object AccountOpenDate { get; set; }
        public Object CountryCode { get; set; }
    }
}

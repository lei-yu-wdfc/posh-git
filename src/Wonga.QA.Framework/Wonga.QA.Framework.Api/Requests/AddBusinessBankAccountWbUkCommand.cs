using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.AddBusinessBankAccount </summary>
    [XmlRoot("AddBusinessBankAccount")]
    public partial class AddBusinessBankAccountWbUkCommand : ApiRequest<AddBusinessBankAccountWbUkCommand>
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

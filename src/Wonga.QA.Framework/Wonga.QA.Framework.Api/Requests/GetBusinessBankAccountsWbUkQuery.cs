using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Wb.Uk.GetBusinessBankAccounts </summary>
    [XmlRoot("GetBusinessBankAccounts")]
    public partial class GetBusinessBankAccountsWbUkQuery : ApiRequest<GetBusinessBankAccountsWbUkQuery>
    {
        public Object OrganisationId { get; set; }
    }
}

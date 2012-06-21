using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetBankAccounts </summary>
    [XmlRoot("GetBankAccounts")]
    public partial class GetBankAccountsQuery : ApiRequest<GetBankAccountsQuery>
    {
        public Object AccountId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetBankAccounts </summary>
    [XmlRoot("GetBankAccounts")]
    public partial class GetBankAccountsQuery : ApiRequest<GetBankAccountsQuery>
    {
        public Object AccountId { get; set; }
    }
}

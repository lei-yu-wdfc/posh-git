using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBankAccounts")]
    public class GetBankAccountsQuery : ApiRequest<GetBankAccountsQuery>
    {
        public Object AccountId { get; set; }
    }
}

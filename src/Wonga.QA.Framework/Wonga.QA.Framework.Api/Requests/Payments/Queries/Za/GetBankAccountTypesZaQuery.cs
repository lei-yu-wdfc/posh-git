using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Za
{
    /// <summary> Wonga.Payments.Queries.Za.GetBankAccountTypes </summary>
    [XmlRoot("GetBankAccountTypes")]
    public partial class GetBankAccountTypesZaQuery : ApiRequest<GetBankAccountTypesZaQuery>
    {
    }
}

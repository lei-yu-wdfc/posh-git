using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetBanks </summary>
    [XmlRoot("GetBanks")]
    public partial class GetBanksQuery : ApiRequest<GetBanksQuery>
    {
    }
}

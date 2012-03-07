using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetBanks </summary>
    [XmlRoot("GetBanks")]
    public partial class GetBanksQuery : ApiRequest<GetBanksQuery>
    {
    }
}

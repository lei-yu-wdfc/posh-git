using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
    /// <summary> Wonga.PpsProvider.Queries.GetPrepaidAccountMaximumTopupAllowed </summary>
    [XmlRoot("GetPrepaidAccountMaximumTopupAllowed")]
    public partial class GetPrepaidAccountMaximumTopupAllowedQuery : ApiRequest<GetPrepaidAccountMaximumTopupAllowedQuery>
    {
        public Object AccountId { get; set; }
    }
}

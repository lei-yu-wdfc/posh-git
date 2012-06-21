using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
    /// <summary> Wonga.Comms.Queries.GetReviewDetails </summary>
    [XmlRoot("GetReviewDetails")]
    public partial class GetReviewDetailsQuery : ApiRequest<GetReviewDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}

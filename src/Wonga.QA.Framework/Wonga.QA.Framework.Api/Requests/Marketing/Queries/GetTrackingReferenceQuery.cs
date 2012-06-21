using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Queries
{
    /// <summary> Wonga.Marketing.Queries.GetTrackingReference </summary>
    [XmlRoot("GetTrackingReference")]
    public partial class GetTrackingReferenceQuery : ApiRequest<GetTrackingReferenceQuery>
    {
        public Object TrackingSession { get; set; }
    }
}

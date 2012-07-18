using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetRepaymentTimeline </summary>
    [XmlRoot("GetRepaymentTimeline")]
    public partial class GetRepaymentTimelineUkQuery : ApiRequest<GetRepaymentTimelineUkQuery>
    {
        public Object AccountId { get; set; }
    }
}

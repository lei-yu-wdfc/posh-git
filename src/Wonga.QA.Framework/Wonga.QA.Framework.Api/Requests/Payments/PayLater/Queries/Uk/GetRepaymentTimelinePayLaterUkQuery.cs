using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetRepaymentTimeline </summary>
    [XmlRoot("GetRepaymentTimeline")]
    public partial class GetRepaymentTimelinePayLaterUkQuery : ApiRequest<GetRepaymentTimelinePayLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}

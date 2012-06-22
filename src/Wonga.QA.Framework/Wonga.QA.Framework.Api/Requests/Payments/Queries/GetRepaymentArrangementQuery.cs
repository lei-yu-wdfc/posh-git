using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetRepaymentArrangement </summary>
    [XmlRoot("GetRepaymentArrangement")]
    public partial class GetRepaymentArrangementQuery : ApiRequest<GetRepaymentArrangementQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
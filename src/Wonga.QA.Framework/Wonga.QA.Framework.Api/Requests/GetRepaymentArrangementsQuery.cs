using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangements")]
    public partial class GetRepaymentArrangementsQuery : ApiRequest<GetRepaymentArrangementsQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

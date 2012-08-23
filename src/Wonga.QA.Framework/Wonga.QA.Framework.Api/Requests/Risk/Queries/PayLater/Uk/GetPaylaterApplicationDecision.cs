using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries.PayLater.Uk
{
    [XmlRoot("GetPayLaterApplicationDecision")]
    public partial class GetPaylaterApplicationDecision : ApiRequest<GetPaylaterApplicationDecision>
    {
        public Object ApplicationId { get; set; }
    }
}

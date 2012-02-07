using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetApplicationDecision")]
    public partial class GetApplicationDecisionQuery : ApiRequest<GetApplicationDecisionQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

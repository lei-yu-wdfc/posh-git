using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Queries.GetApplicationDecision </summary>
    [XmlRoot("GetApplicationDecision")]
    public partial class GetApplicationDecisionQuery : ApiRequest<GetApplicationDecisionQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
    /// <summary> Wonga.Risk.Queries.GetApplicationDecisionDummy </summary>
    [XmlRoot("GetApplicationDecisionDummy")]
    public partial class GetApplicationDecisionDummyQuery : ApiRequest<GetApplicationDecisionDummyQuery>
    {
        public Object ApplicationId { get; set; }
        public Object SleepInMilliseconds { get; set; }
    }
}

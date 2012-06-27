using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries
{
    /// <summary> Wonga.Risk.Queries.GetDeclineAdvice </summary>
    [XmlRoot("GetDeclineAdvice")]
    public partial class GetDeclineAdviceQuery : ApiRequest<GetDeclineAdviceQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.GetFullDeclineAdvice </summary>
    [XmlRoot("GetFullDeclineAdvice")]
    public partial class GetFullDeclineAdviceQuery : ApiRequest<GetFullDeclineAdviceQuery>
    {
        public Object DeclineAdviceKey { get; set; }
    }
}

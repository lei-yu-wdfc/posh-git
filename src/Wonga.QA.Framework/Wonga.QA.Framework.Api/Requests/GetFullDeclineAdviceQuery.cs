using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFullDeclineAdvice")]
    public class GetFullDeclineAdviceQuery : ApiRequest<GetFullDeclineAdviceQuery>
    {
        public Object DeclineAdviceKey { get; set; }
    }
}

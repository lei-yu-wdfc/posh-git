using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangementAllowed")]
    public class GetRepaymentArrangementAllowedQuery : ApiRequest<GetRepaymentArrangementAllowedQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

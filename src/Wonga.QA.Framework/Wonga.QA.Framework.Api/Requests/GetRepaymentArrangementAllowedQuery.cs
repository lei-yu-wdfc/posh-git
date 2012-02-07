using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangementAllowed")]
    public partial class GetRepaymentArrangementAllowedQuery : ApiRequest<GetRepaymentArrangementAllowedQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangement")]
    public partial class GetRepaymentArrangementQuery : ApiRequest<GetRepaymentArrangementQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

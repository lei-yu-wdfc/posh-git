using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangementParameters")]
    public class GetRepaymentArrangementParametersQuery : ApiRequest<GetRepaymentArrangementParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

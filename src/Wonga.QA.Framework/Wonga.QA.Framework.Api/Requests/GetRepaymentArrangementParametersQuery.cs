using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangementParameters")]
    public partial class GetRepaymentArrangementParametersQuery : ApiRequest<GetRepaymentArrangementParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

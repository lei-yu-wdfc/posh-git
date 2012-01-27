using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepayLoanParameters")]
    public class GetRepayLoanParametersQuery : ApiRequest<GetRepayLoanParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

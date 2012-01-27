using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanTopupCalculation")]
    public class GetFixedTermLoanTopupCalculationQuery : ApiRequest<GetFixedTermLoanTopupCalculationQuery>
    {
        public Object ApplicationId { get; set; }
        public Object TopupAmount { get; set; }
    }
}

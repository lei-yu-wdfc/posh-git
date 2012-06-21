using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetFixedTermLoanTopupCalculation </summary>
    [XmlRoot("GetFixedTermLoanTopupCalculation")]
    public partial class GetFixedTermLoanTopupCalculationQuery : ApiRequest<GetFixedTermLoanTopupCalculationQuery>
    {
        public Object ApplicationId { get; set; }
        public Object TopupAmount { get; set; }
    }
}

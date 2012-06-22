using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetFixedTermLoanCalculation </summary>
    [XmlRoot("GetFixedTermLoanCalculation")]
    public partial class GetFixedTermLoanCalculationQuery : ApiRequest<GetFixedTermLoanCalculationQuery>
    {
        public Object LoanAmount { get; set; }
        public Object Term { get; set; }
        public Object PromoCodeId { get; set; }
    }
}

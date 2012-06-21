using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Za
{
    /// <summary> Wonga.Payments.Queries.Za.GetFixedTermLoanCalculationZa </summary>
    [XmlRoot("GetFixedTermLoanCalculationZa")]
    public partial class GetFixedTermLoanCalculationZaQuery : ApiRequest<GetFixedTermLoanCalculationZaQuery>
    {
        public Object LoanAmount { get; set; }
        public Object Term { get; set; }
        public Object PromoCodeId { get; set; }
    }
}

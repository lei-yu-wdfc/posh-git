using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanCalculationZa")]
    public class GetFixedTermLoanCalculationZaQuery : ApiRequest<GetFixedTermLoanCalculationZaQuery>
    {
        public Object LoanAmount { get; set; }
        public Object Term { get; set; }
        public Object PromoCodeId { get; set; }
    }
}

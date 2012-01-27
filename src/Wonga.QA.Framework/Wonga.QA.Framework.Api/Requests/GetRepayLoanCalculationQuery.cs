using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepayLoanCalculation")]
    public class GetRepayLoanCalculationQuery : ApiRequest<GetRepayLoanCalculationQuery>
    {
        public Object ApplicationId { get; set; }
        public Object RepayAmount { get; set; }
        public Object RepayDate { get; set; }
    }
}

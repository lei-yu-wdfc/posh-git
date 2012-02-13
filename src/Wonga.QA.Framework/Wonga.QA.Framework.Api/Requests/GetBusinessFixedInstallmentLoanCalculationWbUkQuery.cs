using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetBusinessFixedInstallmentLoanCalculation")]
    public partial class GetBusinessFixedInstallmentLoanCalculationWbUkQuery : ApiRequest<GetBusinessFixedInstallmentLoanCalculationWbUkQuery>
    {
        public Object LoanAmount { get; set; }
        public Object Term { get; set; }
    }
}

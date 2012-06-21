using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Wb.Uk
{
    /// <summary> Wonga.Payments.Queries.Wb.Uk.GetBusinessFixedInstallmentLoanCalculation </summary>
    [XmlRoot("GetBusinessFixedInstallmentLoanCalculation")]
    public partial class GetBusinessFixedInstallmentLoanCalculationWbUkQuery : ApiRequest<GetBusinessFixedInstallmentLoanCalculationWbUkQuery>
    {
        public Object LoanAmount { get; set; }
        public Object Term { get; set; }
        public Object ApplicationId { get; set; }
    }
}

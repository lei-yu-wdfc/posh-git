using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetFixedTermLoanExtensionCalculation </summary>
    [XmlRoot("GetFixedTermLoanExtensionCalculation")]
    public partial class GetFixedTermLoanExtensionCalculationQuery : ApiRequest<GetFixedTermLoanExtensionCalculationQuery>
    {
        public Object ApplicationId { get; set; }
        public Object ExtendDate { get; set; }
    }
}

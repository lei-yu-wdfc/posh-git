using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanExtensionCalculation")]
    public class GetFixedTermLoanExtensionCalculationQuery : ApiRequest<GetFixedTermLoanExtensionCalculationQuery>
    {
        public Object ApplicationId { get; set; }
        public Object ExtendDate { get; set; }
    }
}

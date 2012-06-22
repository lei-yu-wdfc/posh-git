using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetFixedTermLoanExtensionParameters </summary>
    [XmlRoot("GetFixedTermLoanExtensionParameters")]
    public partial class GetFixedTermLoanExtensionParametersQuery : ApiRequest<GetFixedTermLoanExtensionParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

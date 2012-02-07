using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanExtensionParameters")]
    public partial class GetFixedTermLoanExtensionParametersQuery : ApiRequest<GetFixedTermLoanExtensionParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

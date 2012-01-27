using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanApplication")]
    public class GetFixedTermLoanApplicationQuery : ApiRequest<GetFixedTermLoanApplicationQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

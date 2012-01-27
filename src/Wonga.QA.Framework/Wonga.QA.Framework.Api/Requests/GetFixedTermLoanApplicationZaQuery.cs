using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanApplicationZa")]
    public class GetFixedTermLoanApplicationZaQuery : ApiRequest<GetFixedTermLoanApplicationZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

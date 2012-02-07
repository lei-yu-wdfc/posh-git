using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetFixedTermLoanApplicationZa")]
    public partial class GetFixedTermLoanApplicationZaQuery : ApiRequest<GetFixedTermLoanApplicationZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

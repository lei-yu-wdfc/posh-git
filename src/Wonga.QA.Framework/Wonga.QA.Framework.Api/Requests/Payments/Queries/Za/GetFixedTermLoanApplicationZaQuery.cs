using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Za
{
    /// <summary> Wonga.Payments.Queries.Za.GetFixedTermLoanApplicationZa </summary>
    [XmlRoot("GetFixedTermLoanApplicationZa")]
    public partial class GetFixedTermLoanApplicationZaQuery : ApiRequest<GetFixedTermLoanApplicationZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

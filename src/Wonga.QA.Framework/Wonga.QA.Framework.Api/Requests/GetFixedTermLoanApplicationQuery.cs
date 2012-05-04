using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetFixedTermLoanApplication </summary>
    [XmlRoot("GetFixedTermLoanApplication")]
    public partial class GetFixedTermLoanApplicationQuery : ApiRequest<GetFixedTermLoanApplicationQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
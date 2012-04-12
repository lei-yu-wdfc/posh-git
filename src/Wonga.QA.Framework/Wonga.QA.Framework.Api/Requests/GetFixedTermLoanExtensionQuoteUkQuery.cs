using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Uk.GetFixedTermLoanExtensionQuote </summary>
    [XmlRoot("GetFixedTermLoanExtensionQuote")]
    public partial class GetFixedTermLoanExtensionQuoteUkQuery : ApiRequest<GetFixedTermLoanExtensionQuoteUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.Uk.GetRepayLoanQuote </summary>
    [XmlRoot("GetRepayLoanQuote")]
    public partial class GetRepayLoanQuoteUkQuery : ApiRequest<GetRepayLoanQuoteUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

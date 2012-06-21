using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries
{
    /// <summary> Wonga.Payments.Csapi.Queries.CsGetFixedTermLoanExtensionQuote </summary>
    [XmlRoot("CsGetFixedTermLoanExtensionQuote")]
    public partial class CsGetFixedTermLoanExtensionQuoteQuery : CsRequest<CsGetFixedTermLoanExtensionQuoteQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

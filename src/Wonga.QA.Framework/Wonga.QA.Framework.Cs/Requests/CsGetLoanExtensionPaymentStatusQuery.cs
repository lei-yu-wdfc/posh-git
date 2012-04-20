using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.CsGetLoanExtensionPaymentStatus </summary>
    [XmlRoot("CsGetLoanExtensionPaymentStatus")]
    public partial class CsGetLoanExtensionPaymentStatusQuery : CsRequest<CsGetLoanExtensionPaymentStatusQuery>
    {
        public Object ExtensionId { get; set; }
        public Object SalesforceUser { get; set; }
    }
}

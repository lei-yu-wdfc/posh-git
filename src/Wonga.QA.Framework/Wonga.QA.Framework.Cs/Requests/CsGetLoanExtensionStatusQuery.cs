using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.CsGetLoanExtensionStatus </summary>
    [XmlRoot("CsGetLoanExtensionStatus")]
    public partial class CsGetLoanExtensionStatusQuery : CsRequest<CsGetLoanExtensionStatusQuery>
    {
        public Object ExtensionId { get; set; }
        public Object CsUser { get; set; }
    }
}

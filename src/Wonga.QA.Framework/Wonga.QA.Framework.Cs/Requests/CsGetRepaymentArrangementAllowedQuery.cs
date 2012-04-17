using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.CsGetRepaymentArrangementAllowed </summary>
    [XmlRoot("CsGetRepaymentArrangementAllowed")]
    public partial class CsGetRepaymentArrangementAllowedQuery : CsRequest<CsGetRepaymentArrangementAllowedQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

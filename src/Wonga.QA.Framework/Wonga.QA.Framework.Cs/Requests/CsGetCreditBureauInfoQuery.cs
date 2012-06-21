using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Risk.Csapi.Queries.CsGetCreditBureauInfo </summary>
    [XmlRoot("CsGetCreditBureauInfo")]
    public partial class CsGetCreditBureauInfoQuery : CsRequest<CsGetCreditBureauInfoQuery>
    {
        public Object AccountId { get; set; }
    }
}

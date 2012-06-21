using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Queries
{
    /// <summary> Wonga.Risk.Csapi.Queries.CsGetSocialDetails </summary>
    [XmlRoot("CsGetSocialDetails")]
    public partial class CsGetSocialDetailsQuery : CsRequest<CsGetSocialDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}

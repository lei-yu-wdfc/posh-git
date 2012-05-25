using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Risk.Csapi.Queries.CsGetEmploymentDetails </summary>
    [XmlRoot("CsGetEmploymentDetails")]
    public partial class CsGetEmploymentDetailsQuery : CsRequest<CsGetEmploymentDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}

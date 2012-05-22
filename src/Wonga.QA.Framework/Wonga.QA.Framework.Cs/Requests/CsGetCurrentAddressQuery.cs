using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Queries.CsGetCurrentAddress </summary>
    [XmlRoot("CsGetCurrentAddress")]
    public partial class CsGetCurrentAddressQuery : CsRequest<CsGetCurrentAddressQuery>
    {
        public Object AccountId { get; set; }
    }
}

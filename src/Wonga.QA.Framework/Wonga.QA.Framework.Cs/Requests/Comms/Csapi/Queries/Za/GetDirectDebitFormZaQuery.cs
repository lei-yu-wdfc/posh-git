using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Comms.Csapi.Queries.Za
{
    /// <summary> Wonga.Comms.Csapi.Queries.Za.GetDirectDebitForm </summary>
    [XmlRoot("GetDirectDebitForm")]
    public partial class GetDirectDebitFormZaQuery : CsRequest<GetDirectDebitFormZaQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

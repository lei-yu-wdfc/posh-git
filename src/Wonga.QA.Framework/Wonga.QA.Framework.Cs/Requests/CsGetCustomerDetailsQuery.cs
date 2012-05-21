using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Queries.CsGetCustomerDetails </summary>
    [XmlRoot("CsGetCustomerDetails")]
    public partial class CsGetCustomerDetailsQuery : CsRequest<CsGetCustomerDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}

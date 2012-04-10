using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetScheduledPayments </summary>
    [XmlRoot("GetScheduledPayments")]
    public partial class GetScheduledPaymentsQuery : CsRequest<GetScheduledPaymentsQuery>
    {
        public Object ApplicationGuid { get; set; }
    }
}

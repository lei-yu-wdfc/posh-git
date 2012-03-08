using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetRepaymentArrangements </summary>
    [XmlRoot("GetRepaymentArrangements")]
    public partial class GetRepaymentArrangementsQuery : CsRequest<GetRepaymentArrangementsQuery>
    {
        public Object ApplicationId { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetRepaymentNumber </summary>
    [XmlRoot("GetRepaymentNumber")]
    public partial class GetRepaymentNumberQuery : CsRequest<GetRepaymentNumberQuery>
    {
        public Object AccountId { get; set; }
    }
}

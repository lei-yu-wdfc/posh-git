using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetRepaymentArrangementParameters </summary>
    [XmlRoot("GetRepaymentArrangementParameters")]
    public partial class GetRepaymentArrangementParametersQuery : CsRequest<GetRepaymentArrangementParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

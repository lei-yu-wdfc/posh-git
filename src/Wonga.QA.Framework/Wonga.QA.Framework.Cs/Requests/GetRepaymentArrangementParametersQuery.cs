using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetRepaymentArrangementParameters </summary>
    [XmlRoot("GetRepaymentArrangementParameters")]
    public partial class GetRepaymentArrangementParametersQuery : CsRequest<GetRepaymentArrangementParametersQuery>
    {
        public Object AccountId { get; set; }
    }
}

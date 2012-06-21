using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetRepaymentArrangementCalculation </summary>
    [XmlRoot("GetRepaymentArrangementCalculation")]
    public partial class GetRepaymentArrangementCalculationQuery : CsRequest<GetRepaymentArrangementCalculationQuery>
    {
        public Object AccountId { get; set; }
        public Object NumberOfMonths { get; set; }
        public Object RepaymentFrequency { get; set; }
        public Object FirstRepaymentDate { get; set; }
    }
}

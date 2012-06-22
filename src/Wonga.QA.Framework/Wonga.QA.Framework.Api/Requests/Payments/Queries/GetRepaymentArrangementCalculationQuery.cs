using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
    /// <summary> Wonga.Payments.Queries.GetRepaymentArrangementCalculation </summary>
    [XmlRoot("GetRepaymentArrangementCalculation")]
    public partial class GetRepaymentArrangementCalculationQuery : ApiRequest<GetRepaymentArrangementCalculationQuery>
    {
        public Object AccountId { get; set; }
        public Object NumberOfMonths { get; set; }
        public Object RepaymentFrequency { get; set; }
        public Object FirstRepaymentDate { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetRepaymentArrangementCalculation")]
    public class GetRepaymentArrangementCalculationQuery : ApiRequest<GetRepaymentArrangementCalculationQuery>
    {
        public Object AccountId { get; set; }
        public Object NumberOfMonths { get; set; }
        public Object RepaymentFrequency { get; set; }
        public Object FirstRepaymentDate { get; set; }
    }
}

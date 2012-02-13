using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CreateRepaymentArrangement")]
    public partial class CreateRepaymentArrangementCommand : ApiRequest<CreateRepaymentArrangementCommand>
    {
        public Object ApplicationId { get; set; }
        public Object Frequency { get; set; }
        public Object RepaymentDates { get; set; }
        public Object NumberOfMonths { get; set; }
    }
}

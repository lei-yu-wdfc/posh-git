using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    /// <summary> Wonga.Risk.Commands.PayLater.Uk.RiskPayLaterSaveEmploymentDetails </summary>
    [XmlRoot("RiskPayLaterSaveEmploymentDetails")]
    public partial class RiskPayLaterSaveEmploymentDetailsPayLaterUkCommand : ApiRequest<RiskPayLaterSaveEmploymentDetailsPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object EmploymentStatus { get; set; }
        public Object IncomeFrequency { get; set; }
        public Object NetIncome { get; set; }
        public Object NextPayDate { get; set; }
    }
}

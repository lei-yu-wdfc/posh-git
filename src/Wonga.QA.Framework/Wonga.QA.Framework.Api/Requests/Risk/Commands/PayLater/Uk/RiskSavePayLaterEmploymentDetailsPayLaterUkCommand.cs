using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    /// <summary> Wonga.Risk.Commands.PayLater.Uk.RiskSavePayLaterEmploymentDetails </summary>
    [XmlRoot("RiskSavePayLaterEmploymentDetails")]
    public partial class RiskSavePayLaterEmploymentDetailsPayLaterUkCommand : ApiRequest<RiskSavePayLaterEmploymentDetailsPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object EmploymentStatus { get; set; }
        public Object IncomeFrequency { get; set; }
        public Object NetIncome { get; set; }
        public Object NextPayDate { get; set; }
    }
}

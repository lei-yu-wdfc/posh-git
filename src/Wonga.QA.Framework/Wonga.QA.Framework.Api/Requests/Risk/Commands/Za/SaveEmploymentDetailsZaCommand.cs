using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Za
{
    /// <summary> Wonga.Risk.Commands.Za.SaveEmploymentDetails </summary>
    [XmlRoot("SaveEmploymentDetails")]
    public partial class SaveEmploymentDetailsZaCommand : ApiRequest<SaveEmploymentDetailsZaCommand>
    {
        public Object AccountId { get; set; }
        public Object NetMonthlyIncome { get; set; }
        public Object IncomeFrequency { get; set; }
        public Object NextPayDate { get; set; }
        public Object Status { get; set; }
        public Object EmploymentIndustry { get; set; }
        public Object EmploymentPosition { get; set; }
        public Object EmployerName { get; set; }
        public Object StartDate { get; set; }
        public Object PaidDirectDeposit { get; set; }
    }
}

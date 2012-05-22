using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Risk.Csapi.Commands.CsSaveEmploymentDetails </summary>
    [XmlRoot("CsSaveEmploymentDetails")]
    public partial class CsSaveEmploymentDetailsCommand : CsRequest<CsSaveEmploymentDetailsCommand>
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
    }
}

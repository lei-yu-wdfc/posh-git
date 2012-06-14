using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Pl
{
    /// <summary> Wonga.Risk.Commands.Pl.SaveEmploymentDetails </summary>
    [XmlRoot("SaveEmploymentDetails")]
    public partial class SaveEmploymentDetailsPlCommand : ApiRequest<SaveEmploymentDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object NetMonthlyIncome { get; set; }
        public Object IncomeFrequency { get; set; }
        public Object NextPayDate { get; set; }
        public Object Status { get; set; }
        public Object EmploymentIndustry { get; set; }
        public Object EmploymentPosition { get; set; }
        public Object EmployerName { get; set; }
        public Object PaidDirectDeposit { get; set; }
        public Object UniversityName { get; set; }
        public Object UniversityCity { get; set; }
        public Object UniversityType { get; set; }
        public Object YearsAtEmployer { get; set; }
        public Object MonthsAtEmployer { get; set; }
    }
}

using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Pl.SaveEmploymentDetailsPl </summary>
    [XmlRoot("SaveEmploymentDetailsPl")]
    public partial class SaveEmploymentDetailsPlCommand : ApiRequest<SaveEmploymentDetailsPlCommand>
    {
        public Object AccountId { get; set; }
        public Object NetMonthlyIncome { get; set; }
        public Object IncomeFrequency { get; set; }
        public Object NextPayDate { get; set; }
        public Object Status { get; set; }
        public Object StatusOther { get; set; }
        public Object EmploymentIndustry { get; set; }
        public Object EmploymentIndustryOther { get; set; }
        public Object UniversityType { get; set; }
        public Object EmployerName { get; set; }
        public Object YearsAtEmployer { get; set; }
        public Object MonthsAtEmployer { get; set; }
        public Object PaidDirectDeposit { get; set; }
    }
}

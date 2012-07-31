using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Pages.FinancialAssessment
{
    public sealed class FAAboutYouPage
    {
        public String AgreementReference { get; set; }
        public String EmailAddress { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String DateOfBirth { get; set; }
        public String HouseNumber { get; set; }
        public String PostCode { get; set; }
        public String Employer { get; set; }
        public String YourHousehold { get; set; }
        public String ChildrenInHousehold { get; set; }
        public String NumberOfVehiles { get; set; }
        public String ButtonPrevious { get; set; }
        public String ButtonNext { get; set; }
        public String PostCodeError { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Pages.Wb
{
    public sealed class EligibilityQuestionsPage
    {
        public String FormId { get; set; }
        public String CheckResident { get; set; }
        public String CheckDirector { get; set; }
        public String CheckActiveCompany { get; set; }
        public String CheckTurnover { get; set; }
        public String CheckVat { get; set; }
        public String CheckOnlineAccess { get; set; }
        public String CheckGuarantee { get; set; }
        public String NextButton { get; set; }
        public String CheckDebitCard { get; set; }
    }
}

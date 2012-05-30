using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings.Sections;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    /// <summary>
    /// Your Name Section
    /// </summary>
    public class YourNameSection
    {
        public String Fieldset { get; set; }
        public String Title { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleName { get; set; }
        public String FirstNameErrorForm { get; set; }
        public String MiddleNameErrorForm { get; set; }
        public String LastNameErrorForm { get; set; }
    }
}

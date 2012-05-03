using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.UI.Mappings.Sections
{
    public class AccountDetailsSection
    {
        public String Fieldset { get; set; }
        public String Password { get; set; }
        public String PasswordConfirm { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public String PasswordErrorForm { get; set; }
        public String PasswordConfirmErrorForm { get; set; }
        public String PasswordWarningMessage { get; set; }
    }
}

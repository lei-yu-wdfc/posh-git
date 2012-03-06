using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;
using System.Threading;

namespace Wonga.QA.Tests.Ui
{
    class MySummary : UiTest
    {

        [Test, AUT(AUT.Ca)]
        public void IsRepaymentWarningAvailableForLn()
        {
            var loginPage = Client.Login();

            string email = Data.GetEmail();

            var customer = CustomerBuilder.New().WithEmailAddress(email).Build();

            var summary = loginPage.LoginAs(email, Data.GetPassword());
            Console.WriteLine(summary.Title);
        }

    }
}

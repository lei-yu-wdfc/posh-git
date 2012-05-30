using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Ui;

namespace Wonga.QA.Tests.Admin.Prepaid
{
    public  class PrepaidAdminTest :UiTest
    {
        
        [Test,AUT(AUT.Uk),JIRA("PP-149")]
        public void AdminShouldSeePremiumCardMask()
        {
            Client.LoginPrepaidAdmin().LoginAs();
        }
    }
}

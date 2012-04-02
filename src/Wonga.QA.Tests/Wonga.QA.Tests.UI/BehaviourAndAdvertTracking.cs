using OpenQA.Selenium;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.UiElements;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.UI.UiElements.Pages.Wb;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Tests.Ui
{
    class BehaviourAndAdvertTracking : UiTest
    {
        [Test, AUT(AUT.Za)]
        public void L0VerifyDoubleclickTagInsertedInPage()
        {
            // Load the homepage:
            var page = Client.Home();

            // Check that the page contains the wonga_doubleclick module v1.0 signature:
            Assert.IsTrue(page.Client.Source().Contains(" wonga_doubleclick-v6.x-1.0-"));

            // Check that the page contains the correct doubleclick identifier for this AUT:
            Assert.IsTrue(page.Client.Source().Contains("src=3567941;"));

            // Check that the page contains the correct doubleclick category tag for this AUT:
            Assert.IsTrue(page.Client.Source().Contains("cat=za_ho244;"));
        }
    }
}

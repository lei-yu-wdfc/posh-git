using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui
{
    [TestFixture, Parallelizable(TestScope.All)]
    class ClickTest : UiTest
    {

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("CA-2326")]
        public void VerifyIsClickCodeLoaded()
        {
            var homePage = Client.Home();

            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"http.*click.*.js", RegexOptions.IgnoreCase));

        }

        [Test, AUT(AUT.Ca, AUT.Za), JIRA("CA-2326"), Pending("Nowicki - fixed it")]
        public void VerifyIsClickParamsSet()
        {
            var homePage = Client.Home();
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['tids'", RegexOptions.IgnoreCase));
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['region'", RegexOptions.IgnoreCase));
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['devicetype'", RegexOptions.IgnoreCase));
            Assert.IsTrue(Regex.IsMatch(homePage.Source, @"_clickconfiguration\.push\(\['devicegroup'", RegexOptions.IgnoreCase));

        }

    }
}